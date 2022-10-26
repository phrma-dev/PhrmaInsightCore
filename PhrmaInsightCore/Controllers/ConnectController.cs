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
using UserInfo = PhrmaInsightCore.Models.DB.UserInfo;

namespace PhrmaInsightCore.Controllers
{
	[Authorize]
	public class ConnectController : Controller
	{
		private readonly ILogger<FinanceController> _logger;
		DatabaseContext _context;


		public ConnectController(ILogger<FinanceController> logger, DatabaseContext context, DatabaseContext secondContext)
		{
			_logger = logger;
			_context = context;
		}
		public IActionResult Analytics()
		{
			return View();
		}

		[HttpGet]
		public JsonResult Tracking()
		{

			UserActivity userActivity = new UserActivity();
			userActivity.FullUserName = User.Identity.Name.ToLower();
			userActivity.Page = "Connect Analytics";
			userActivity.Activity = "Viewed Page";
			_context.UserActivity.Add(userActivity);
			_context.SaveChanges();

			return Json("success");
		}


		[HttpGet]
		public JsonResult GetSiteCategoryStatistics(string begindate, string enddate, string company, string phrma, string employee, string section, string subsection)
		{
			var filtered_list = SetFilter(begindate, enddate, phrma, company, employee, section, subsection);
			var returnJSON = filtered_list.Select(a => new { a.Section}).GroupBy(a => new { a.Section }).Select(a => new { SectionCount = a.Count(), a.Key.Section }).OrderByDescending(a => a.SectionCount).ToList();

			return Json(returnJSON);
		}

		[HttpGet]
		public JsonResult GetSubSectionStatistics(string begindate, string enddate, string company, string phrma, string employee, string section, string subsection)
		{
			var filtered_list = SetFilter(begindate, enddate, phrma, company, employee, section, subsection);
			var returnJSON = filtered_list.Select(a => new { a.SubSection }).GroupBy(a => new { a.SubSection }).Select(a => new { SubSectionCount = a.Count(), a.Key.SubSection}).OrderByDescending(a => a.SubSectionCount).ToList();

			return Json(returnJSON);
		}

		[HttpGet]
		public JsonResult GetTopCompaniesByViews(string begindate, string enddate, string company, string phrma, string employee, string section, string subsection)
		{
			var filtered_list = SetFilter(begindate, enddate, phrma, company, employee, section, subsection);
			var returnJSON = filtered_list.GroupBy(a => new { a.Company }).Select(a => new { count = a.Count(), company = a.Key.Company }).OrderByDescending(a => a.count).ToList();
			return Json(returnJSON);

		}

