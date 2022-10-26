using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Graph;
using Microsoft.Graph.Auth;
using Microsoft.Graph.Extensions;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhrmaInsightCore.Models;
using PhrmaInsightCore.Models.DB;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;


namespace PhrmaInsightCore.Controllers
{
    [Authorize]
    public class ActiveDirectoryController : Controller
    {


        DatabaseContext _context;
        DatabaseContext _delete;

        public ActiveDirectoryController(DatabaseContext context, DatabaseContext delete)
        {

            _context = context;
            _delete = delete;
        }


        [AllowAnonymous]
        public async Task<List<AzureADUsers>> AlertMediaSync()
        {
            var URL = "https://api.alertmedia.com/api/users";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Basic OTBkZTBmNjMtYjdhOS00MDU3LTgzMDAtMGNhYTQ0M2VjMzVmOmIwNjQ5YzNmN2JlMGY4NGFhODI0N2ZmN2Q2OGVlMTdm");
            client.DefaultRequestHeaders.Add("Range", "items=0-500");
            var response = await client.GetAsync(URL);
            var result = await response.Content.ReadAsStringAsync();
            
            var jsonResult = JArray.Parse(result);

            List<AzureADUsers> AMList = new List<AzureADUsers>();
            for (int i = 0; i < jsonResult.Count; i++)
            {
                AzureADUsers AMUser = new AzureADUsers();
                AMUser.alertmediaid = jsonResult[i]["id"].ToString();
                AMUser.USER_PRINCIPAL_NAME = jsonResult[i]["email"].ToString();

                AMList.Add(AMUser);
            }
            return AMList;
        }


        [AllowAnonymous]
        [HttpGet]
        // Load user's profile.
        public async Task<JsonResult> GetGroupEvents(string groupid)
        {
            var access_token = await GetToken();

 
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "bearer " + access_token);
            var URL = "https://graph.microsoft.com/v1.0/groups/"+ groupid + "/calendar/events";

            var req = await client.GetAsync(URL);
            var response = req.Content.ReadAsStringAsync();
            Newtonsoft.Json.Linq.JObject jsonObject = JObject.Parse(response.Result.ToString());
            var json = jsonObject["value"];
            return Json(json);
        }



        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> UpdateUsers()
        {


            var all = _delete.AzureADUsers.ToList();
            _delete.AzureADUsers.RemoveRange(all);

            var alertMediaList = await AlertMediaSync();

            var access_token = await GetToken();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "bearer " + access_token);
            var URL = "https://graph.microsoft.com/v1.0/groups/70880266-e045-4455-8530-134adb83ae22/members?$select=department,givenName,surname,userPrincipalName,officeLocation,jobTitle,mobilePhone&$top=300";
            var req = await client.GetAsync(URL);
            var response = req.Content.ReadAsStringAsync();
            Newtonsoft.Json.Linq.JObject jsonObject = JObject.Parse(response.Result.ToString());
            var employees = jsonObject["value"];

            List<AzureADUsers> users = new List<AzureADUsers>();
            for (int i = 0; i < employees.Count(); i++)
            {
                AzureADUsers user = new AzureADUsers();
                user.USER_PRINCIPAL_NAME = employees[i]["userPrincipalName"].ToString().ToLower();
                user.GIVEN_NAME = employees[i]["givenName"].ToString();
                user.SURNAME = employees[i]["surname"].ToString();
                user.OFFICE_LOCATION = employees[i]["officeLocation"].ToString();
                user.TITLE = employees[i]["jobTitle"].ToString();
                user.DEPARTMENT = employees[i]["department"].ToString();
                user.MOBILE_PHONE = employees[i]["mobilePhone"].ToString();

                try
                {
                    var manager_URL = "https://graph.microsoft.com/v1.0/users/" + employees[i]["userPrincipalName"].ToString().ToLower() + "/manager";
                    var m_req = await client.GetAsync(manager_URL);
                    var m_response = m_req.Content.ReadAsStringAsync();
                    Newtonsoft.Json.Linq.JObject m_jsonObject = JObject.Parse(m_response.Result.ToString());
                    var manager = m_jsonObject["userPrincipalName"].ToString().ToLower();
                    user.AZURE_AD_MANAGER = manager;
                }
                catch (Exception)
                {

                    user.AZURE_AD_MANAGER = "";
                }

                try
                {
                    user.alertmediaid = alertMediaList.Where(a => a.USER_PRINCIPAL_NAME == employees[i]["userPrincipalName"].ToString().ToLower()).Select(a => a.alertmediaid).FirstOrDefault().ToString();
                }
                catch (Exception)
                {

                    user.alertmediaid = "";
                }

                users.Add(user);
            }

            _context.AzureADUsers.AddRange(users);
            _context.SaveChanges();

            return Json("OK");

        }

        public async Task<string> GetToken()
        {
            var dict = new Dictionary<string, string>() 
            {
                
                {"client_id", "6ffd2f73-0453-4ed0-b13d-1113d598427d"},
                {"scope", "https://graph.microsoft.com/.default"},
                {"client_secret", "XzAu040LT3kYjZuZKqmRCN=Hq?AxfA:]" },
                {"grant_type", "client_credentials"}

            };
            string authorityUri = "https://login.microsoftonline.com/794a0e5b-d96f-4b71-a78a-4456c6b5486a/oauth2/v2.0/token";
            var client = new HttpClient();
            var req = new HttpRequestMessage(HttpMethod.Post, authorityUri) { Content = new FormUrlEncodedContent(dict) };
            var res = await client.SendAsync(req);
            var result = res.Content.ReadAsStringAsync();
            var json = JObject.Parse(result.Result.ToString());
            var jsonResult = json["access_token"].ToString();
            return jsonResult;
        }
    }
}
