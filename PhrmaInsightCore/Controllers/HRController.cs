using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
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
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Z.Expressions;
using System.Drawing;
using OfficeOpenXml.Table.PivotTable;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace PhrmaInsightCore.Controllers
{

    [Authorize]
    public class HRController : Controller
    {
        DatabaseContext _context;

        public HRController(DatabaseContext context)
        {
            _context = context;

        }
        [AllowAnonymous]
        [HttpGet]
        public JsonResult Photos() {
            var json = _context.Employees.ToList();
            return Json(json);
        }
        [AllowAnonymous]
        [HttpGet]
        public IActionResult Organization(string department)
        {
            if (department != null && department != "")
            {
                ViewBag.Department = department;
            }
            else
            {
                ViewBag.Department = "";
            }
           
            return View();
        }

        public class Org
        {
            public string id { get; set; }
            public string title { get; set; }
            public string text { get; set; }
            public string parent { get; set; }
            public string dir { get; set; }
            public bool open { get; set; }
            public string img { get; set; }
            public int width { get; set; }
            public string department { get; set; }

        }


        [AllowAnonymous]
        [HttpGet]
        public async Task<string> FindDepartmentHead(string department) 
        {
            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://prod-54.eastus.logic.azure.com:443/workflows/6932ffb74b784735a6f3114db26ebba0/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=F6atxo7wclHe5yqPR-zo4tnZGZN--KvMofRxvREUwaU");
            var result = response.Content.ReadAsStringAsync();
            Newtonsoft.Json.Linq.JObject data = JObject.Parse(result.Result.ToString());
            var emps = data["value"];
            List<string> empsList = new List<string>();
            for (int i = 0; i < emps.Count(); i++)
            {
                empsList.Add(emps[i]["userPrincipalName"].ToString().ToLower());
            }

            var employees = _context.AzureADUsers.Where(a => a.DEPARTMENT == department && empsList.Contains(a.USER_PRINCIPAL_NAME) && a.AZURE_AD_MANAGER != null).Select(a => new { a.USER_PRINCIPAL_NAME, a.AZURE_AD_MANAGER}).ToList();
            var names = _context.AzureADUsers.Where(a => a.DEPARTMENT == department && empsList.Contains(a.USER_PRINCIPAL_NAME) && a.AZURE_AD_MANAGER != null).Select(a => a.USER_PRINCIPAL_NAME).ToList();
            foreach (var emp in employees)
            {
                if (names.Contains(emp.AZURE_AD_MANAGER) == false)
                {
                    return emp.USER_PRINCIPAL_NAME;
                }
            }
            return "";
        }



        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> GetOrgStructure(string department) 
        {
            string dept = department != null && department != "" ? department.Replace("amp","&") : "";

            HttpClient client = new HttpClient();
            var response = await client.GetAsync("https://prod-54.eastus.logic.azure.com:443/workflows/6932ffb74b784735a6f3114db26ebba0/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=F6atxo7wclHe5yqPR-zo4tnZGZN--KvMofRxvREUwaU");
            var result = response.Content.ReadAsStringAsync();
            Newtonsoft.Json.Linq.JObject data = JObject.Parse(result.Result.ToString());
            var emps = data["value"];
            List<string> empsList = new List<string>();
            for (int i = 0; i < emps.Count(); i++)
            {
                empsList.Add(emps[i]["userPrincipalName"].ToString().ToLower());
            }
            var deptHead = dept != "" ? await FindDepartmentHead(dept) : "subl@phrma.org";
            var employees = dept == "" ? _context.AzureADUsers.Where(a => a.AZURE_AD_MANAGER != null && empsList.Contains(a.USER_PRINCIPAL_NAME)).ToList() : _context.AzureADUsers.Where(a => a.DEPARTMENT == dept && a.USER_PRINCIPAL_NAME != deptHead && a.AZURE_AD_MANAGER != null && empsList.Contains(a.USER_PRINCIPAL_NAME)).ToList();
            //var top = dept == "" ? _context.AzureADUsers.Where(a => a.TITLE == "President & CEO").FirstOrDefault() : _context.AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == deptHead).FirstOrDefault();
            var top = _context.AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == "subl@phrma.org").FirstOrDefault();


            List<Org> orgList = new List<Org>();
            Org orgTop = new Org();
            orgTop.id = top.USER_PRINCIPAL_NAME;
            orgTop.title = top.GIVEN_NAME + " " + top.SURNAME;
            orgTop.text = top.TITLE;
            orgTop.width = 210;
            orgTop.open = true;
            orgTop.dir = "vertical";
            orgTop.parent = "";
            try
            {
                orgTop.img = _context.UserInfo.Where(a => a.FullUserName == top.USER_PRINCIPAL_NAME).Select(a => a.Image).FirstOrDefault().ToString();
            }
            catch (Exception)
            {
                orgTop.img = "../../img/nophoto.png";
               
            }
            orgList.Add(orgTop);
            

            foreach (var item in employees)
            {
                Org org = new Org();
                org.id = item.USER_PRINCIPAL_NAME;
                org.title = item.GIVEN_NAME + " " + item.SURNAME;
                org.text = item.TITLE;
                org.parent = item.AZURE_AD_MANAGER;
                org.dir = "vertical";
                org.open = true;
                org.width = 210;
                try
                {
                    org.img = _context.UserInfo.Where(a => a.FullUserName == item.USER_PRINCIPAL_NAME).Select(a => a.Image).FirstOrDefault().ToString();
                }
                catch (Exception)
                {
                    org.img = "../../img/nophoto.png";

                }
                orgList.Add(org);
            }

            return Json(orgList);
        }




        // GET PHRMA EMPLOYEES //

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> Get_Org() 
        {
           
         
            List<OrgChartUsers> employees = new List<OrgChartUsers>();

            employees = await _context.OrgChartUsers.ToListAsync();
            return Json(employees);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<JsonResult> Get_Employees(string department)
        {
            string dept = department != null && department != "" ? department.Replace("amp", "&") : "";

            List<Employees> employees = new List<Employees>();

            employees = dept != "All"
                ? await _context.Employees.Where(a => a.ManagerEmail != "" && a.Department == dept).ToListAsync()
                : await _context.Employees.Where(a => a.ManagerEmail != "" || a.Title == "President & CEO").ToListAsync();
            return Json(employees);
        }


    }
}