		[HttpGet]
		public JsonResult GetTopDownloads (string begindate, string enddate, string company, string phrma, string employee)
		{
			DateTime beginDate = new DateTime(Convert.ToInt32(begindate.Substring(6, 4)), Convert.ToInt32(begindate.Substring(0, 2)), Convert.ToInt32(begindate.Substring(3, 2)), 0, 0, 0);
			DateTime endDate = new DateTime(Convert.ToInt32(enddate.Substring(6, 4)), Convert.ToInt32(enddate.Substring(0, 2)), Convert.ToInt32(enddate.Substring(3, 2)), 23, 59, 59);
			if (employee == "" || employee == null)
			{
				if (phrma == "Include")
				{
					var returnJSON = company == null || company == "" ? _context.SharePointUserActivity.Where(a => (a.URL.Contains(".pdf") || a.URL.Contains(".pptx")) && a.Site == "Connect" && (a.IsExternal == "true" || a.Company == "PhRMA") && a.Section != "Not Assigned" && a.Section != null && a.ActivityTime >= beginDate && a.ActivityTime <= endDate).GroupBy(a => new { a.URL }).Select(a => new { count = a.Count(), url = a.Key.URL.Replace("%20", " ") }).OrderByDescending(a => a.count).ToList()
					: _context.SharePointUserActivity.Where(a =>  a.Site == "Connect" && (a.URL.Contains(".pdf") || a.URL.Contains(".pptx")) && (a.IsExternal == "true" || a.Company == "PhRMA") && a.Company == company && a.Section != "Not Assigned" && a.Section != null && a.ActivityTime >= beginDate && a.ActivityTime <= endDate).GroupBy(a => new { a.URL }).Select(a => new { count = a.Count(), url = a.Key.URL.Replace("%20", " ") }).OrderByDescending(a => a.count).ToList();
					return Json(returnJSON);
				}
				else
				{
					var returnJSON = company == null || company == "" ? _context.SharePointUserActivity.Where(a =>  (a.URL.Contains(".pdf") || a.URL.Contains(".pptx")) && a.Site == "Connect" && a.IsExternal == "true" && a.Section != "Not Assigned" && a.Section != null && a.ActivityTime >= beginDate && a.ActivityTime <= endDate).GroupBy(a => new { a.URL }).Select(a => new { count = a.Count(), url = a.Key.URL.Replace("%20", " ") }).OrderByDescending(a => a.count).ToList()
					: _context.SharePointUserActivity.Where(a => (a.URL.Contains(".pdf") || a.URL.Contains(".pptx")) && a.Site == "Connect" && a.IsExternal == "true" && a.Company == company && a.Section != "Not Assigned" && a.Section != null && a.ActivityTime >= beginDate && a.ActivityTime <= endDate).GroupBy(a => new { a.URL }).Select(a => new { count = a.Count(), url = a.Key.URL.Replace("%20", " ") }).OrderByDescending(a => a.count).ToList();
					return Json(returnJSON);
				}
			}
			else
			{
				var emp = employee.Contains("@phrma.org") ? employee : employee + "#ext#@phrma.onmicrosoft.com";
				var returnJSON = _context.SharePointUserActivity.Where(a => (a.URL.Contains(".pdf") || a.URL.Contains(".pptx")) && a.Site == "Connect" && a.Email.ToLower() == emp && a.Section != "Not Assigned" && a.Section != null && a.ActivityTime >= beginDate && a.ActivityTime <= endDate).GroupBy(a => new { a.URL }).Select(a => new { count = a.Count(), url = a.Key.URL.Replace("%20"," ") }).OrderByDescending(a => a.count).ToList();
				return Json(returnJSON);
			}

		}

		[HttpGet]
		public JsonResult GetTopCompaniesByDistinctUsers(string begindate, string enddate, string company, string phrma, string employee, string section, string subsection)
		{
			var filtered_list = SetFilter(begindate, enddate, phrma, company, employee, section, subsection);
			var returnJSON = filtered_list.Select(a => new { a.Company, a.Email }).Distinct().GroupBy(a => new { a.Company }).Select(a => new { count = a.Count(), company = a.Key.Company }).ToList();
			return Json(returnJSON);

		}

		public class ImpexiumMemberUser {
			public string company { get; set; }
			public string name { get; set; }
			public int count { get; set; }
			public string email { get; set; }
		}

		[HttpGet]
		public JsonResult GetSpecificEmployees(string begindate, string enddate, string company, string phrma, string employee, string section, string subsection)
		{

			var filtered_list = SetFilter(begindate, enddate, phrma, company, employee, section, subsection);
			var returnJSON = filtered_list.Select(a => new { a.Company, a.Email }).GroupBy(a => new { a.Company, a.Email }).Select(a => new { count = a.Count(), company = a.Key.Company, name = a.Key.Email }).ToList();

			var impexiumMembers = _context.ImpexiumMembers.ToList();
			var returnJSONList = returnJSON.ToList();
			List<ImpexiumMemberUser> impx = new List<ImpexiumMemberUser>();
			for (var k = 0; k < returnJSONList.Count(); k++)
			{
				string impexiumName = "";
				try
				{
					if (returnJSONList[k].name.Contains("@phrma.org"))
					{
						impexiumName = _context.AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == returnJSONList[k].name).Select(a => a.GIVEN_NAME + " " + a.SURNAME).FirstOrDefault().ToString();

					}
					else
					{
						impexiumName = impexiumMembers.Where(a => a.Email == returnJSONList[k].name).Select(a => a.First_Name + " " + a.Last_Name).FirstOrDefault().ToString();
					}
				}
				catch (Exception)
				{
					impexiumName = "Unassigned";
				}
				ImpexiumMemberUser uimpx = new ImpexiumMemberUser();
				uimpx.company = returnJSONList[k].company;
				uimpx.count = returnJSONList[k].count;
				uimpx.name = impexiumName;
				uimpx.email = returnJSONList[k].name;
				impx.Add(uimpx);
			}

