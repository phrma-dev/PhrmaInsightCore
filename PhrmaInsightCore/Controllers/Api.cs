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
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhrmaInsightCore.Models;
using PhrmaInsightCore.Models.DB;
using Microsoft.Graph;
using Microsoft.Graph.Core;
using Microsoft.Graph.Auth;
using Microsoft.Graph.Extensions;
using Microsoft.Identity.Client;
using Microsoft.Graph.Core.Requests;

namespace PhrmaInsightCore.Controllers
{
    public class ApiController : Controller
    {
        public string ClientID = "6ffd2f73-0453-4ed0-b13d-1113d598427d";
        public string AADAuthorityUri = "https://login.microsoftonline.com/common/oauth2/authorize/";
        public string ClientSecret = "XzAu040LT3kYjZuZKqmRCN=Hq?AxfA:]";
        DatabaseContext _context;
       

        public ApiController(DatabaseContext context)
        {
            _context = context;
        }

        [HttpGet]
        public JsonResult GetPhrmaUsers()
        {
            var users = _context.AzureADUsers.ToList();
            return Json(users);
        }
        [AllowAnonymous]
        public async Task<JsonResult> PhrmaUsers()
        {
            var access_token = await GetToken();
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "bearer " + access_token);
            var URL = "https://graph.microsoft.com/v1.0/groups/495b894d-d64a-4eb4-9ace-a32d2d62e275/members?$select=department,givenName,surname,userPrincipalName,officeLocation,jobTitle,mobilePhone,businessPhones&$top=300";
            //"https://graph.microsoft.com/v1.0/groups/70880266-e045-4455-8530-134adb83ae22/members?$select=department,givenName,surname,userPrincipalName,officeLocation,jobTitle,mobilePhone&$top=300";
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
                user.OFFICE_PHONE = employees[i]["businessPhones"][0].ToString();

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


                users.Add(user);
            }
            return Json(users);

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
