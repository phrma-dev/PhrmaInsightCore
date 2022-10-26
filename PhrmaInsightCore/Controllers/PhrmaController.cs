using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace PhrmaInsightCore.Controllers
{
    public class PhrmaController : Controller
    {

        public class Ticket { 
            public string UserEmail { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Priority { get; set; }
            public string SubCategory { get; set; }
            public string ClientId { get; set; }
        
        }

        string CWPS_Authorization_URL = "https://CWPS.com/api/Authorize_Client";
        string CWPS_Post_URL = "https://CWPS.com/api/Commit_New_Ticket_To_Database";
        string clientId = "phrma";
        private string _secret_key_ = "AKLJDJDIUPFDPEH0I5I0I5I5T4T548FDAKEIKDSGRIRUOHGHGURHOEIWHGJH4488F8F87GF7GYR4NJGN44";

        [HttpGet]
        public string Get_Authorization_Code_From_CWPS ()
        {

            // PhRMA Requests one time Encryption Key from CWPS using its secret key and client id


            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", _secret_key_);
            client.DefaultRequestHeaders.Add("ClientId", clientId);
            var response = client.GetAsync(CWPS_Authorization_URL);
            JObject JsonResult = JObject.Parse(response.Result.ToString());
            var encryptionKey = JsonResult["key"].ToString();

            return encryptionKey;
        }

        [HttpPost]
        public JsonResult Send_Task_To_CWPS([FromBody] Ticket ticket) 
        {

            // PhRMA Receives one time Encryption Key from CWPS

            string encryptionKey = Get_Authorization_Code_From_CWPS();

            // PhRMA uses Encryption Key to Encrypt JSON payload;

            Ticket newTicket = new Ticket();
            newTicket.UserEmail = Encrypt(ticket.UserEmail, encryptionKey);
            newTicket.Title = Encrypt(ticket.Title, encryptionKey);
            newTicket.Priority = Encrypt(ticket.Priority, encryptionKey);
            newTicket.Description = Encrypt(ticket.Description, encryptionKey);
            newTicket.SubCategory = Encrypt(ticket.SubCategory, encryptionKey);
            newTicket.ClientId = Encrypt(clientId, encryptionKey);

            var Json_Payload = Json(newTicket).ToString();

            // PhRMA sends JSON payload with the new ticket back to CWPS with the encryption key, secret key, and client id

            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("key", encryptionKey);
            client.DefaultRequestHeaders.Add("Authorization", _secret_key_);
            client.DefaultRequestHeaders.Add("ClientId", clientId);

            var response = client.PostAsync(CWPS_Post_URL, new StringContent(Json_Payload, Encoding.UTF8, "application/json"));
            JObject JsonResult = JObject.Parse(response.Result.ToString());

            if (JsonResult["message"].ToString() == "success")
            {

                return Json("success");
            }
            else {

                return Json("failure");
            }
        }

        public string Encrypt(string value, string KeyValue)
        {
            if (string.IsNullOrEmpty(value)) return value;
            try
            {
                var key = Encoding.UTF8.GetBytes(KeyValue);

                using (var aesAlg = Aes.Create())
                {
                    using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV))
                    {
                        using (var msEncrypt = new MemoryStream())
                        {
                            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            using (var swEncrypt = new StreamWriter(csEncrypt))
                            {
                                swEncrypt.Write(value);
                            }

                            var iv = aesAlg.IV;

                            var decryptedContent = msEncrypt.ToArray();

                            var result = new byte[iv.Length + decryptedContent.Length];

                            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                            Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                            var str = Convert.ToBase64String(result);
                            var fullCipher = Convert.FromBase64String(str);
                            return str;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }




        [HttpGet]
        public JsonResult Authorize_Client() {

            var secretKey = Request.Headers["Authorization"].ToString();
            var clientId = Request.Headers["ClientId"].ToString();

            // CWPS checks PhRMA's Secret Key and Client Id to verify it's us and then provides a one time Encryption Key. 

            // Code to check secretKey with clientId in database goes here 
            // If validated then run code below

            var encryptionKey = "{ \"key\" : " + Guid.NewGuid().ToString("N").ToUpper() + "\"";
           

            return Json(encryptionKey);
        }

        [HttpPost]
        public string Commit_New_Ticket_To_Database([FromBody] Ticket ticket) {


            var secretKey = Request.Headers["Authorization"].ToString();
            var clientId = Request.Headers["ClientId"].ToString();
            var encryptionKey = Request.Headers["key"];

            // CWPS checks secretKey with clientId in database again to validate

            // if validated then run code below


            // CWPS submits ticket to AutoTrack databasd

            Ticket newTicket = new Ticket();

            newTicket.UserEmail = Decrypt(ticket.UserEmail, encryptionKey);
            newTicket.Title = Decrypt(ticket.Title, encryptionKey);
            newTicket.Priority = Decrypt(ticket.Priority, encryptionKey);
            newTicket.Description = Decrypt(ticket.Description, encryptionKey);
            newTicket.SubCategory = Decrypt(ticket.SubCategory, encryptionKey);
            newTicket.ClientId = Decrypt(clientId, encryptionKey);

            // Database.Save(newTicket);

            return "ok";
        }

        public string Decrypt(string value, string KeyValue)
        {
            if (string.IsNullOrEmpty(value)) return value;
            try
            {
                value = value.Replace(" ", "+");
                var fullCipher = Convert.FromBase64String(value);

                var iv = new byte[16];
                var cipher = new byte[fullCipher.Length - iv.Length];

                Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, fullCipher.Length - iv.Length);
                var key = Encoding.UTF8.GetBytes(KeyValue);

                using (var aesAlg = Aes.Create())
                {
                    using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                    {
                        string result;
                        using (var msDecrypt = new MemoryStream(cipher))
                        {
                            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                            {
                                using (var srDecrypt = new StreamReader(csDecrypt))
                                {
                                    result = srDecrypt.ReadToEnd();
                                }
                            }
                        }

                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }


    }
}
