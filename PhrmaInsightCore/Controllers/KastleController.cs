using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Data;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PhrmaInsightCore.Models;
using PhrmaInsightCore.Models.DB;
using RestSharp;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http;
using OfficeOpenXml;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PhrmaInsightCore.Controllers
{

    [AllowAnonymous]
    public class KastleController : Controller
    {
        DatabaseContext _context;

        public KastleController(DatabaseContext context)
        {
            _context = context;

        }
        public IActionResult Admin()
        {

            return View();
        }

        public IActionResult MyRecurringMeetings()
        {

            return View();
        }



        public IActionResult Visitors()
        {
            ViewBag.Organizer = _context.AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == User.Identity.Name.ToLower()).Select(a => a.GIVEN_NAME + " " + a.SURNAME).FirstOrDefault().ToString();
            return View();
        }


        public IActionResult MyVisitorLog()
        {

            return View();
        }

        [HttpGet]
        public IActionResult EditMeeting(string groupid)
        {
            var data = _context.KastleVisitor.Where(a => a.GroupId == groupid).Select(a => new { a.GroupId, a.EndDate, a.Subject, a.NotifyEmail, a.NiceMeetingDate, a.NiceMeetingTime, a.MeetingDate, a.Host, a.MeetingOrganizer, a.MeetingOrganizerEmail, a.RecurringId, a.RecurringInterval }).Distinct().First();
            ViewBag.Subject = data.Subject;
            ViewBag.Notify = data.NotifyEmail;
            ViewBag.Host = data.Host;
            ViewBag.GroupId = data.GroupId;
            ViewBag.MeetingDate = data.MeetingDate;
            ViewBag.NiceMeetingDate = data.NiceMeetingDate;
            ViewBag.Time = data.NiceMeetingTime;
            ViewBag.Organizer = _context.AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == data.MeetingOrganizerEmail).Select(a => a.GIVEN_NAME + " " + a.SURNAME).FirstOrDefault().ToString();
            ViewBag.RecurringId = data.RecurringId;
            ViewBag.RecurringInterval = data.RecurringInterval;
            return View();
        }

        [HttpGet]
        public IActionResult EditRecurringMeeting(string groupid)
        {
            var data = _context.KastleVisitorRecurring.Where(a => a.GroupId == groupid).Select(a => new { a.GroupId, a.EndDate, a.Subject, a.NotifyEmail, a.NiceMeetingDate, a.NiceMeetingTime, a.MeetingDate, a.Host, a.MeetingOrganizer, a.MeetingOrganizerEmail, a.RecurringId, a.RecurringInterval }).Distinct().First();
            ViewBag.Subject = data.Subject;
            ViewBag.Notify = data.NotifyEmail;
            ViewBag.Host = data.Host;
            ViewBag.GroupId = data.GroupId;
            ViewBag.MeetingDate = data.MeetingDate;
            ViewBag.NiceMeetingDate = data.NiceMeetingDate;
            ViewBag.Time = data.NiceMeetingTime;
            ViewBag.Organizer = _context.AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == data.MeetingOrganizerEmail).Select(a => a.GIVEN_NAME + " " + a.SURNAME).FirstOrDefault().ToString();
            ViewBag.RecurringId = data.RecurringId;
            ViewBag.RecurringInterval = data.RecurringInterval;
            return View();
        }


        [AllowAnonymous]
        [HttpPost]
        public JsonResult UpdateMeeting([FromBody] KastleVisitor kastleVisitor)
        {

            var updateitems = _context.KastleVisitor.Where(a => a.GroupId == kastleVisitor.GroupId).ToList();
            List<KastleVisitor> updatevisitorlist = new List<KastleVisitor>();
            List<KastleVisitorModify> updatevisitorlistmodify = new List<KastleVisitorModify>();
            var hostDepartment = _context.AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == kastleVisitor.Host.ToString()).Select(a => a.DEPARTMENT).First().ToString();
            foreach (var item in updateitems)
            {
                item.Host = kastleVisitor.Host;
                item.HostDepartment = hostDepartment;
                item.NotifyEmail = kastleVisitor.NotifyEmail;
                item.MeetingDate = kastleVisitor.MeetingDate;
                item.NiceMeetingDate = kastleVisitor.NiceMeetingDate;
                item.NiceMeetingTime = kastleVisitor.NiceMeetingTime;
                item.Subject = kastleVisitor.Subject;
                item.EndDate = kastleVisitor.EndDate;
                item.SequenceId = (Convert.ToInt32(item.SequenceId) + 1).ToString();
                item.KastleConfirmed = "0";
                updatevisitorlist.Add(item);

                KastleVisitorModify vMod = new KastleVisitorModify();
                vMod.EndDate = kastleVisitor.EndDate;
                vMod.GroupId = item.GroupId;
                vMod.Host = kastleVisitor.Host;
                vMod.HostDepartment = hostDepartment;
                vMod.MeetingDate = kastleVisitor.MeetingDate;
                vMod.MeetingOrganizer = item.MeetingOrganizer;
                vMod.MeetingOrganizerEmail = item.MeetingOrganizerEmail;
                vMod.NiceMeetingDate = kastleVisitor.NiceMeetingDate;
                vMod.NiceMeetingTime = kastleVisitor.NiceMeetingTime;
                vMod.NotifyEmail = kastleVisitor.NotifyEmail;
                vMod.RecurringId = item.RecurringId;
                vMod.RecurringInterval = item.RecurringInterval;
                vMod.Subject = kastleVisitor.Subject;
                vMod.Timestamp = item.Timestamp;
                vMod.UId = item.UId;
                vMod.VisitorEmail = item.VisitorEmail;
                vMod.VisitorFirstName = item.VisitorFirstName;
                vMod.VisitorLastName = item.VisitorLastName;
                vMod.VisitorOrganization = item.VisitorOrganization;
                vMod.SequenceId = (Convert.ToInt32(item.SequenceId) + 1).ToString();
                updatevisitorlistmodify.Add(vMod);

            }
            _context.KastleVisitor.UpdateRange(updatevisitorlist);
            _context.KastleVisitorModify.AddRange(updatevisitorlistmodify);
            _context.SaveChanges();
            return Json("");
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult UpdateRecurringMeeting([FromBody] KastleVisitorRecurring kastleVisitor)
        {

            var updateitems = _context.KastleVisitorRecurring.Where(a => a.GroupId == kastleVisitor.GroupId).ToList();
            List<KastleVisitorRecurring> updatevisitorlist = new List<KastleVisitorRecurring>();
            var hostDepartment = _context.AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == kastleVisitor.Host.ToString()).Select(a => a.DEPARTMENT).First().ToString();
            foreach (var item in updateitems)
            {
                item.Host = kastleVisitor.Host;
                item.HostDepartment = hostDepartment;
                item.NotifyEmail = kastleVisitor.NotifyEmail;
                item.MeetingDate = kastleVisitor.MeetingDate;
                item.NiceMeetingDate = kastleVisitor.NiceMeetingDate;
                item.NiceMeetingTime = kastleVisitor.NiceMeetingTime;
                item.Subject = kastleVisitor.Subject;
                item.EndDate = kastleVisitor.EndDate;
                updatevisitorlist.Add(item);
            }
            _context.KastleVisitorRecurring.UpdateRange(updatevisitorlist);

            _context.SaveChanges();
            return Json("");
        }

        [HttpPost]
        public JsonResult NewVisitors([FromBody] KastleVisitor kastleVisitor)
        {
            Guid guid = Guid.NewGuid();
            kastleVisitor.UId = guid.ToString();
            var Department = _context.AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == User.Identity.Name.ToLower()).Select(a => a.DEPARTMENT).First().ToString();
            kastleVisitor.HostDepartment = Department;
            kastleVisitor.MeetingOrganizer = User.Identity.Name.ToLower();
            kastleVisitor.SequenceId = "0";
            kastleVisitor.KastleConfirmed = "0";
            _context.KastleVisitor.Add(kastleVisitor);
            _context.SaveChanges();
            return Json("");
        }

        [HttpPost]
        public JsonResult NewRecurringMeeting([FromBody] KastleVisitorRecurring kastleVisitor)
        {
            var Department = _context.AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == User.Identity.Name.ToLower()).Select(a => a.DEPARTMENT).First().ToString();
            kastleVisitor.HostDepartment = Department;
            kastleVisitor.MeetingOrganizer = User.Identity.Name.ToLower();
            _context.KastleVisitorRecurring.Add(kastleVisitor);
            _context.SaveChanges();
            return Json("");
        }
        
        [HttpGet]
        public JsonResult GetVisitors()
        {
            var Department = _context.AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == User.Identity.Name.ToLower()).Select(a => a.DEPARTMENT).First().ToString();
            var visitors = _context.KastleVisitor.Where(a => a.Host == User.Identity.Name.ToLower() || a.NotifyEmail == User.Identity.Name.ToLower() || a.MeetingOrganizer == User.Identity.Name.ToLower()).OrderBy(a => a.MeetingDate).ToList();
            var AzureADUsers = _context.AzureADUsers.ToList();
            foreach (var item in visitors)
            {
                item.MeetingOrganizer = AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == item.MeetingOrganizerEmail).Select(a => a.GIVEN_NAME + " " + a.SURNAME).First().ToString();
                item.Host = AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == item.Host).Select(a => a.GIVEN_NAME + " " + a.SURNAME).First().ToString();
            }

            return Json(visitors.OrderBy(a => a.MeetingDate).ThenBy(a => a.Host).ThenBy(a => a.Subject).ThenBy(a => a.VisitorOrganization).ThenBy(a => a.VisitorLastName).ThenBy(a => a.VisitorFirstName));
        }


        [HttpGet]
        public JsonResult GetRecurringMeetings()
        {
            var Department = _context.AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == User.Identity.Name.ToLower()).Select(a => a.DEPARTMENT).First().ToString();
            var visitors = _context.KastleVisitorRecurring.Where(a => a.Host == User.Identity.Name.ToLower() || a.NotifyEmail == User.Identity.Name.ToLower() || a.MeetingOrganizer == User.Identity.Name.ToLower()).OrderBy(a => a.MeetingDate).ToList();
            var AzureADUsers = _context.AzureADUsers.ToList();
            foreach (var item in visitors)
            {
                item.MeetingOrganizer = AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == item.MeetingOrganizerEmail).Select(a => a.GIVEN_NAME + " " + a.SURNAME).First().ToString();
                item.Host = AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == item.Host).Select(a => a.GIVEN_NAME + " " + a.SURNAME).First().ToString();
            }

            return Json(visitors.OrderBy(a => a.MeetingDate).ThenBy(a => a.Host).ThenBy(a => a.ImpexiumGroup).ToList());
        }

        [HttpGet]
        public JsonResult GetVisitorGroups()
        {
            var Department = _context.AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == User.Identity.Name.ToLower()).Select(a => a.DEPARTMENT).First().ToString();
            var visitorgroups = _context.KastleVisitor.Where(a => a.Host == User.Identity.Name.ToLower() || a.NotifyEmail == User.Identity.Name.ToLower() || a.MeetingOrganizer == User.Identity.Name.ToLower()).Select(a => new { a.GroupId, a.NiceMeetingDate, a.NiceMeetingTime, a.MeetingDate, a.Subject, a.Host, a.NotifyEmail }).Distinct().OrderBy(a => a.MeetingDate).ToList();
            return Json(visitorgroups);
        }

        [HttpGet]
        public JsonResult GetVisitorsDepartment()
        {
            var Department = _context.AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == User.Identity.Name.ToLower()).Select(a => a.DEPARTMENT).First().ToString();
            var visitors = _context.KastleVisitor.Where(a => a.HostDepartment == Department.ToString()).ToList();
            return Json(visitors);
        }

        [HttpGet]
        public JsonResult GetVisitorGroupsDepartment()
        {
            var Department = _context.AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == User.Identity.Name.ToLower()).Select(a => a.DEPARTMENT).First().ToString();
            var visitorgroups = _context.KastleVisitor.Where(a => a.HostDepartment == Department).Select(a => new { a.GroupId, a.NiceMeetingDate, a.NiceMeetingTime, a.MeetingDate, a.Subject, a.Host, a.NotifyEmail }).Distinct().OrderBy(a => a.MeetingDate).ToList();
            return Json(visitorgroups);
        }

        [HttpGet]
        public JsonResult GetVisitorsAdmin()
        {

            var visitors = _context.KastleVisitor.OrderBy(a => a.MeetingDate).ToList();
            var AzureADUsers = _context.AzureADUsers.ToList();
            foreach (var item in visitors)
            {
                item.MeetingOrganizer = AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == item.MeetingOrganizerEmail).Select(a => a.GIVEN_NAME + " " + a.SURNAME).First().ToString();
                item.Host = AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == item.Host).Select(a => a.GIVEN_NAME + " " + a.SURNAME).First().ToString();
            }
            return Json(visitors.OrderBy(a => a.MeetingDate).ThenBy(a => a.Host).ThenBy(a => a.Subject).ThenBy(a => a.VisitorOrganization).ThenBy(a => a.VisitorLastName).ThenBy(a => a.VisitorFirstName));
        }

        [HttpGet]
        public JsonResult GetVisitorGroupsAdmin()
        {
            var visitorgroups = _context.KastleVisitor.Select(a => new { a.GroupId, a.NiceMeetingDate, a.NiceMeetingTime, a.MeetingDate, a.Subject, a.Host, a.NotifyEmail }).Distinct().OrderBy(a => a.MeetingDate).ToList();
            return Json(visitorgroups);
        }


        [HttpGet]
        public JsonResult GetCommittees()
        {
            var committees = _context.CRMSource.OrderBy(a => a.Title).ToList();
            return Json(committees);
        }

        [HttpGet]
        public JsonResult GetUsers()
        {
            var users = _context.AzureADUsers.OrderBy(a => a.SURNAME).ToList();
            return Json(users);
        }

        [Produces("application/json")]
        [HttpGet]
        public String GetPhrmaUsers() 
        {
            var users = _context.AzureADUsers.OrderBy(a => a.SURNAME).ToList();
            string jsonString = "{";
  

            for (int k = 0; k < users.Count; k++)
            {
                jsonString += "{" +
                    "\"firstname\":\"" + users[k].GIVEN_NAME + "\"," +
                    "\"lastname\":\"" + users[k].SURNAME + "\"," +
                    "\"email\":\"" + users[k].USER_PRINCIPAL_NAME + "\"" +
                "}";

                jsonString += k < users.Count() - 1 ? "," : "";
            }

            jsonString += "}";

           
            return jsonString;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<String> GetCommitteeMembers(string committee)
        {
            //const string accessEndPoint = "http://public.impexium.com/Api/v1/WebApiUrl";
            //const string appName = "PhrmaExtranetUserManagerLive";
            //const string appKey = "Z2J201j5W7T6w60y";
            //const string appId = "PhrmaExtranetUserManagerLive";
            //const string appPassword = "Z2J201j5W7T6w60y";
            //const string userName = "EUM_Integration@extranetusermanager.com";
            //const string pwd = "csja58zv69w3";

            //const string accessEndPoint = "http://public.impexium.com/Api/v1/WebApiUrl";
            //const string appName = "PhrmaLIVE";
            //const string appKey = "f3q5tCu5243bc75s";
            //const string appId = "PhrmaLIVE";
            //const string appPassword = "f3q5tCu5243bc75s";
            //const string userName = "PhRMA_Integration@phrma.org";
            //const string pwd = "UPRFPQVkjZbHXt92";

            //values from api
            //string apiEndPoint = "https://impexium.phrma.org/api/v1/signup/authenticate";
            //string baseUri = "https://impexium.phrma.org:443/api/v1/";
            //string accessToken = "";
            //string appToken = "";
            //string userToken = "";
            //string userId = "";

            ////Step 1 : Get ApiEndPoint and AccessToken
            ////POST api/v1/WebApiUrl
            //var client = new RestClient(accessEndPoint);
            //var request = new RestRequest(Method.POST);
            //request.AddHeader("content-type", "application/json;charset=utf-8");
            //request.AddParameter("application/json;charset=utf-8", "{\"AppName\":\"" + appName + "\",\"AppKey\":\"" + appKey + "\"}", ParameterType.RequestBody);
            //IRestResponse response = client.Execute(request);
            //dynamic result = JsonConvert.DeserializeObject(response.Content);
            //apiEndPoint = result.uri;
            //accessToken = result.accessToken;

            ////Step 2: Get AppToken or UserToken or Both
            ////POST api/v1/Signup/Authenticate
            //client = new RestClient(apiEndPoint);
            //request = new RestRequest(Method.POST);
            //request.AddHeader("accesstoken", accessToken);
            //request.AddHeader("content-type", "application/json");
            //request.AddParameter("application/json", "{\"AppId\":\"" + appId + "\",\"AppPassword\":\"" + appPassword + "\",\"AppUserEmail\":\"" + userName + "\",\"AppUserPassword\":\"" + pwd + "\"}", ParameterType.RequestBody);
            //response = client.Execute(request);
            //result = JsonConvert.DeserializeObject(response.Content);
            //appToken = result.appToken;
            //baseUri = result.uri;
            //userToken = result.userToken;
            //userId = result.userId;
            ////https://localhost:44333/Kastle/GetCommitteeMembers?committee=EUM-JPN-RAREDISEASES
            ////IMPORTANT: Save the appToken and UserToken, and add this to the header for all other calls
            ////Example 1 : Get User's information
            ////GET api/v1/Individuals/Profile/{Id}/1
            //string query = "https://impexium.phrma.org:443/api/v1/committees/" + committee + "/Members";

            //client = new RestClient(query);
            //request = new RestRequest(Method.GET);
            //request.AddHeader("usertoken", userToken);
            //request.AddHeader("apptoken", appToken);
            //request.AddHeader("content-type", "application/json");
            //response = client.Execute(request);

            //var indResult = ParseJsonToDictionary(response.Content);
            //var dataList = GetList(indResult["dataList"]);
            //List<JObject> objectList = new List<JObject>();
            //string json = "[";
            //for (int i = 0; i < dataList.Count(); i++)
            //{
            //    var members = GetDictionary(dataList[i]);
            //    var firstname = members["firstName"].ToString();
            //    var lastname = members["lastName"].ToString();
            //    var email = "";
            //    var organization = "";

            //    try
            //    {
            //        organization = GetDictionary(members["primaryOrganization"])["name"].ToString();
            //    }
            //    catch (Exception)
            //    {

            //        organization = "";
            //    }
            //    try
            //    {
            //        email = GetDictionary(GetList(members["emails"])[0])["address"].ToString();
            //    }
            //    catch (Exception)
            //    {
            //        email = "no-email@phrma.org";

            //    }

            //    json += "{" +
            //        "\"firstname\":\"" +  firstname + "\"," +
            //        "\"lastname\":\"" + lastname + "\"," +
            //        "\"email\":\"" + email + "\"," +
            //         "\"organization\":\"" + organization + "\"" +
            //    "}";

            //    if (i == dataList.Count() - 1)
            //    {
            //        json += "]";
            //    }
            //    else {
            //        json += ",";
            //    }



            //}




            List<JObject> objectList = new List<JObject>();
            HttpClient client = new HttpClient();
            var url = "https://impexiumconnector.phmspo-prod.p.azurewebsites.net/committee/v1/" + committee + "/Members";
            var response = await client.GetAsync(url);
            var result = response.Content.ReadAsStringAsync();
            Newtonsoft.Json.Linq.JObject json = JObject.Parse(result.Result.ToString());



            var items = json["dataList"];
            //var items = json["dataList"]["result"];
            string jsonString = "[";
            for (var q = 0; q < items.Count(); q++)
            {
                JObject newJson = new JObject();

                var firstname = items[q]["firstName"].ToString();
                var lastname = items[q]["lastName"].ToString();
                var email = "";
                var organization = "";
                try
                {
                    email = items[q]["emails"][0]["address"].ToString().ToLower();

                }
                catch (Exception)
                {
                    email = "no-email@phrma.org";
                }
                try
                {
                    organization = items[q]["primaryOrganization"]["name"].ToString();
                }
                catch (Exception)
                {
                    organization = "";
                }

                jsonString += "{" +
                    "\"firstname\":\"" + firstname + "\"," +
                    "\"lastname\":\"" + lastname + "\"," +
                    "\"email\":\"" + email + "\"," +
                        "\"organization\":\"" + organization + "\"" +
                "}";



                if (q == items.Count() - 1)
                {
                    jsonString += "]";
                }
                else
                {
                    jsonString += ",";
                }
            }

            return jsonString;


        }



        private static Dictionary<string, object> ParseJsonToDictionary(string input)
        {
            // This utility method converts a JSON string into a dictionary of names and values for easy access
            if (input != null)
            {
                var initial = JsonConvert.DeserializeObject<Dictionary<string, object>>(input);
                return initial.ToDictionary(d => d.Key, d => d.Value is JArray ? ParseJsonToList(d.Value.ToString())
                                                                : (d.Value is JObject ? ParseJsonToDictionary(d.Value.ToString()) : d.Value));
            }
            return null;
        }

        private static List<object> ParseJsonToList(string input)
        {
            // This utility method converts a JSON string into a dictionary of names and values for easy access
            if (input != null)
            {
                var initial = JsonConvert.DeserializeObject<List<object>>(input);
                return initial.Select(d => d is JArray ? ParseJsonToList(d.ToString())
                                    : (d is JObject ? ParseJsonToDictionary(d.ToString()) : d)).ToList();
            }
            return null;
        }

        private static Dictionary<string, object> GetDictionary(object input)
        {
            return input as Dictionary<string, object>;
        }
        private static List<object> GetList(object input)
        {
            return input as List<object>;
        }

        [HttpGet]
        public JsonResult DeleteMeeting(string groupid)
        {
            _context.KastleVisitor.RemoveRange(_context.KastleVisitor.Where(a => a.GroupId == groupid).ToList());
            var userIds = _context.KastleVisitor.Where(a => a.GroupId == groupid).ToList();
            List<KastleVisitorDelete> visitorDelete = new List<KastleVisitorDelete>();

            foreach (var del in userIds)
            {
                KastleVisitorDelete vDel = new KastleVisitorDelete();
                vDel.EndDate = del.EndDate;
                vDel.GroupId = del.GroupId;
                vDel.Host = del.Host;
                vDel.HostDepartment = del.HostDepartment;
                vDel.MeetingDate = del.MeetingDate;
                vDel.MeetingOrganizer = del.MeetingOrganizer;
                vDel.NiceMeetingDate = del.NiceMeetingDate;
                vDel.NiceMeetingTime = del.NiceMeetingTime;
                vDel.NotifyEmail = del.NotifyEmail;
                vDel.RecurringId = del.RecurringId;
                vDel.RecurringInterval = del.RecurringInterval;
                vDel.Subject = del.Subject;
                vDel.Timestamp = del.Timestamp;
                vDel.UId = del.UId;
                vDel.VisitorEmail = del.VisitorEmail;
                vDel.VisitorFirstName = del.VisitorFirstName;
                vDel.VisitorLastName = del.VisitorLastName;
                vDel.VisitorOrganization = del.VisitorOrganization;
                vDel.SequenceId = del.SequenceId;

                visitorDelete.Add(vDel);
            }
            _context.KastleVisitorDelete.AddRange(visitorDelete);
            _context.SaveChanges();
            return Json("OK");
        }



        public JsonResult DeleteVisitor(string uid)
        {
            KastleVisitorDelete vDel = new KastleVisitorDelete();
            var del = _context.KastleVisitor.Where(a => a.UId == uid).First();
            vDel.EndDate = del.EndDate;
            vDel.GroupId = del.GroupId;
            vDel.Host = del.Host;
            vDel.HostDepartment = del.HostDepartment;
            vDel.MeetingDate = del.MeetingDate;
            vDel.MeetingOrganizer = del.MeetingOrganizer;
            vDel.NiceMeetingDate = del.NiceMeetingDate;
            vDel.NiceMeetingTime = del.NiceMeetingTime;
            vDel.NotifyEmail = del.NotifyEmail;
            vDel.RecurringId = del.RecurringId;
            vDel.RecurringInterval = del.RecurringInterval;
            vDel.Subject = del.Subject;
            vDel.Timestamp = del.Timestamp;
            vDel.UId = del.UId;
            vDel.VisitorEmail = del.VisitorEmail;
            vDel.VisitorFirstName = del.VisitorFirstName;
            vDel.VisitorLastName = del.VisitorLastName;
            vDel.VisitorOrganization = del.VisitorOrganization;
            vDel.SequenceId = del.SequenceId;
            _context.KastleVisitorDelete.Add(vDel);
            _context.SaveChanges();
            _context.KastleVisitor.Remove(_context.KastleVisitor.Where(a => a.UId == uid).First());
            _context.SaveChanges();
            return Json("OK");
        }

        public JsonResult DeleteVisitorRecurringMeeting(string id)
        {
            KastleVisitorRecurring vDel = new KastleVisitorRecurring();
            var del = _context.KastleVisitorRecurring.Where(a => a.Id == Convert.ToInt64(id)).First();
            _context.KastleVisitorRecurring.Remove(del);
            _context.SaveChanges();
            return Json("OK");
        }
        [HttpGet]
        public JsonResult DeleteRecurringMeeting(string groupid)
        {
            KastleVisitorRecurring vDel = new KastleVisitorRecurring();
            var del = _context.KastleVisitorRecurring.Where(a => a.GroupId == groupid).ToList();
            _context.KastleVisitorRecurring.RemoveRange(del);
            _context.SaveChanges();
            return Json("OK");
        }

        public class Visitor {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Organization { get; set; }
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<JsonResult> UploadFile([FromForm] IFormFile file)
        {
            List<Visitor> visitors = new List<Visitor>();
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);

                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;

                    for (int row = 2; row <= rowCount; row++)
                    {
                        var fname = worksheet.Cells[row, 1].Value.ToString().Trim();
                        var lname = worksheet.Cells[row, 2].Value.ToString().Trim();
                        var email = "";
                        var org = "";
                        try
                        {
                            org = worksheet.Cells[row, 4].Value.ToString().Trim() == null ? "" : worksheet.Cells[row, 4].Value.ToString().Trim();
                        }
                        catch (Exception)
                        { 

                        }
                        try
                        {
                            email = worksheet.Cells[row, 3].Value.ToString().Trim() == null ? "" : worksheet.Cells[row, 3].Value.ToString().Trim();
                        }
                        catch (Exception)
                        {

                        }
                     
                        if (fname != "" && lname != "" && fname != null && lname != null) {
                            visitors.Add(new Visitor
                            {
                                FirstName = worksheet.Cells[row, 1].Value.ToString().Trim(),
                                LastName = worksheet.Cells[row, 2].Value.ToString().Trim(),
                                Email = email,
                                Organization = org
                            });

                        }

                    }
                }
            }
            var returnJSON = visitors.ToList();
            return Json(returnJSON);
        }
    }
}