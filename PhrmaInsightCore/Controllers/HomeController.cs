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

namespace PhrmaInsightCore.Controllers
{

    public class HomeController : Controller
    {

   
        private readonly ILogger<HomeController> _logger;
        string ClientID = "6ffd2f73-0453-4ed0-b13d-1113d598427d";
        string PowerBiAPI = "https://analysis.windows.net/powerbi/api";
        string AADAuthorityUri = "https://login.microsoftonline.com/common/oauth2/authorize/";
        string ClientSecret = "XzAu040LT3kYjZuZKqmRCN=Hq?AxfA:]";
        //Uri Uri = new Uri("https://localhost:44333/");
        Uri Uri = new Uri("https://phrma.azurewebsites.net/");
        string baseUri = "https://api.powerbi.com/v1.0/myorg/";
        DatabaseContext _context;
        DatabaseContext _secondContext;

        public HomeController(ILogger<HomeController> logger, DatabaseContext context, DatabaseContext secondContext)
        {
            _logger = logger;
            _context = context;
            _secondContext = secondContext;
        }

        public IActionResult Test() 
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Thanks() 
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        //public IActionResult Main()
        //{

        //    ViewBag.Image = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Image).FirstOrDefault().ToString();
        //    ViewBag.Name = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Name).FirstOrDefault().ToString();
        //    ViewBag.Title = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Title).FirstOrDefault().ToString();
        //    ViewBag.Department = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Department).FirstOrDefault().ToString();
        //    ViewBag.DepartmentId = _context.Security.Where(a => a.Username == User.Identity.Name.ToLower()).Select(a => a.DepartmentId).FirstOrDefault().ToString();
        //    ViewBag.OfficeLocation = _context.UserInfo.Where(x => x.FullUserName == User.Identity.Name).Select(x => x.OfficeLocation).FirstOrDefault().ToString();
        //    ViewBag.WeatherUrl = _context.WeatherWidgets.Where(a => a.Location == _context.UserInfo.Where(x => x.FullUserName == User.Identity.Name.ToLower()).Select(x => x.OfficeLocation).FirstOrDefault().ToString()).Select(a => a.WeatherURL).ToString();
        //    return View();

        //}
        public IActionResult Main()
        {
            return Redirect("../Finance/Dashboard");
        }
        public IActionResult Index() {
            return Redirect("../Finance/Dashboard");
        }


        //[HttpGet]
        //public async Task<IActionResult> Index(string code)
        //{

        //    bool knownUser = _context.UserInfo.Select(a => a.FullUserName).ToList().Contains(User.Identity.Name.ToLower());

        //    if (!knownUser)
        //    {
        //        GetCode();
        //    }
        //    else
        //    { }

        //    if (code == null)
        //    {
        //        GetAuthorizationCode();
        //    }


        //    if (code != null)
        //    {
        //        var authenticationResult = await GetToken(code);
        //        var accessToken = authenticationResult.AccessToken;

        //        ViewBag.AccessToken = accessToken;
        //        Configure Reports request
        //        System.Net.WebRequest request = System.Net.WebRequest.Create(
        //        String.Format("{0}/Reports",
        //        baseUri)) as System.Net.HttpWebRequest;

        //        request.Method = "GET";
        //        request.ContentLength = 0;
        //        request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken));
        //    }

        //    return View();
        //}

        public void GetCode()
            {
              var @params = new NameValueCollection
                            {
                                //Azure AD will return an authorization code. 
                                //See the Redirect class to see how "code" is used to AcquireTokenByAuthorizationCode
                                {"response_type", "code"},

                                //Client ID is used by the application to identify themselves to the users that they are requesting permissions from. 
                                //You get the client id when you register your Azure app.
                                {"client_id", ClientID},

                                //Resource uri to the Power BI resource to be authorized
                                // https://analysis.windows.net/powerbi/api
                                {"resource", "https://graph.microsoft.com/"},

                                //After user authenticates, Azure AD will redirect back to the web app
                                { "redirect_uri", "https://phrma.azurewebsites.net/Home/UserDetail/"}
                                //{ "redirect_uri", "https://phrma.azurewebsites.net/Finance/Dashboard"}
                            };

              //Create sign-in query string
              var queryString = HttpUtility.ParseQueryString(string.Empty);
              queryString.Add(@params);

              //Redirect authority
              //Authority Uri is an Azure resource that takes a client id to get an Access token
              // AADAuthorityUri = https://login.windows.net/common/oauth2/authorize/
              string authorityUri = AADAuthorityUri;
              var authUri = String.Format("{0}?{1}", authorityUri, queryString);
              Response.Redirect(authUri);
            }


        protected void GetAuthorizationCode()
        {
          //Create a query string
          //Create a sign-in NameValueCollection for query string
          var @params = new NameValueCollection
              {
                  //Azure AD will return an authorization code. 
                  //See the Redirect class to see how "code" is used to AcquireTokenByAuthorizationCode
                  {"response_type", "code"},

                  //Client ID is used by the application to identify themselves to the users that they are requesting permissions from. 
                  //You get the client id when you register your Azure app.
                  {"client_id", ClientID},

                  //Resource uri to the Power BI resource to be authorized
                  // https://analysis.windows.net/powerbi/api
                  {"resource", PowerBiAPI},

                  //After user authenticates, Azure AD will redirect back to the web app
                  //{"redirect_uri", "https://localhost:44333/"}
                  {"redirect_uri", "https://phrma.azurewebsites.net/"}
              };

          //Create sign-in query string
          var queryString = HttpUtility.ParseQueryString(string.Empty);
          queryString.Add(@params);
          string authorityUri = AADAuthorityUri;
          var authUri = String.Format("{0}?{1}", authorityUri, queryString);
          Response.Redirect(authUri);


        }

        public async Task<AuthenticationResult> GetToken(string code)
        {

          TokenCache TC = new TokenCache();
          AuthenticationContext AC = new AuthenticationContext(AADAuthorityUri, TC);
          ClientCredential cc = new ClientCredential(ClientID, ClientSecret);
          AuthenticationResult AR = await AC.AcquireTokenByAuthorizationCodeAsync(code, Uri, cc);

          return AR;
        }