			var fList = impx.GroupBy(a => new { a.name, a.company, a.email }).Select(a => new { count = a.Sum(q => q.count), company = a.Key.company, name = a.Key.name, email = a.Key.email }).ToList();
			var finalList = fList.OrderByDescending(a => a.count).Take(20).ToList();

			return Json(finalList);


		}

		public IQueryable<SharePointUserActivity> SetFilter(string begindate, string enddate, string phrma, string company, string employee, string section, string subsection) 
		{
			DateTime beginDate = new DateTime(Convert.ToInt32(begindate.Substring(6, 4)), Convert.ToInt32(begindate.Substring(0, 2)), Convert.ToInt32(begindate.Substring(3, 2)), 0, 0, 0);
			DateTime endDate = new DateTime(Convert.ToInt32(enddate.Substring(6, 4)), Convert.ToInt32(enddate.Substring(0, 2)), Convert.ToInt32(enddate.Substring(3, 2)), 23, 59, 59);

			string company_string = company == "" || company == null ? "" : company;
			string employee_string = employee == "" || employee == null ? "" : employee;
			string section_string = section == "" || section == null ? "" : section;
			string subsection_string = subsection == "" || subsection == null ? "" : subsection;

			var unfiltered_list = _context.SharePointUserActivity;
			var initial_filter = unfiltered_list.Where(a => a.Site == "Connect" && a.URL.Contains("payload|") == false && a.Section != "Not Assigned" && a.Section != null && a.ActivityTime.Date >= beginDate && a.ActivityTime.Date <= endDate);
			var company_filter = company_string != "" ? initial_filter.Where(a => a.Company == company) : initial_filter;
			var emp = employee_string.Contains("@phrma.org") ? employee_string : employee_string + "#ext#@phrma.onmicrosoft.com";
			var employee_filter = employee_string != "" ? company_filter.Where(a => a.Email.ToLower() == emp) : company_filter;
			var phrma_filter = phrma != "Include" ? employee_filter.Where(a => a.Company != "PhRMA" && a.IsExternal == "true") : employee_filter.Where(a => a.Company == "PhRMA" || a.IsExternal == "true");
			var section_filter = section_string != "" ? phrma_filter.Where(a => a.Section == section) : phrma_filter;
			var subsection_filter = subsection_string != "" ? section_filter.Where(a => a.SubSection == subsection) : section_filter;
			return subsection_filter;
		}

		[HttpGet]
        public JsonResult GetSiteViewCount(string begindate, string enddate, string company, string timeframe, string phrma, string employee, string section, string subsection)
        {

			var filtered_list = SetFilter(begindate, enddate, phrma, company, employee, section, subsection);

			if (timeframe == "week")
			{
				var returnJSON = filtered_list.GroupBy(a => new { a.ActivityTime.Date }).Select(a => new { count = a.Count(), Day = a.Key.Date }).OrderBy(a => a.Day).ToList();
				return Json(returnJSON);
			}

			else if (timeframe == "day")
			{
				var returnJSON = filtered_list.GroupBy(a => new { a.Hour }).Select(a => new { count = a.Count(), a.Key.Hour }).OrderBy(a => Convert.ToInt64(a.Hour)).ToList();
				return Json(returnJSON);
			}
			else
			{
				var returnJSON = filtered_list.GroupBy(a => new { a.Month, a.Year }).Select(a => new { count = a.Count(), Month = a.Key.Month, Year = a.Key.Year }).OrderBy(a => a.Year).ThenBy(a => Convert.ToInt32(a.Month)).ToList();
				return Json(returnJSON);
			}
		}
	}
}