        [AllowAnonymous]
        public async Task<IActionResult> FederalAdvocacy(string code)
        {
          if (code == null)
          {
            GetCodeFA();
          }
          else
          {

            TokenCache TC = new TokenCache();
            AuthenticationContext AC = new AuthenticationContext(AADAuthorityUri, TC);
            ClientCredential cc = new ClientCredential(ClientID, ClientSecret);
            //AuthenticationResult AR = await AC.AcquireTokenByAuthorizationCodeAsync(code, new Uri("https://localhost:44366/Home/FederalAdvocacy/"), cc);
            AuthenticationResult AR = await AC.AcquireTokenByAuthorizationCodeAsync(code, new Uri("https://phrma.azurewebsites.net/Home/FederalAdvocacy/"), cc);
            var authenticationResult = AR;
            var accessToken = authenticationResult.AccessToken;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken.ToString());

            var meetingsResult = await client.GetAsync("https://graph.microsoft.com/v1.0/sites/phrma.sharepoint.com,fe30a36a-54d5-4ca9-98a5-126d47970377,c11ec6c7-f725-4679-ad63-b22cda350192/lists/PhRMA%20Meetings/items");
            var meetingsResponse = meetingsResult.Content.ReadAsStringAsync();

            Newtonsoft.Json.Linq.JObject meetingObject = JObject.Parse(meetingsResponse.Result.ToString());
            List<int> meetingIds = new List<int>();
            int k = 0;
            foreach (var item in meetingObject["value"])
            {
              meetingIds.Add(Convert.ToInt32(meetingObject["value"][k]["id"].ToString()));
              k++;
            }

            List<BoardMeetingTracker> meetingsList = new List<BoardMeetingTracker>();

            foreach (var meetingId in meetingIds)
            {
              try
              {
                BoardMeetingTracker newMeeting = new BoardMeetingTracker();
                var URL = "https://graph.microsoft.com/v1.0/sites/phrma.sharepoint.com,fe30a36a-54d5-4ca9-98a5-126d47970377,c11ec6c7-f725-4679-ad63-b22cda350192/lists/PhRMA%20Meetings/items/" + meetingId.ToString();
                var result = await client.GetAsync(URL);
                var response = result.Content.ReadAsStringAsync();
                Newtonsoft.Json.Linq.JObject jsonObject = JObject.Parse(response.Result.ToString());
                newMeeting.Date = Convert.ToDateTime(jsonObject["fields"]["DateofMeeting"]);
                newMeeting.MemberName = jsonObject["fields"]["Title"].ToString();
                foreach (var item in JArray.Parse(JsonConvert.SerializeObject(jsonObject["fields"]["PhRMA_x0020_Attendees"])).ToObject<List<string>>())
                {
                  newMeeting.Phrma_Attendees += "!" + item.ToString();
                }
                foreach (var item in JArray.Parse(JsonConvert.SerializeObject(jsonObject["fields"]["Board_x0020_Attendees"])).ToObject<List<string>>())
                {
                  newMeeting.Board_Attendees += "!" + item.ToString();
                }

                newMeeting.Topic = jsonObject["fields"]["Topic"].ToString();
                meetingsList.Add(newMeeting);
              }
              catch (Exception)
              {

              }

            }

            var docResult = await client.GetAsync("https://graph.microsoft.com/v1.0/sites/phrma.sharepoint.com,fe30a36a-54d5-4ca9-98a5-126d47970377,c11ec6c7-f725-4679-ad63-b22cda350192/lists/Documents/items");
            var docResponse = docResult.Content.ReadAsStringAsync();




            Newtonsoft.Json.Linq.JObject jsonObjectItemCount = JObject.Parse(docResponse.Result.ToString());

            List<int> docIds = new List<int>();
            int g = 0;
            foreach (var item in jsonObjectItemCount["value"])
            {
              docIds.Add(Convert.ToInt32(jsonObjectItemCount["value"][g]["id"].ToString()));
              g++;
            }

            List<SharePointDocument> ListShp = new List<SharePointDocument>();
            List<SharePointDocumentStore> ListShpDS = new List<SharePointDocumentStore>();
            int q = 0;
            foreach (var docId in docIds)
            {
              var URL = "https://graph.microsoft.com/v1.0/sites/phrma.sharepoint.com,fe30a36a-54d5-4ca9-98a5-126d47970377,c11ec6c7-f725-4679-ad63-b22cda350192/lists/Documents/items/" + docId.ToString();
              var result = await client.GetAsync(URL);
              var response = result.Content.ReadAsStringAsync();
              Newtonsoft.Json.Linq.JObject jsonObject = JObject.Parse(response.Result.ToString());
              List<string> Members = new List<string>();

              try
              {
                var members = jsonObject["fields"]["Members"];
                SharePointDocument shp = new SharePointDocument();
                SharePointDocumentStore sharePointDocumentStore = new SharePointDocumentStore();
                shp.Web_Url = jsonObject["webUrl"].ToString();
                shp.Document_Name = jsonObject["fields"]["FileLeafRef"].ToString();
                shp.Document_Type = jsonObject["fields"]["Document_x0020_Description"].ToString();
                sharePointDocumentStore.Web_Url = jsonObject["webUrl"].ToString();
                sharePointDocumentStore.Document_Name = jsonObject["fields"]["FileLeafRef"].ToString();
                sharePointDocumentStore.Document_Type = jsonObject["fields"]["Document_x0020_Description"].ToString();
                try
                {
                  shp.DateOfMeeting = jsonObject["fields"]["Meeting_x0020_Date"].ToString();
                  sharePointDocumentStore.DateOfMeeting = jsonObject["fields"]["Meeting_x0020_Date"].ToString();
                }
                catch (Exception)
                {

                }
                try
                {
                  shp.State = jsonObject["fields"]["State"].ToString();
                  sharePointDocumentStore.State = jsonObject["fields"]["State"].ToString();
                }
                catch (Exception)
                {

                }

                var MemberArray = JArray.Parse(JsonConvert.SerializeObject(jsonObject["fields"]["Members"])).ToObject<List<string>>();
                string memberstring = "";
                int z = 0;

                foreach (var member in MemberArray)
                {
                  Members.Add(member.ToString());
                  if (z != 0)
                  {
                    memberstring += "#";
                  }
                  memberstring += member.ToString();
                  z++;
                }
                sharePointDocumentStore.MembersString = memberstring;
                ListShpDS.Add(sharePointDocumentStore);


                shp.Members = Members;
                ListShp.Add(shp);
              }
              catch (Exception)
              {
              }

              q++;
            }
            _context.SharePointDocumentStore.RemoveRange(_context.SharePointDocumentStore.ToList());

            _context.SharePointDocumentStore.AddRange(ListShpDS);
            _context.SaveChanges();

            ViewBag.Meetings = meetingsList;
            ViewBag.MemberNotes = _context.MemberNotes.ToList();
            ViewBag.Result = ListShp;
          }
          ViewBag.HouseReps = _context.HouseMembers.ToList();
          return View();
        }

        [AllowAnonymous]
        public async Task<IActionResult> FederalAdvocacyFastLoad(string code)
        {
          if (code == null)
          {
            GetCodeFAFASTLOAD();
          }
          else
          {
            TokenCache TC = new TokenCache();
            AuthenticationContext AC = new AuthenticationContext(AADAuthorityUri, TC);
            ClientCredential cc = new ClientCredential(ClientID, ClientSecret);
            //AuthenticationResult AR = await AC.AcquireTokenByAuthorizationCodeAsync(code, new Uri("https://localhost:44333/Home/FederalAdvocacyFastLoad/"), cc);
            AuthenticationResult AR = await AC.AcquireTokenByAuthorizationCodeAsync(code, new Uri("https://phrma.azurewebsites.net/Home/FederalAdvocacyFastLoad/"), cc);
            var authenticationResult = AR;
            var accessToken = authenticationResult.AccessToken;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken.ToString());

            var meetingsResult = await client.GetAsync("https://graph.microsoft.com/v1.0/sites/phrma.sharepoint.com,fe30a36a-54d5-4ca9-98a5-126d47970377,c11ec6c7-f725-4679-ad63-b22cda350192/lists/PhRMA%20Meetings/items");
            var meetingsResponse = meetingsResult.Content.ReadAsStringAsync();

            Newtonsoft.Json.Linq.JObject meetingObject = JObject.Parse(meetingsResponse.Result.ToString());
            List<int> meetingIds = new List<int>();
            int k = 0;
            foreach (var item in meetingObject["value"])
            {
              meetingIds.Add(Convert.ToInt32(meetingObject["value"][k]["id"].ToString()));
              k++;
            }

            List<BoardMeetingTracker> meetingsList = new List<BoardMeetingTracker>();

            foreach (var meetingId in meetingIds)
            {
              try
              {
                BoardMeetingTracker newMeeting = new BoardMeetingTracker();
                var URL = "https://graph.microsoft.com/v1.0/sites/phrma.sharepoint.com,fe30a36a-54d5-4ca9-98a5-126d47970377,c11ec6c7-f725-4679-ad63-b22cda350192/lists/PhRMA%20Meetings/items/" + meetingId.ToString();
                var result = await client.GetAsync(URL);
                var response = result.Content.ReadAsStringAsync();
                Newtonsoft.Json.Linq.JObject jsonObject = JObject.Parse(response.Result.ToString());
                newMeeting.Date = Convert.ToDateTime(jsonObject["fields"]["DateofMeeting"]);
                newMeeting.MemberName = jsonObject["fields"]["Title"].ToString();
                foreach (var item in JArray.Parse(JsonConvert.SerializeObject(jsonObject["fields"]["PhRMA_x0020_Attendees"])).ToObject<List<string>>())
                {
                  newMeeting.Phrma_Attendees += "!" + item.ToString();
                }
                foreach (var item in JArray.Parse(JsonConvert.SerializeObject(jsonObject["fields"]["Board_x0020_Attendees"])).ToObject<List<string>>())
                {
                  newMeeting.Board_Attendees += "!" + item.ToString();
                }

                newMeeting.Topic = jsonObject["fields"]["Topic"].ToString();
                meetingsList.Add(newMeeting);
              }
              catch (Exception)
              {

              }

            }

            List<SharePointDocument> ListShp = new List<SharePointDocument>();

            foreach (var item in _context.SharePointDocumentStore.ToList())
            {
              SharePointDocument shp = new SharePointDocument();
              shp.DateOfMeeting = item.DateOfMeeting;
              shp.Document_Name = item.Document_Name;
              shp.Document_Type = item.Document_Type;
              shp.Members = item.MembersString.Split("#").ToList();
              shp.State = item.State;
              shp.Web_Url = item.Web_Url;
              shp.Id = item.Id;
              ListShp.Add(shp);
            }

            ViewBag.Meetings = meetingsList;
            ViewBag.MemberNotes = _context.MemberNotes.ToList();
            ViewBag.Result = ListShp;
          }
          ViewBag.HouseReps = _context.HouseMembers.ToList();
          return View();
        }

        [HttpPost]
        public JsonResult MemberNotes([FromBody] MemberNote note)
        {
          MemberNote new_note = new MemberNote();
          new_note.MemberId = note.MemberId;
          new_note.User = User.Identity.Name.ToLower();
          new_note.Note = note.Note;
          if (!_context.MemberNotes.Select(a => a.MemberId).Contains(new_note.MemberId))
          {
            _context.MemberNotes.Add(new_note);
            _context.SaveChanges();
          }
          else
          {
            MemberNote updateNote = _context.MemberNotes.Where(a => a.MemberId == note.MemberId).FirstOrDefault();
            updateNote.Note = note.Note;
            _context.MemberNotes.Update(updateNote);
            _context.SaveChanges();
          }

          return Json(note.Note.ToString());
        }



        [HttpGet]
        public JsonResult GetCongressData(string StateId)
        {
          var data = _context.CongressDistricts.Where(a => a.StateId == StateId).FirstOrDefault();
          return Json(data);
        }

        [HttpGet]
        public JsonResult PostNewForecastData(string project, string location, string department, string vendor, string mainaccount, string referencecode)
        {


          bool isAlreadyInTable;


          try
          {
            isAlreadyInTable = _context.SoftCommitmentForecast.Contains(_context.SoftCommitmentForecast.Where(a => a.ProjectId == project && a.LocationId == location.Replace("%20", " ") && a.DepartmentId == department && a.VendorId == vendor && a.MainAccountId == mainaccount && a.ReferenceCodeId == referencecode && a.EntryCategory == "1").FirstOrDefault());

          }
          catch (Exception)
          {

            isAlreadyInTable = false;
          }

          if (!isAlreadyInTable)
          {

            SoftCommitmentForecast item = new SoftCommitmentForecast();
            item.ProjectId = project;
            item.ProjectName = _context.Projects.Where(a => a.ProjectId == project).Select(a => a.ProjectName).First().ToString();
            item.LocationId = location;
            item.ReferenceCodeId = referencecode;
            item.Note = "...";
            item.MonthId = "5";
            

            if (referencecode != "None")
            {
              item.ReferenceCodeName = referencecode;
            }
            else
            {
              item.ReferenceCodeName = "None";
            }

            item.EntryCategory = "1";
            item.Year = "2019";
            if (location == "None")
            {
              item.LocationName = "None";
              item.RegionName = "None";
            }
            else
            {
              item.LocationName = _context.Locations.Where(a => a.LocationId == location).Select(a => a.LocationName).First().ToString();
              item.RegionName = _context.Locations.Where(a => a.LocationId.TrimStart() == location.TrimStart()).Select(a => a.RegionName).First().ToString();
            }


            item.DepartmentId = department;
            item.DepartmentName = _context.Departments.Where(a => a.DepartmentId == department).Select(a => a.DepartmentName).First().ToString();
            item.VendorId = vendor;

            if (vendor == "Budget Entry")
            {
              item.VendorName = "Budget Entry";
            }
            else
            {
              item.VendorName = _context.Vendors.Where(a => a.VendorId == vendor).Select(a => a.VendorName).First().ToString();
            }
            item.MainAccountId = mainaccount;
            item.MainAccountName = _context.MainAccounts.Where(a => a.MainAccountId == mainaccount).Select(a => a.MainAccountName).First().ToString();
            item.LastModifiedByUser = User.Identity.Name.ToString();
            item.LastModifiedDateTime = DateTime.UtcNow.ToString();
            item.AccountCategoryId = _context.MainAccounts.Where(a => a.MainAccountId == mainaccount).Select(a => a.AccountCategoryId).First().ToString();
            item.AccountCategory = _context.MainAccounts.Where(a => a.MainAccountId == mainaccount).Select(a => a.AccountCategory).First().ToString();
            // item.LastModifiedByUserImage = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name.ToLower()).Select(a => a.Image).FirstOrDefault();

            _context.SoftCommitmentForecast.Add(item);
            _context.SaveChanges();
            if (isAlreadyInTable == false)
            {
              return Json("success");
            }
            else
            {
              return Json("failure");
            }

          }

          if (isAlreadyInTable == false)
          {
            return Json("success");
          }
          else
          {
            return Json("failure");
          }
        }

        [HttpGet]
        public JsonResult PostNewNextYearBudgetData(string project, string location, string department, string vendor, string mainaccount, string referencecode)
        {


          bool isAlreadyInTable;


          try
          {
            isAlreadyInTable = _context.Budgeting.Contains(_context.Budgeting.Where(a => a.ProjectId == project && a.LocationId == location.Replace("%20", " ") && a.DepartmentId == department && a.VendorId == vendor && a.MainAccountId == mainaccount && a.ReferenceCodeId == referencecode).FirstOrDefault());

          }
          catch (Exception)
          {

            isAlreadyInTable = false;
          }

          if (!isAlreadyInTable)
          {

            Budgeting item = new Budgeting();
            item.ProjectId = project;
            item.ProjectName = _context.Projects.Where(a => a.ProjectId == project).Select(a => a.ProjectName).First().ToString();
            item.LocationId = location;
            item.ReferenceCodeId = referencecode;
            item.Note = "...";
            item.ASA_Region =  location != "None" ? _context.ASARegion.Where(a => a.LOCATIONID == location).Select(a => a.ASA_REGION).First().ToString() : "None";


            if (referencecode != "None")
            {
              item.ReferenceCodeName = referencecode;
            }
            else
            {
              item.ReferenceCodeName = "None";
            }


            item.Year = "2020";
            if (location == "None")
            {
              item.LocationName = "None";
              item.RegionName = "None";
            }
            else
            {
              item.LocationName = _context.Locations.Where(a => a.LocationId == location).Select(a => a.LocationName).First().ToString();
              item.RegionName = _context.Locations.Where(a => a.LocationId.TrimStart() == location.TrimStart()).Select(a => a.RegionName).First().ToString();
            }


            item.DepartmentId = department;
            item.DepartmentName = _context.Departments.Where(a => a.DepartmentId == department).Select(a => a.DepartmentName).First().ToString();
            item.VendorId = vendor;

            if (vendor == "Budget Entry")
            {
              item.VendorName = "Budget Entry";
            }
            else
            {
              item.VendorName = _context.Vendors.Where(a => a.VendorId == vendor).Select(a => a.VendorName).First().ToString();
            }
            item.MainAccountId = mainaccount;
            item.MainAccountName = _context.MainAccounts.Where(a => a.MainAccountId == mainaccount).Select(a => a.MainAccountName).First().ToString();
            item.AccountCategoryId = _context.MainAccounts.Where(a => a.MainAccountId == mainaccount).Select(a => a.AccountCategoryId).First().ToString();
            item.AccountCategory = _context.MainAccounts.Where(a => a.MainAccountId == mainaccount).Select(a => a.AccountCategory).First().ToString();
            // item.LastModifiedByUserImage = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name.ToLower()).Select(a => a.Image).FirstOrDefault();

            _context.Budgeting.Add(item);
            _context.SaveChanges();
            if (isAlreadyInTable == false)
            {
              return Json("success");
            }
            else
            {
              return Json("failure");
            }

          }

          if (isAlreadyInTable == false)
          {
            return Json("success");
          }
          else
          {
            return Json("failure");
          }
        }


        public IActionResult SharepointAnalytics()
        {

          ViewBag.Image = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Image).FirstOrDefault().ToString();
          ViewBag.Name = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Name).FirstOrDefault().ToString();
          ViewBag.Title = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Title).FirstOrDefault().ToString();
          ViewBag.Department = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Department).FirstOrDefault().ToString();
          ViewBag.OfficeLocation = _context.UserInfo.Where(x => x.FullUserName == User.Identity.Name).Select(x => x.OfficeLocation).FirstOrDefault().ToString();
          ViewBag.WeatherUrl = _context.WeatherWidgets.Where(a => a.Location == _context.UserInfo.Where(x => x.FullUserName == User.Identity.Name).Select(x => x.OfficeLocation).Distinct().ToString()).Select(a => a.WeatherURL).ToString();
          ViewBag.DepartmentId = _context.Security.Where(a => a.Username == User.Identity.Name.ToLower()).Select(a => a.DepartmentId).FirstOrDefault().ToString();
          return View();
        }

        [HttpGet]
        public JsonResult LoginTracking()
        {
          return Json("success");
        }

        [HttpGet]
        public JsonResult AddItemTracking()
        {

          UserActivity userActivity = new UserActivity();
          userActivity.FullUserName = User.Identity.Name.ToLower();
          userActivity.Page = "Forecast";
          userActivity.Activity = "Add Item";
          _context.UserActivity.Add(userActivity);
          _context.SaveChanges();

          return Json("success");
        }

        [HttpGet]
        public JsonResult TrackPowerBIPage(string report)
        {

          UserActivity userActivity = new UserActivity();
          userActivity.FullUserName = User.Identity.Name.ToLower();
          userActivity.Page = "Finance Power BI";
          userActivity.Activity = report;
          _context.UserActivity.Add(userActivity);
          _context.SaveChanges();

          return Json("success");
        }

        [HttpGet]
        public JsonResult ForecastQueryTracking()
        {

          UserActivity userActivity = new UserActivity();
          userActivity.FullUserName = User.Identity.Name.ToLower();
          userActivity.Page = "Forecast";
          userActivity.Activity = "Query Data";
          _context.UserActivity.Add(userActivity);
          _context.SaveChanges();

          return Json("success");
        }


        [HttpGet]
        public JsonResult UpdateItemSave()
        {

          UserActivity userActivity = new UserActivity();
          userActivity.FullUserName = User.Identity.Name.ToLower();
          userActivity.Page = "Forecast";
          userActivity.Activity = "Update Item";
          _context.UserActivity.Add(userActivity);
          _context.SaveChanges();

          return Json("success");
        }


        public void GetCodeFA()
        {
            var @params = new NameValueCollection
            {
                //Azure AD will return an authorization code. 
                //See the Redirect class to see how "code" is used to AcquireTokenByAuthorizationCode
                {"response_type", "code"},

                //Client ID is used by the application to identify themselves to the users that they are requesting permissions from. 
                //You get the client id when you register your Azure app.
                {"client_id", ClientID},

                //Resource uri to the Power BI resource to be authorized
                // https://analysis.windows.net/powerbi/api
                {"resource", "https://graph.microsoft.com/"},

                //After user authenticates, Azure AD will redirect back to the web app
                //{"redirect_uri", "https://localhost:44366/Home/FederalAdvocacy/"}
                {"redirect_uri", "https://phrma.azurewebsites.net/Home/FederalAdvocacy/"}
            };

          //Create sign-in query string
          var queryString = HttpUtility.ParseQueryString(string.Empty);
          queryString.Add(@params);

          //Redirect authority
          //Authority Uri is an Azure resource that takes a client id to get an Access token
          // AADAuthorityUri = https://login.windows.net/common/oauth2/authorize/
          string authorityUri = AADAuthorityUri;
          var authUri = String.Format("{0}?{1}", authorityUri, queryString);
          Response.Redirect(authUri);
        }

        public void GetCodeFAFASTLOAD()
        {
            var @params = new NameValueCollection
            {
                //Azure AD will return an authorization code. 
                //See the Redirect class to see how "code" is used to AcquireTokenByAuthorizationCode
                {"response_type", "code"},

                //Client ID is used by the application to identify themselves to the users that they are requesting permissions from. 
                //You get the client id when you register your Azure app.
                {"client_id", ClientID},

                //Resource uri to the Power BI resource to be authorized
                // https://analysis.windows.net/powerbi/api
                {"resource", "https://graph.microsoft.com/"},

                //After user authenticates, Azure AD will redirect back to the web app
                //{"redirect_uri", "https://localhost:44366/Home/FederalAdvocacyFastLoad/"}
                {"redirect_uri", "https://phrma.azurewebsites.net/Home/FederalAdvocacyFastLoad/"}
            };

            //Create sign-in query string
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            queryString.Add(@params);

            //Redirect authority
            //Authority Uri is an Azure resource that takes a client id to get an Access token
            // AADAuthorityUri = https://login.windows.net/common/oauth2/authorize/
            string authorityUri = AADAuthorityUri;
            var authUri = String.Format("{0}?{1}", authorityUri, queryString);
            Response.Redirect(authUri);
        }

        public async Task<IActionResult> UserDetail(string code)
        {
          if (code == null)
          {
            GetCode();
          }


          TokenCache TC = new TokenCache();
          AuthenticationContext AC = new AuthenticationContext(AADAuthorityUri, TC);
          ClientCredential cc = new ClientCredential(ClientID, ClientSecret);
          //AuthenticationResult AR = await AC.AcquireTokenByAuthorizationCodeAsync(imageCode, new Uri("https://localhost:44366/Home/ShowPhoto/"), cc);
          AuthenticationResult AR = await AC.AcquireTokenByAuthorizationCodeAsync(code, new Uri("https://phrma.azurewebsites.net/Home/UserDetail/"), cc);
          var authenticationResult = AR;
          var accessToken = authenticationResult.AccessToken;
          HttpClient client = new HttpClient();
          client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken.ToString());
          var result = await client.GetAsync("https://graph.microsoft.com/beta/me/photo/$value");

          var pictureMemoryStream = new MemoryStream();
          await result.Content.CopyToAsync(pictureMemoryStream);
          var pictureByteArray = pictureMemoryStream.ToArray();
          var pictureBase64 = Convert.ToBase64String(pictureByteArray);
          Models.DB.UserInfo info = new Models.DB.UserInfo();

          client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken.ToString());
          var result2 = await client.GetAsync("https://graph.microsoft.com/v1.0/me/?$select=department,givenName,userPrincipleName,officeLocation,mail,jobTitle,displayName");
          var response = result2.Content.ReadAsStringAsync();
          Newtonsoft.Json.Linq.JObject jsonObject = JObject.Parse(response.Result.ToString());
          var name = jsonObject["displayName"].ToString();


          info.FullUserName = User.Identity.Name.ToLower();

          if (pictureBase64.Substring(0, 12) == "ew0KICAiZXJy")
          {
            info.Image = "data:image/jpeg; base64,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";
          }
          else
          {
            info.Image = "data:image/jpeg;base64," + pictureBase64;
          }

          info.OfficeLocation = jsonObject["officeLocation"].ToString();
          info.Name = name;
          info.Title = jsonObject["jobTitle"].ToString();
          info.Department = jsonObject["department"].ToString();

          try
          {
            _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name.ToLower()).Select(a => a.Image).First().ToString();
            var item = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name.ToLower()).First();
            item.LastUpdate = DateTime.UtcNow.Subtract(TimeSpan.FromHours(4));
            _secondContext.UserInfo.Update(item);
            _secondContext.SaveChanges();




          }
          catch (Exception)
          {

            _secondContext.UserInfo.Add(info);
            _secondContext.SaveChanges();

          }

          UserActivity userActivity = new UserActivity();
          userActivity.FullUserName = User.Identity.Name.ToLower();
          userActivity.Page = "Dashboard";
          userActivity.Activity = "Register";
          _context.UserActivity.Add(userActivity);
          _context.SaveChanges();

          return RedirectToAction("Index");


        }



        [HttpPost]
        public JsonResult UpdateItem([FromBody] PostSoftCommitmentForecast postsf)
        {



          string note = postsf.Note;
          var items = postsf.Query.ToString().Split("|");
          var department = items[0];
          var project = items[1];
          var location = items[2];
          var mainaccount = items[3];
          var vendor = items[4];
          var referencecode = items[5];
          string forecastbudget = items[6];
          if (vendor == "Budget Entry")
          {
            try
            {
              var budgetItemNoteSync = _context.SoftCommitmentForecast.Where(a => a.EntryCategory == "0" && a.DepartmentId == department && a.ProjectId == project && a.LocationId == location && a.MainAccountId == mainaccount && a.VendorId == vendor && a.ReferenceCodeId == referencecode).FirstOrDefault();
              budgetItemNoteSync.Note = note;
              budgetItemNoteSync.NoteWriter = User.Identity.Name.ToLower().ToString();
              _context.SoftCommitmentForecast.Update(budgetItemNoteSync);
              _context.SaveChanges();
            }
            catch (Exception)
            {


            }

          }
          var updateditems = _context.SoftCommitmentForecast.Where(a => a.DepartmentId == department && a.ProjectId == project && a.LocationId == location && a.MainAccountId == mainaccount && a.VendorId == vendor && a.ReferenceCodeId == referencecode).ToList();
          foreach (var updateditem in updateditems)
          {
            if (updateditem.EntryCategory == "1")
            {
              updateditem.ForecastBudget = Convert.ToInt32(forecastbudget);
            }


            if (updateditem.Note != note)
            {
              updateditem.Note = note;
              updateditem.NoteWriter = User.Identity.Name.ToLower().ToString();
            }

            _context.SoftCommitmentForecast.Update(updateditem);
            _context.SaveChanges();
          }



          return Json("success");

        }

        [HttpPost]
        public JsonResult UpdateBudgetItem([FromBody] PostSoftCommitmentForecast postsf)
        {



          string note = postsf.Note;
          var items = postsf.Query.ToString().Split("|");
          var department = items[0];
          var project = items[1];
          var location = items[2];
          var mainaccount = items[3];
          var vendor = items[4];
          var referencecode = items[5];
          string nextyearbudget = items[6];

          var updateditems = _context.SoftCommitmentForecast.Where(a => a.DepartmentId == department && a.ProjectId == project && a.LocationId == location && a.MainAccountId == mainaccount && a.VendorId == vendor && a.ReferenceCodeId == referencecode).ToList();

          foreach (var updateditem in updateditems)
          {
            _context.SoftCommitmentForecast.Update(updateditem);
            _context.SaveChanges();
          }

          return Json("success");

        }


 
        [HttpGet]
        public IActionResult Forecast(string departmentid, string projectid, string locationid, string hasdata)
        {

          ViewBag.OfficeLocation = _context.UserInfo.Where(x => x.FullUserName == User.Identity.Name).Select(x => x.OfficeLocation).FirstOrDefault().ToString();
        //  ViewBag.WeatherUrl = _context.WeatherWidgets.Where(a => a.Location == _context.UserInfo.Where(x => x.FullUserName == User.Identity.Name.ToLower()).Select(x => x.OfficeLocation).Distinct().ToString()).Select(a => a.WeatherURL).ToString();
          ViewBag.Image = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Image).FirstOrDefault().ToString();
          ViewBag.Name = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Name).FirstOrDefault().ToString();
          ViewBag.Title = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Title).FirstOrDefault().ToString();
          ViewBag.Department = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Department).FirstOrDefault().ToString();
          ViewBag.DepartmentId = _context.Security.Where(a => a.Username == User.Identity.Name.ToLower()).Select(a => a.DepartmentId).FirstOrDefault().ToString();
          List<string> departmentSecuritySummary = new List<string>();

          bool IsRegionUser = _context.Security.Where(a => a.Username == User.Identity.Name.ToLower()).Select(a => a.Region).FirstOrDefault() != "";
          List<string> allowedRegions = new List<string>();
          if (IsRegionUser)
          {
            allowedRegions = _context.Security.Where(a => a.Username == User.Identity.Name.ToLower()).Select(a => a.Region).ToList();
          }
          else
          {
            allowedRegions = _context.Locations.Select(a => a.RegionName).ToList();
            allowedRegions.Add("None");
          }
          var allowedDepartments = new List<string>();
          int AllowedAccountCategories = 0;
          var security = _context.Security.Where(a => a.Username == User.Identity.Name.ToLower()).Select(a => a.DepartmentId).Distinct().ToList();

          var securityUsers = _context.Security.Where(a => a.Username == User.Identity.Name.ToLower()).Select(a => a.SecurityUsers).ToList();
          bool isAllCategories = securityUsers.Contains("All");

          if (isAllCategories)
          {
            AllowedAccountCategories = 100;
          }
          else
          {
            AllowedAccountCategories = 15;
          }


          if (security.Contains("All"))
          {

            ViewBag.Departments = _context.Departments.Distinct().ToList().OrderBy(a => a.DepartmentId);
            departmentSecuritySummary = _context.Departments.Distinct().Select(a => a.DepartmentId).ToList();
            allowedDepartments = _context.Departments.Distinct().ToList().Select(a => a.DepartmentId).ToList();

          }
          else
          {

            ViewBag.Departments = _context.Departments.Where(a => security.Contains(a.DepartmentId)).Distinct().OrderBy(a => a.DepartmentId).ToList();
            departmentSecuritySummary = security;
            allowedDepartments = _context.Departments.Where(a => security.Contains(a.DepartmentId)).Distinct().Select(a => a.DepartmentId).ToList();
          }

          var departmentSummaryData = _context.SoftCommitmentForecast.Where(a => departmentSecuritySummary.Contains(a.DepartmentId) && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).ToList();
          var summarizedDepartmentSummaryData = departmentSummaryData.GroupBy(a => new { a.DepartmentId, a.DepartmentName, a.ProjectId, a.ProjectName }).Select(a => new { WorkingBudget = a.Sum(x => x.WorkingBudget), Actuals = a.Sum(x => x.Actuals), Requisitions = a.Sum(x => x.Requisitions), POBalance = a.Sum(x => x.HardCommitments), TotalCommitted = a.Sum(x => x.TotalCommitted), ForecastBudget = a.Sum(x => x.ForecastBudget), NextYearBudget = a.Sum(x => x.NextYearBudget), Department = a.Key.DepartmentId + " - " + a.Key.DepartmentName, Project = a.Key.ProjectId + " - " + a.Key.ProjectName }).ToList();
          List<SoftCommitmentForecast> DeptSummaryDataList = new List<SoftCommitmentForecast>();

          var deptOnlySummaryTotals = departmentSummaryData.GroupBy(a => new { a.DepartmentId, a.DepartmentName }).Select(a => new { WorkingBudget = a.Sum(x => x.WorkingBudget), Actuals = a.Sum(x => x.Actuals), Requisitions = a.Sum(x => x.Requisitions), POBalance = a.Sum(x => x.HardCommitments), TotalCommitted = a.Sum(x => x.TotalCommitted), ForecastBudget = a.Sum(x => x.ForecastBudget), NextYearBudget = a.Sum(x => x.NextYearBudget), Department = a.Key.DepartmentId + " - " + a.Key.DepartmentName }).ToList();

          foreach (var item in deptOnlySummaryTotals)
          {
            SoftCommitmentForecast deptSumItem = new SoftCommitmentForecast();
            deptSumItem.DepartmentName = item.Department;
            deptSumItem.ProjectName = "00aa";
            deptSumItem.LocationName = "00aa";
            deptSumItem.WorkingBudget = item.WorkingBudget;
            deptSumItem.Actuals = item.Actuals;
            deptSumItem.Requisitions = item.Requisitions;
            deptSumItem.HardCommitments = item.POBalance;
            deptSumItem.TotalCommitted = item.TotalCommitted;
            deptSumItem.ForecastBudget = item.ForecastBudget;
            deptSumItem.NextYearBudget = item.NextYearBudget;
            deptSumItem.EntryCategory = "DT";
            deptSumItem.CollapseGroup = "#zzqq" + item.Department.Split(" - ")[0].ToString();
            if (item.WorkingBudget > item.ForecastBudget)
            {
              deptSumItem.ArrowIndicator = "down";

            }
            else if (item.WorkingBudget < item.ForecastBudget)
            {
              deptSumItem.ArrowIndicator = "up";
            }
            else
            {
              deptSumItem.ArrowIndicator = "equal";
            }


            DeptSummaryDataList.Add(deptSumItem);
          }


          foreach (var item in summarizedDepartmentSummaryData)
          {
            SoftCommitmentForecast deptSumItem = new SoftCommitmentForecast();
            deptSumItem.DepartmentName = item.Department;
            deptSumItem.ProjectName = item.Project;
            deptSumItem.LocationName = "00bb";
            deptSumItem.WorkingBudget = item.WorkingBudget;
            deptSumItem.Actuals = item.Actuals;
            deptSumItem.Requisitions = item.Requisitions;
            deptSumItem.HardCommitments = item.POBalance;
            deptSumItem.TotalCommitted = item.TotalCommitted;
            deptSumItem.ForecastBudget = item.ForecastBudget;
            deptSumItem.NextYearBudget = item.NextYearBudget;
            deptSumItem.EntryCategory = "PT";
            deptSumItem.CollapseGroup = "#zzqq" + item.Department.Split(" - ")[0].ToString();
            if (item.WorkingBudget > item.ForecastBudget)
            {
              deptSumItem.ArrowIndicator = "down";

            }
            else if (item.WorkingBudget < item.ForecastBudget)
            {
              deptSumItem.ArrowIndicator = "up";
            }
            else
            {
              deptSumItem.ArrowIndicator = "equal";
            }


            DeptSummaryDataList.Add(deptSumItem);
          }

          var summarizedDepartmentLocationProjectSummaryData = departmentSummaryData.GroupBy(a => new { a.DepartmentId, a.DepartmentName, a.ProjectId, a.ProjectName, a.LocationName }).Select(a => new { WorkingBudget = a.Sum(x => x.WorkingBudget), Actuals = a.Sum(x => x.Actuals), Requisitions = a.Sum(x => x.Requisitions), POBalance = a.Sum(x => x.HardCommitments), TotalCommitted = a.Sum(x => x.TotalCommitted), ForecastBudget = a.Sum(x => x.ForecastBudget), NextYearBudget = a.Sum(x => x.NextYearBudget), Department = a.Key.DepartmentId + " - " + a.Key.DepartmentName, Project = a.Key.ProjectId + " - " + a.Key.ProjectName, Location = a.Key.LocationName }).ToList();
          foreach (var item in summarizedDepartmentLocationProjectSummaryData)
          {
            SoftCommitmentForecast deptSumItem = new SoftCommitmentForecast();
            deptSumItem.DepartmentName = item.Department;
            deptSumItem.ProjectName = item.Project;
            deptSumItem.LocationName = item.Location;
            deptSumItem.WorkingBudget = item.WorkingBudget;
            deptSumItem.Actuals = item.Actuals;
            deptSumItem.Requisitions = item.Requisitions;
            deptSumItem.HardCommitments = item.POBalance;
            deptSumItem.TotalCommitted = item.TotalCommitted;
            deptSumItem.ForecastBudget = item.ForecastBudget;
            deptSumItem.NextYearBudget = item.NextYearBudget;
            deptSumItem.EntryCategory = "LT";
            deptSumItem.CollapseGroup = "#zzqq" + item.Department.Split(" - ")[0].ToString();
            if (item.WorkingBudget > item.ForecastBudget)
            {
              deptSumItem.ArrowIndicator = "down";

            }
            else if (item.WorkingBudget < item.ForecastBudget)
            {
              deptSumItem.ArrowIndicator = "up";
            }
            else
            {
              deptSumItem.ArrowIndicator = "equal";
            }


            DeptSummaryDataList.Add(deptSumItem);
          }
















          var grandTotalSummaryDepartment = departmentSummaryData.GroupBy(a => "Total").Select(a => new { WorkingBudget = a.Sum(x => x.WorkingBudget), Actuals = a.Sum(x => x.Actuals), Requisitions = a.Sum(x => x.Requisitions), POBalance = a.Sum(x => x.HardCommitments), TotalCommitted = a.Sum(x => x.TotalCommitted), ForecastBudget = a.Sum(x => x.ForecastBudget), NextYearBudget = a.Sum(x => x.NextYearBudget) }).ToList();
          foreach (var item in grandTotalSummaryDepartment)
          {
            SoftCommitmentForecast deptSumItem = new SoftCommitmentForecast();
            deptSumItem.DepartmentName = "00aa";
            deptSumItem.ProjectName = "00aa";
            deptSumItem.WorkingBudget = item.WorkingBudget;
            deptSumItem.Actuals = item.Actuals;
            deptSumItem.Requisitions = item.Requisitions;
            deptSumItem.HardCommitments = item.POBalance;
            deptSumItem.TotalCommitted = item.TotalCommitted;
            deptSumItem.ForecastBudget = item.ForecastBudget;
            deptSumItem.NextYearBudget = item.NextYearBudget;
            deptSumItem.EntryCategory = "GT";

            if (item.WorkingBudget > item.ForecastBudget)
            {
              deptSumItem.ArrowIndicator = "down";

            }
            else if (item.WorkingBudget < item.ForecastBudget)
            {
              deptSumItem.ArrowIndicator = "up";
            }
            else
            {
              deptSumItem.ArrowIndicator = "equal";
            }

            DeptSummaryDataList.Add(deptSumItem);
          }


          ViewBag.DepartmentSummaryData = DeptSummaryDataList.OrderBy(a => a.DepartmentName).ThenBy(a => a.ProjectName).ThenBy(a => a.LocationName).ToList();



          if (hasdata != "false")
          {
            List<SoftCommitmentForecast> AllData = new List<SoftCommitmentForecast>();

            List<SoftCommitmentForecast> listForecast = _context.SoftCommitmentForecast.Where(a => a.DepartmentId == departmentid && a.ProjectId == projectid && a.LocationId == locationid && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).ToList();


            SoftCommitmentForecast GrandTotal = new SoftCommitmentForecast();
            GrandTotal.WorkingBudget = listForecast.Sum(a => a.WorkingBudget);
            GrandTotal.Actuals = listForecast.Sum(a => a.Actuals);
            GrandTotal.Requisitions = listForecast.Sum(a => a.Requisitions);
            GrandTotal.HardCommitments = listForecast.Sum(a => a.HardCommitments);
            GrandTotal.ForecastBudget = listForecast.Sum(a => a.ForecastBudget);
            GrandTotal.TotalCommitted = listForecast.Sum(a => a.TotalCommitted);
            GrandTotal.NextYearBudget = listForecast.Sum(a => a.NextYearBudget);
            GrandTotal.DepartmentId = departmentid;
            GrandTotal.MainAccountId = "00 a";
            GrandTotal.MainAccountName = "";
            GrandTotal.LocationId = "";
            GrandTotal.ProjectId = "";
            GrandTotal.VendorId = "00 a";
            GrandTotal.VendorName = "00 a";
            GrandTotal.EntryCategory = "GT";
            GrandTotal.AccountCategory = "Grand Total";
            GrandTotal.AccountCategoryId = "00 a";
            GrandTotal.ReferenceCodeId = "";
            GrandTotal.ReferenceCodeName = "";
            if (GrandTotal.WorkingBudget > GrandTotal.ForecastBudget)
            {
              GrandTotal.ArrowIndicator = "down";

            }
            else if (GrandTotal.WorkingBudget < GrandTotal.ForecastBudget)
            {
              GrandTotal.ArrowIndicator = "up";
            }
            else
            {
              GrandTotal.ArrowIndicator = "equal";
            }






            List<SoftCommitmentForecast> AccountCategoryTotals = new List<SoftCommitmentForecast>();
            var tempList = listForecast.GroupBy(a => new { a.AccountCategoryId, a.AccountCategory }).Select(a => new { WorkingBudget = a.Sum(x => x.WorkingBudget), Actuals = a.Sum(x => x.Actuals), ForecastBudget = a.Sum(x => x.ForecastBudget), POBalance = a.Sum(x => x.HardCommitments), Requisitions = a.Sum(x => x.Requisitions), TotalCommitted = a.Sum(x => x.TotalCommitted), NextYearBudget = a.Sum(x => x.NextYearBudget), AccountCategory = a.Key.AccountCategory, AccountCategoryId = a.Key.AccountCategoryId }).ToList();
            foreach (var item in tempList)
            {
              SoftCommitmentForecast sf = new SoftCommitmentForecast();
              sf.WorkingBudget = item.WorkingBudget;
              sf.Actuals = item.Actuals;
              sf.ForecastBudget = item.ForecastBudget;
              sf.Requisitions = item.Requisitions;
              sf.HardCommitments = item.POBalance;
              sf.TotalCommitted = item.TotalCommitted;
              sf.NextYearBudget = item.NextYearBudget;
              sf.DepartmentId = "";
              sf.MainAccountId = "00 aa";
              sf.MainAccountName = "";
              sf.LocationId = "00 aa";
              sf.ProjectId = "00 aa";
              sf.VendorId = "00 aa";
              sf.VendorName = "";
              sf.EntryCategory = "ACT";
              sf.AccountCategory = item.AccountCategory;
              sf.AccountCategoryId = item.AccountCategoryId;
              sf.ReferenceCodeId = "00 aa";
              sf.ReferenceCodeName = "00 aa";
              if (item.WorkingBudget > item.ForecastBudget)
              {
                sf.ArrowIndicator = "down";

              }
              else if (item.WorkingBudget < item.ForecastBudget)
              {
                sf.ArrowIndicator = "up";
              }
              else
              {
                sf.ArrowIndicator = "equal";
              }
              AccountCategoryTotals.Add(sf);
            }



            List<SoftCommitmentForecast> MainAccountTotals = new List<SoftCommitmentForecast>();
            listForecast.Select(a => new { a.AccountCategoryId, a.MainAccountId }).Distinct().ToList();


            var tempMainAccountList = listForecast.GroupBy(a => new { a.AccountCategoryId, a.AccountCategory, a.MainAccountId, a.MainAccountName }).Select(a => new { WorkingBudget = a.Sum(x => x.WorkingBudget), Actuals = a.Sum(x => x.Actuals), ForecastBudget = a.Sum(x => x.ForecastBudget), POBalance = a.Sum(x => x.HardCommitments), Requisitions = a.Sum(x => x.Requisitions), NextYearBudget = a.Sum(x => x.NextYearBudget), TotalCommitted = a.Sum(x => x.TotalCommitted), AccountCategory = a.Key.AccountCategory, AccountCategoryId = a.Key.AccountCategoryId, MainAccountName = a.Key.MainAccountName, MainAccountId = a.Key.MainAccountId }).ToList();

            foreach (var item in tempMainAccountList)
            {
              SoftCommitmentForecast sf = new SoftCommitmentForecast();
              sf.WorkingBudget = item.WorkingBudget;
              sf.Actuals = item.Actuals;
              sf.ForecastBudget = item.ForecastBudget;
              sf.NextYearBudget = item.NextYearBudget;
              sf.Requisitions = item.Requisitions;
              sf.HardCommitments = item.POBalance;
              sf.TotalCommitted = item.TotalCommitted;
              sf.DepartmentId = "0 a";
              sf.MainAccountId = item.MainAccountId;
              sf.MainAccountName = item.MainAccountName;
              sf.LocationId = "0 a";
              sf.ProjectId = "0 a";
              sf.VendorId = "0 a";
              sf.VendorName = "0 a";
              sf.EntryCategory = "ST";
              sf.AccountCategory = item.AccountCategory;
              sf.AccountCategoryId = item.AccountCategoryId;
              sf.ReferenceCodeId = "0 a";
              sf.ReferenceCodeName = "0 a";
              if (item.WorkingBudget > item.ForecastBudget)
              {
                sf.ArrowIndicator = "down";

              }
              else if (item.WorkingBudget < item.ForecastBudget)
              {
                sf.ArrowIndicator = "up";
              }
              else
              {
                sf.ArrowIndicator = "equal";
              }
              MainAccountTotals.Add(sf);
            }



            var tempLineItemList = listForecast.GroupBy(a => new { a.AccountCategoryId, a.AccountCategory, a.MainAccountId, a.MainAccountName, a.ProjectId, a.ProjectName, a.DepartmentId, a.DepartmentName, a.ReferenceCodeId, a.ReferenceCodeName, a.LocationId, a.LocationName, a.VendorId, a.VendorName, a.Note }).Select(a => new { WorkingBudget = a.Sum(x => x.WorkingBudget), Actuals = a.Sum(x => x.Actuals), ForecastBudget = a.Sum(x => x.ForecastBudget), POBalance = a.Sum(x => x.HardCommitments), Requisitions = a.Sum(x => x.Requisitions), TotalCommitted = a.Sum(x => x.TotalCommitted), NextYearBudget = a.Sum(x => x.NextYearBudget), AccountCategory = a.Key.AccountCategory, AccountCategoryId = a.Key.AccountCategoryId, MainAccountName = a.Key.MainAccountName, MainAccountId = a.Key.MainAccountId, VendorName = a.Key.VendorName, VendorId = a.Key.VendorId, ReferenceCodeName = a.Key.ReferenceCodeName, ReferenceCodeId = a.Key.ReferenceCodeId, ProjectId = a.Key.ProjectId, ProjectName = a.Key.ProjectName, DepartmentName = a.Key.DepartmentName, DepartmentId = a.Key.DepartmentId, LocationId = a.Key.LocationId, LocationName = a.Key.LocationName, Note = a.Key.Note }).ToList();
            List<SoftCommitmentForecast> LineItemTotals = new List<SoftCommitmentForecast>();

            foreach (var item in tempLineItemList)
            {
              SoftCommitmentForecast sf = new SoftCommitmentForecast();
              sf.WorkingBudget = item.WorkingBudget;
              sf.Actuals = item.Actuals;
              sf.ForecastBudget = item.ForecastBudget;
              sf.NextYearBudget = item.NextYearBudget;
              sf.Requisitions = item.Requisitions;
              sf.HardCommitments = item.POBalance;
              sf.TotalCommitted = item.TotalCommitted;
              sf.DepartmentId = item.DepartmentId;
              sf.DepartmentName = item.DepartmentName;
              sf.MainAccountId = item.MainAccountId;
              sf.MainAccountName = item.MainAccountName;
              sf.LocationId = item.LocationId;
              sf.ProjectId = item.ProjectId;
              sf.VendorId = item.VendorId;
              sf.VendorName = item.VendorName;
              sf.EntryCategory = "LT";
              sf.AccountCategory = item.AccountCategory;
              sf.AccountCategoryId = item.AccountCategoryId;
              sf.ReferenceCodeId = item.ReferenceCodeId;
              sf.ReferenceCodeName = item.ReferenceCodeName;
              sf.Note = item.Note;
              if (item.WorkingBudget > item.ForecastBudget)
              {
                sf.ArrowIndicator = "down";

              }
              else if (item.WorkingBudget < item.ForecastBudget)
              {
                sf.ArrowIndicator = "up";
              }
              else
              {
                sf.ArrowIndicator = "equal";
              }
              LineItemTotals.Add(sf);
            }


            var idArrayMainAccount = new string[52] { "#a", "#b", "#c", "#d", "#e", "#f", "#g", "#h", "#i", "#j", "#k", "#l", "#m", "#n", "#o", "#p", "#q", "#r", "#s", "#t", "#u", "#v", "#w", "#x", "#y", "#z", "#a1", "#b1", "#c1", "#d1", "#e1", "#f1", "#g1", "#h1", "#i1", "#j1", "#k1", "#l1", "#m1", "#n1", "#o1", "#p1", "#q1", "#r1", "#s1", "#t1", "#u1", "#v1", "#w1", "#x1", "#y1", "#z1" };
            var i = 0;

            foreach (var sact in AccountCategoryTotals.OrderBy(a => a.AccountCategoryId))
            {
              //sact.CollapseGroup = idArrayAccountCategory[i];
              AllData.Add(sact);
              foreach (var mat in MainAccountTotals.Where(a => a.AccountCategoryId == sact.AccountCategoryId).OrderBy(a => a.MainAccountId))
              {
                mat.CollapseGroup = idArrayMainAccount[i];
                AllData.Add(mat);
                foreach (var lit in LineItemTotals.Where(a => a.MainAccountId == mat.MainAccountId).OrderBy(a => a.VendorName).ThenBy(a => a.ReferenceCodeName))
                {
                  lit.CollapseGroup = idArrayMainAccount[i];
                  AllData.Add(lit);
                }
                i++;
              }

            }



            AllData.Insert(0, GrandTotal);



            ViewBag.HasData = "true";
            var model = AllData;
            ViewBag.Vendors = _context.Vendors.Distinct().OrderBy(a => a.VendorName).ToList();
            ViewBag.MainAccounts = _context.MainAccounts.OrderBy(a => a.MainAccountId).Distinct().ToList();
            ViewBag.Department = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Department).FirstOrDefault().ToString();
            ViewBag.ReferenceCode = _context.ReferenceCode.OrderBy(a => a.ReferenceCodeName).ToList();

            var allowedProjects = _context.SoftCommitmentForecast.Where(a => allowedDepartments.Contains(a.DepartmentId) && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).Select(a => a.ProjectId).Distinct().ToList();

            ViewBag.Projects = _context.Projects.Where(a => allowedProjects.Contains(a.ProjectId)).OrderBy(a => a.ProjectId).ToList();
            ViewBag.DistinctDimensions = JsonConvert.SerializeObject(_context.SoftCommitmentForecast.Select(a => new { a.DepartmentId, a.ProjectId, a.LocationId }).Distinct().ToList()).ToString();
            ViewBag.Locations = _context.Locations.Where(a => allowedRegions.Contains(a.RegionName)).OrderBy(a => a.LocationName).ToList();



            return View(model);
          }
          else
          {



            ViewBag.Vendors = _context.Vendors.Distinct().OrderBy(a => a.VendorName).ToList();
            ViewBag.MainAccounts = _context.MainAccounts.Distinct().OrderBy(a => a.MainAccountId).ToList();
            ViewBag.Department = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Department).FirstOrDefault().ToString();
            ViewBag.ReferenceCode = _context.ReferenceCode.OrderBy(a => a.ReferenceCodeName).ToList();
            var allowedProjects = _context.SoftCommitmentForecast.Where(a => allowedDepartments.Contains(a.DepartmentId) && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).Select(a => a.ProjectId).Distinct().ToList();
            ViewBag.Projects = _context.Projects.Where(a => allowedProjects.Contains(a.ProjectId)).OrderBy(a => a.ProjectId).ToList();
            ViewBag.HasData = "false";
            ViewBag.DistinctDimensions = JsonConvert.SerializeObject(_context.SoftCommitmentForecast.Select(a => new { a.DepartmentId, a.ProjectId, a.LocationId }).Distinct().ToList()).ToString();
            ViewBag.Locations = _context.Locations.Where(a => allowedRegions.Contains(a.RegionName)).OrderBy(a => a.LocationName).ToList();
            var model = _context.SoftCommitmentForecast.ToList();
            return View(model);
          }

        }

        
        [HttpGet]
        public async Task<JsonResult> SenatorsGET()
        {
          return Json(await _context.Senators.ToListAsync());
           
        }


    }
}
