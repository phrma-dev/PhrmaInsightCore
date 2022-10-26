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
	public class FinanceController : Controller
	{
		private readonly ILogger<FinanceController> _logger;
		string ClientID = "6ffd2f73-0453-4ed0-b13d-1113d598427d";
		string PowerBiAPI = "https://analysis.windows.net/powerbi/api";
		string AADAuthorityUri = "https://login.microsoftonline.com/common/oauth2/authorize/";
		string ClientSecret = "XzAu040LT3kYjZuZKqmRCN=Hq?AxfA:]";
		//Uri Uri = new Uri("https://localhost:44333/Finance/AuthorizePowerBI");
		Uri Uri = new Uri("https://phrma.azurewebsites.net/Finance/AuthorizePowerBI");
		string baseUri = "https://api.powerbi.com/v1.0/myorg/";
		DatabaseContext _context;

		public FinanceController(ILogger<FinanceController> logger, DatabaseContext context)
		{
			_logger = logger;
			_context = context;
		}

		public IActionResult Dashboard()
		{
			return View();
		}

		public IActionResult Reporting()
		{
			return View();
		}

		public IActionResult Admin()
		{
			return View();
		}

		public IActionResult ForecastLocationSummary()
		{
			var security = _context.Security.Where(a => a.Username == User.Identity.Name.ToLower()).Select(a => a.DepartmentId).Distinct().ToList();

			if (security.Contains("All"))
			{
				ViewBag.Departments = _context.Departments.Distinct().ToList().OrderBy(a => a.DepartmentId);
			}
			else
			{
				ViewBag.Departments = _context.Departments.Where(a => security.Contains(a.DepartmentId)).Distinct().OrderBy(a => a.DepartmentId).ToList();
			}

			return View();
		}

		public IActionResult BudgetingLocationSummary()
		{
			var security = _context.Security.Where(a => a.Username == User.Identity.Name.ToLower()).Select(a => a.DepartmentId).Distinct().ToList();

			if (security.Contains("All"))
			{
				ViewBag.Departments = _context.Departments.Distinct().ToList().OrderBy(a => a.DepartmentId);
			}
			else
			{
				ViewBag.Departments = _context.Departments.Where(a => security.Contains(a.DepartmentId)).Distinct().OrderBy(a => a.DepartmentId).ToList();
			}

			return View();
		}
		[HttpGet]
		public JsonResult GetFinanceReportingUsers() { 
			return Json(_context.Security.Where(a => a.DepartmentId != "None" && a.Username != "powerbiprodsvc@phrma.onmicrosoft.com").OrderBy(a => a.Username).ThenBy(a => a.DepartmentId).ThenBy(a => a.Region).ThenBy(a => a.SecurityUsers).ToList());
		}

		public JsonResult AddFinanceReportingUser(string email, string department, string access, string region) {
			var regionString = region == "None" ? "" : region;
			var alreadyExists = _context.Security.Where(a => a.Username == email && a.DepartmentId == department && a.SecurityUsers == access && a.Region == regionString).Count();
			if (alreadyExists == 0)
			{
				Security security = new Security();
				security.Region = regionString;
				security.SecurityUsers = access;
				security.Username = email;
				security.DepartmentId = department;
				_context.Security.Add(security);
				_context.SaveChanges();
				return Json("OK");
			}
			else { 
				return Json("FAILURE");
			}
		}


		public IActionResult Budgeting(string departmentid, string projectid, string locationid, string hasdata)
		{

			ViewBag.OfficeLocation = _context.UserInfo.Where(x => x.FullUserName == User.Identity.Name).Select(x => x.OfficeLocation).FirstOrDefault().ToString();
			// ViewBag.WeatherUrl = _context.WeatherWidgets.Where(a => a.Location == _context.UserInfo.Where(x => x.FullUserName == User.Identity.Name).Select(x => x.OfficeLocation).Distinct().ToString()).Select(a => a.WeatherURL).ToString();
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

			var departmentSummaryData = _context.Budgeting.Where(a => departmentSecuritySummary.Contains(a.DepartmentId) && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).ToList();
			var summarizedDepartmentSummaryData = departmentSummaryData.GroupBy(a => new { a.DepartmentId, a.DepartmentName, a.ProjectId, a.ProjectName }).Select(a => new { NextYearBoardApproved = a.Sum(x => x.NextYearBoardApproved), Actuals = a.Sum(x => x.ThisYearActuals), Forecast = a.Sum(x => x.ThisYearForecast), NextYearBudget = a.Sum(x => x.NextYearBudget), Department = a.Key.DepartmentId + " - " + a.Key.DepartmentName, Project = a.Key.ProjectId + " - " + a.Key.ProjectName }).ToList();
			List<Budgeting> DeptSummaryDataList = new List<Budgeting>();

			var deptOnlySummaryTotals = departmentSummaryData.GroupBy(a => new { a.DepartmentId, a.DepartmentName })
				.Select(a => new { NextYearBoardApproved = a.Sum(x => x.NextYearBoardApproved), ThisYearActuals = a.Sum(x => x.ThisYearActuals), ThisYearForecast = a.Sum(x => x.ThisYearForecast), NextYearBudget = a.Sum(x => x.NextYearBudget), Department = a.Key.DepartmentId + " - " + a.Key.DepartmentName }).ToList();

			foreach (var item in deptOnlySummaryTotals)
			{
				Budgeting deptSumItem = new Budgeting();
				deptSumItem.DepartmentName = item.Department;
				deptSumItem.ProjectName = "00aa";
				deptSumItem.LocationName = "00aa";
				deptSumItem.ThisYearActuals = item.ThisYearActuals;
				deptSumItem.ThisYearForecast = item.ThisYearForecast;
				deptSumItem.NextYearBudget = item.NextYearBudget;
				deptSumItem.NextYearBoardApproved = item.NextYearBoardApproved;
				deptSumItem.EntryCategory = "DT";
				deptSumItem.CollapseGroup = "#zzqq" + item.Department.Split(" - ")[0].ToString();
				DeptSummaryDataList.Add(deptSumItem);
			}


			foreach (var item in summarizedDepartmentSummaryData)
			{
				Budgeting deptSumItem = new Budgeting();
				deptSumItem.DepartmentName = item.Department;
				deptSumItem.ProjectName = item.Project;
				deptSumItem.LocationName = "00bb";
				deptSumItem.ThisYearActuals = item.Actuals;
				deptSumItem.ThisYearForecast = item.Forecast;
				deptSumItem.NextYearBudget = item.NextYearBudget;
				deptSumItem.NextYearBoardApproved = item.NextYearBoardApproved;
				deptSumItem.EntryCategory = "PT";
				deptSumItem.CollapseGroup = "#zzqq" + item.Department.Split(" - ")[0].ToString();



				DeptSummaryDataList.Add(deptSumItem);
			}

			var summarizedDepartmentLocationProjectSummaryData = departmentSummaryData.GroupBy(a => new { a.DepartmentId, a.DepartmentName, a.ProjectId, a.ProjectName, a.LocationName }).Select(a => new { NextYearBoardApproved = a.Sum(x => x.NextYearBoardApproved), ThisYearActuals = a.Sum(x => x.ThisYearActuals), ThisYearForecast = a.Sum(x => x.ThisYearForecast), NextYearBudget = a.Sum(x => x.NextYearBudget), Department = a.Key.DepartmentId + " - " + a.Key.DepartmentName, Project = a.Key.ProjectId + " - " + a.Key.ProjectName, Location = a.Key.LocationName }).ToList();
			foreach (var item in summarizedDepartmentLocationProjectSummaryData)
			{
				Budgeting deptSumItem = new Budgeting();
				deptSumItem.DepartmentName = item.Department;
				deptSumItem.ProjectName = item.Project;
				deptSumItem.LocationName = item.Location;
				deptSumItem.ThisYearActuals = item.ThisYearActuals;
				deptSumItem.ThisYearForecast = item.ThisYearForecast;
				deptSumItem.NextYearBudget = item.NextYearBudget;
				deptSumItem.NextYearBoardApproved = item.NextYearBoardApproved;
				deptSumItem.EntryCategory = "LT";
				deptSumItem.CollapseGroup = "#zzqq" + item.Department.Split(" - ")[0].ToString();
				DeptSummaryDataList.Add(deptSumItem);
			}



			var grandTotalSummaryDepartment = departmentSummaryData.GroupBy(a => "Total").Select(a => new { NextYearBoardApproved = a.Sum(x => x.NextYearBoardApproved), ThisYearActuals = a.Sum(x => x.ThisYearActuals), ThisYearForecast = a.Sum(x => x.ThisYearForecast), NextYearBudget = a.Sum(x => x.NextYearBudget) }).ToList();
			foreach (var item in grandTotalSummaryDepartment)
			{
				Budgeting deptSumItem = new Budgeting();
				deptSumItem.DepartmentName = "00aa";
				deptSumItem.ProjectName = "00aa";
				deptSumItem.ThisYearActuals = item.ThisYearActuals;
				deptSumItem.ThisYearForecast = item.ThisYearForecast;
				deptSumItem.NextYearBudget = item.NextYearBudget;
				deptSumItem.NextYearBoardApproved = item.NextYearBoardApproved;
				deptSumItem.EntryCategory = "GT";
				DeptSummaryDataList.Add(deptSumItem);
			}


			ViewBag.DepartmentSummaryData = DeptSummaryDataList.OrderBy(a => a.DepartmentName).ThenBy(a => a.ProjectName).ThenBy(a => a.LocationName).ToList();



			if (hasdata != "false")
			{
				List<Budgeting> AllData = new List<Budgeting>();

				List<Budgeting> listForecast = _context.Budgeting.Where(a => a.DepartmentId == departmentid && a.ProjectId == projectid && a.LocationId == locationid && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).ToList();

				Budgeting GrandTotal = new Budgeting();
				GrandTotal.ThisYearActuals = listForecast.Sum(a => a.ThisYearActuals);
				GrandTotal.ThisYearForecast = listForecast.Sum(a => a.ThisYearForecast);
				GrandTotal.NextYearBudget = listForecast.Sum(a => a.NextYearBudget);
				GrandTotal.NextYearBoardApproved = listForecast.Sum(a => a.NextYearBoardApproved);
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




				List<Budgeting> AccountCategoryTotals = new List<Budgeting>();
				var tempList = listForecast.GroupBy(a => new { a.AccountCategoryId, a.AccountCategory }).Select(a => new { NextYearBoardApproved = a.Sum(x => x.NextYearBoardApproved), ThisYearActuals = a.Sum(x => x.ThisYearActuals), ThisYearForecast = a.Sum(x => x.ThisYearForecast), NextYearBudget = a.Sum(x => x.NextYearBudget), AccountCategory = a.Key.AccountCategory, AccountCategoryId = a.Key.AccountCategoryId }).ToList();
				foreach (var item in tempList)
				{
					Budgeting sf = new Budgeting();
					sf.ThisYearActuals = item.ThisYearActuals;
					sf.ThisYearForecast = item.ThisYearForecast;
					sf.NextYearBudget = item.NextYearBudget;
					sf.NextYearBoardApproved = item.NextYearBoardApproved;
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

					AccountCategoryTotals.Add(sf);
				}



				List<Budgeting> MainAccountTotals = new List<Budgeting>();
				listForecast.Select(a => new { a.AccountCategoryId, a.MainAccountId }).Distinct().ToList();


				var tempMainAccountList = listForecast.GroupBy(a => new { a.AccountCategoryId, a.AccountCategory, a.MainAccountId, a.MainAccountName }).Select(a => new { NextYearBoardApproved = a.Sum(x => x.NextYearBoardApproved), ThisYearActuals = a.Sum(x => x.ThisYearActuals), ThisYearForecast = a.Sum(x => x.ThisYearForecast), NextYearBudget = a.Sum(x => x.NextYearBudget), AccountCategory = a.Key.AccountCategory, AccountCategoryId = a.Key.AccountCategoryId, MainAccountName = a.Key.MainAccountName, MainAccountId = a.Key.MainAccountId }).ToList();

				foreach (var item in tempMainAccountList)
				{
					Budgeting sf = new Budgeting();
					sf.ThisYearActuals = item.ThisYearActuals;
					sf.ThisYearForecast = item.ThisYearForecast;
					sf.NextYearBudget = item.NextYearBudget;
					sf.NextYearBoardApproved = item.NextYearBoardApproved;
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

					MainAccountTotals.Add(sf);
				}



				var tempLineItemList = listForecast.GroupBy(a => new { a.AccountCategoryId, a.AccountCategory, a.MainAccountId, a.MainAccountName, a.ProjectId, a.ProjectName, a.DepartmentId, a.DepartmentName, a.ReferenceCodeId, a.ReferenceCodeName, a.LocationId, a.LocationName, a.VendorId, a.VendorName, a.Note }).Select(a => new { NextYearBoardApproved = a.Sum(x => x.NextYearBoardApproved), ThisYearActuals = a.Sum(x => x.ThisYearActuals), ThisYearForecast = a.Sum(x => x.ThisYearForecast), NextYearBudget = a.Sum(x => x.NextYearBudget), AccountCategory = a.Key.AccountCategory, AccountCategoryId = a.Key.AccountCategoryId, MainAccountName = a.Key.MainAccountName, MainAccountId = a.Key.MainAccountId, VendorName = a.Key.VendorName, VendorId = a.Key.VendorId, ReferenceCodeName = a.Key.ReferenceCodeName, ReferenceCodeId = a.Key.ReferenceCodeId, ProjectId = a.Key.ProjectId, ProjectName = a.Key.ProjectName, DepartmentName = a.Key.DepartmentName, DepartmentId = a.Key.DepartmentId, LocationId = a.Key.LocationId, LocationName = a.Key.LocationName, Note = a.Key.Note }).ToList();
				List<Budgeting> LineItemTotals = new List<Budgeting>();

				foreach (var item in tempLineItemList)
				{
					Budgeting sf = new Budgeting();
					sf.ThisYearActuals = item.ThisYearActuals;
					sf.ThisYearForecast = item.ThisYearForecast;
					sf.NextYearBudget = item.NextYearBudget;
					sf.NextYearBoardApproved = item.NextYearBoardApproved;
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
				ViewBag.MainAccounts = _context.MainAccounts.Distinct().OrderBy(a => Convert.ToInt32(a.MainAccountId)).ToList();
				ViewBag.Department = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Department).FirstOrDefault().ToString();
				ViewBag.ReferenceCode = _context.ReferenceCode.OrderBy(a => a.ReferenceCodeName).ToList();

				var allowedProjects = _context.Budgeting.Where(a => allowedDepartments.Contains(a.DepartmentId) && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).Select(a => a.ProjectId).Distinct().ToList();

				ViewBag.Projects = _context.Projects.Where(a => allowedProjects.Contains(a.ProjectId)).OrderBy(a => a.ProjectId).ToList();
				ViewBag.DistinctDimensions = JsonConvert.SerializeObject(_context.Budgeting.Select(a => new { a.DepartmentId, a.ProjectId, a.LocationId }).Distinct().ToList()).ToString();
				ViewBag.Locations = _context.Locations.Where(a => allowedRegions.Contains(a.RegionName)).OrderBy(a => a.LocationId).ToList();



				return View(model);
			}
			else
			{



				ViewBag.Vendors = _context.Vendors.Distinct().OrderBy(a => a.VendorName).ToList();
				ViewBag.MainAccounts = _context.MainAccounts.Distinct().OrderBy(a => Convert.ToInt32(a.MainAccountId)).ToList();
				ViewBag.Department = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Department).FirstOrDefault().ToString();
				ViewBag.ReferenceCode = _context.ReferenceCode.OrderBy(a => a.ReferenceCodeName).ToList();
				var allowedProjects = _context.Budgeting.Where(a => allowedDepartments.Contains(a.DepartmentId) && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).Select(a => a.ProjectId).Distinct().ToList();
				ViewBag.Projects = _context.Projects.Where(a => allowedProjects.Contains(a.ProjectId)).OrderBy(a => a.ProjectId).ToList();
				ViewBag.HasData = "false";
				ViewBag.DistinctDimensions = JsonConvert.SerializeObject(_context.Budgeting.Select(a => new { a.DepartmentId, a.ProjectId, a.LocationId }).Distinct().ToList()).ToString();
				ViewBag.Locations = _context.Locations.Where(a => allowedRegions.Contains(a.RegionName)).OrderBy(a => a.LocationId).ToList();
				var model = _context.Budgeting.ToList();
				return View(model);
			}

		}

		[HttpDelete]
		public JsonResult DeleteFinanceReportingUsers(string id)
		{
			var deleteThis = _context.Security.Where(a => a.Id == Convert.ToInt32(id)).FirstOrDefault();
			_context.Security.Remove(deleteThis);
			_context.SaveChanges();
			return Json("OK");
		}

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
                                { "redirect_uri", "https://localhost:44333/Finance/GraphTest"}
                                //{ "redirect_uri", "https://phrma.azurewebsites.net/Finance/GraphTest"}
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

		public async Task<JsonResult> GraphTest(string code)
		{
			if (code == null)
			{
				GetCode();
			}
			else { 
				TokenCache TC = new TokenCache();
				AuthenticationContext AC = new AuthenticationContext(AADAuthorityUri, TC);
				ClientCredential cc = new ClientCredential(ClientID, ClientSecret);
				//AuthenticationResult AR = await AC.AcquireTokenByAuthorizationCodeAsync(code, new Uri("https://phrma.azurewebsites.net/Finance/GraphTest"), cc);
				AuthenticationResult AR = await AC.AcquireTokenByAuthorizationCodeAsync(code, new Uri("https://localhost:44333/Finance/GraphTest"), cc);
				var authenticationResult = AR;
				var accessToken = authenticationResult.AccessToken;
				HttpClient client = new HttpClient();
				client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accessToken.ToString());

				var population = _context.AzureADUsers.ToList();
				
				string photos = "";
				var k = 0;
				List<UserInfo> info = new List<UserInfo>();
				List<AzureADUsers> updateManagerList = new List<AzureADUsers>();
				foreach (var item in population) 
				{
					try
					{
						var url = "https://graph.microsoft.com/v1.0/users/" + item.USER_PRINCIPAL_NAME.ToLower() + "/manager";
						var result = await client.GetAsync(url);
						var response = result.Content.ReadAsStringAsync();
						JObject json = JObject.Parse(response.Result);
						var manager = json["userPrincipalName"].ToString().ToLower();
						var person = item;
						person.AZURE_AD_MANAGER = manager;
						updateManagerList.Add(person);
					}
					catch (Exception)
					{
						continue;
						
					}


					//var pictureMemoryStream = new MemoryStream();
					//await result.Content.CopyToAsync(pictureMemoryStream);
					//var pictureByteArray = pictureMemoryStream.ToArray();
					//var pictureBase64 = Convert.ToBase64String(pictureByteArray);
					//photos += "------" + k + " " + item.GIVEN_NAME + " " + item.SURNAME + ": photo = " + pictureBase64.Substring(0,10);
					//k++;
					//var updateimage = _context.UserInfo.Where(a => a.FullUserName == item.USER_PRINCIPAL_NAME.ToLower()).ToList();
					//foreach (var itemz in updateimage)
					//{
					//	itemz.Image = "data:image/jpeg;base64," + pictureBase64.ToString();
					//	info.Add(itemz);
						
					//}
					//_context.UserInfo.UpdateRange(info);
					//_context.SaveChanges();

				}
				_context.UpdateRange(updateManagerList);
				_context.SaveChanges();
				return Json("OK");
				//return Json(photos);
			}

			return Json("OK");
		}

		[HttpGet]
		public async Task<IActionResult> AuthorizeTeamsPowerBI(string code)
		{

			var accessToken = "";
			if (code == null)
			{
				GetTeamsAuthorizationCode();
			}
			else
			{
				var authenticationResult = await GetTeamsToken(code);
				accessToken = authenticationResult.AccessToken;

				ViewBag.AccessToken = accessToken;
				//Configure Reports request
				System.Net.WebRequest request = System.Net.WebRequest.Create(
				String.Format("{0}/Reports",
				baseUri)) as System.Net.HttpWebRequest;

				request.Method = "GET";
				request.ContentLength = 0;
				request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken));
			}
			return View();
			
		}
		public async Task<AuthenticationResult> GetTeamsToken(string code)
		{
			Uri uri = new Uri("https://phrma.azurewebsites.net/Finance/AuthorizeTeamsPowerBI");
			TokenCache TC = new TokenCache();
			AuthenticationContext AC = new AuthenticationContext(AADAuthorityUri, TC);
			ClientCredential cc = new ClientCredential(ClientID, ClientSecret);
			AuthenticationResult AR = await AC.AcquireTokenByAuthorizationCodeAsync(code, uri, cc);

			return AR;
		}
		protected void GetTeamsAuthorizationCode()
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
				 //{"redirect_uri", "https://localhost:44333/Finance/AuthorizePowerBI"}
				  {"redirect_uri", "https://phrma.azurewebsites.net/Finance/AuthorizeTeamsPowerBI"}
			  };

			//Create sign-in query string
			var queryString = HttpUtility.ParseQueryString(string.Empty);
			queryString.Add(@params);
			string authorityUri = AADAuthorityUri;
			var authUri = String.Format("{0}?{1}", authorityUri, queryString);
			Response.Redirect(authUri);


		}



		[HttpGet]
		public async Task<IActionResult> AuthorizePowerBI(string code)
		{


		  if (code == null)
		  {
			GetAuthorizationCode();
		  }


		  if (code != null)
		  {
			var authenticationResult = await GetToken(code);
			var accessToken = authenticationResult.AccessToken;

			ViewBag.AccessToken = accessToken;
			//Configure Reports request
			System.Net.WebRequest request = System.Net.WebRequest.Create(
			String.Format("{0}/Reports",
			baseUri)) as System.Net.HttpWebRequest;

			request.Method = "GET";
			request.ContentLength = 0;
			request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken));
		  }

		  return View();
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
				 //{"redirect_uri", "https://localhost:44333/Finance/AuthorizePowerBI"}
				  {"redirect_uri", "https://phrma.azurewebsites.net/Finance/AuthorizePowerBI"}
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

		[HttpPost]
		public JsonResult UpdateNextYearBudgetItem([FromBody] PostNextYearBudgetItem postnybi)
		{



			string note = postnybi.note;
			var items = postnybi.query.ToString().Split("|");
			var department = items[0];
			var project = items[1];
			var location = items[2];
			var mainaccount = items[3];
			var vendor = items[4];
			var referencecode = items[5];
			string budget = items[6];

			var updateditem = _context.Budgeting.Where(a => a.DepartmentId == department && a.ProjectId == project && a.LocationId == location && a.MainAccountId == mainaccount && a.VendorId == vendor && a.ReferenceCodeId == referencecode).FirstOrDefault();
			updateditem.NextYearBudget = Convert.ToInt32(budget);
			updateditem.Note = note;

			_context.Budgeting.Update(updateditem);
			_context.SaveChanges();




			return Json("success");

		}

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

				if (referencecode != "None")
				{
					item.ReferenceCodeName = referencecode;
				}
				else
				{
					item.ReferenceCodeName = "None";
				}



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
			else
			{
				return Json("failure");
			}
		}
		[HttpGet]
		public JsonResult GetBudgetingLocation(string departmentid, string projectid, string locationid)
		{


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

			var departmentSummaryData = _context.Budgeting.Where(a => departmentSecuritySummary.Contains(a.DepartmentId) && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).ToList();
			var summarizedDepartmentSummaryData = departmentSummaryData.GroupBy(a => new { a.DepartmentId, a.DepartmentName, a.ProjectId, a.ProjectName }).Select(a => new { NextYearBoardApproved = a.Sum(x => x.NextYearBoardApproved), ThisYearActuals = a.Sum(x => x.ThisYearActuals), ThisYearForecast = a.Sum(x => x.ThisYearForecast), NextYearBudget = a.Sum(x => x.NextYearBudget), Department = a.Key.DepartmentId + " - " + a.Key.DepartmentName, Project = a.Key.ProjectId + " - " + a.Key.ProjectName }).ToList();
			List<Budgeting> DeptSummaryDataList = new List<Budgeting>();

			var deptOnlySummaryTotals = departmentSummaryData.GroupBy(a => new { a.DepartmentId, a.DepartmentName }).Select(a => new { NextYearBoardApproved = a.Sum(x => x.NextYearBoardApproved), ThisYearActuals = a.Sum(x => x.ThisYearActuals), ThisYearForecast = a.Sum(x => x.ThisYearForecast), NextYearBudget = a.Sum(x => x.NextYearBudget), Department = a.Key.DepartmentId + " - " + a.Key.DepartmentName }).ToList();

			foreach (var item in deptOnlySummaryTotals)
			{
				Budgeting deptSumItem = new Budgeting();
				deptSumItem.DepartmentName = item.Department;
				deptSumItem.ProjectName = "00aa";
				deptSumItem.LocationName = "00aa";
				deptSumItem.ThisYearActuals = item.ThisYearActuals;
				deptSumItem.ThisYearForecast = item.ThisYearForecast;
				deptSumItem.NextYearBudget = item.NextYearBudget;
				deptSumItem.NextYearBoardApproved = item.NextYearBoardApproved;
				deptSumItem.EntryCategory = "DT";
				deptSumItem.CollapseGroup = "#zzqq" + item.Department.Split(" - ")[0].ToString();
				DeptSummaryDataList.Add(deptSumItem);
			}


			foreach (var item in summarizedDepartmentSummaryData)
			{
				Budgeting deptSumItem = new Budgeting();
				deptSumItem.DepartmentName = item.Department;
				deptSumItem.ProjectName = item.Project;
				deptSumItem.LocationName = "00bb";
				deptSumItem.ThisYearActuals = item.ThisYearActuals;
				deptSumItem.ThisYearForecast = item.ThisYearForecast;
				deptSumItem.NextYearBudget = item.NextYearBudget;
				deptSumItem.NextYearBoardApproved = item.NextYearBoardApproved;
				deptSumItem.EntryCategory = "PT";
				deptSumItem.CollapseGroup = "#zzqq" + item.Department.Split(" - ")[0].ToString();



				DeptSummaryDataList.Add(deptSumItem);
			}

			var summarizedDepartmentLocationProjectSummaryData = departmentSummaryData.GroupBy(a => new { a.DepartmentId, a.DepartmentName, a.ProjectId, a.ProjectName, a.LocationName }).Select(a => new { NextYearBoardApproved = a.Sum(x => x.NextYearBoardApproved), ThisYearActuals = a.Sum(x => x.ThisYearActuals), ThisYearForecast = a.Sum(x => x.ThisYearForecast), NextYearBudget = a.Sum(x => x.NextYearBudget), Department = a.Key.DepartmentId + " - " + a.Key.DepartmentName, Project = a.Key.ProjectId + " - " + a.Key.ProjectName, Location = a.Key.LocationName }).ToList();
			foreach (var item in summarizedDepartmentLocationProjectSummaryData)
			{
				Budgeting deptSumItem = new Budgeting();
				deptSumItem.DepartmentName = item.Department;
				deptSumItem.ProjectName = item.Project;
				deptSumItem.LocationName = item.Location;
				deptSumItem.ThisYearActuals = item.ThisYearActuals;
				deptSumItem.ThisYearForecast = item.ThisYearForecast;
				deptSumItem.NextYearBudget = item.NextYearBudget;
				deptSumItem.NextYearBoardApproved = item.NextYearBoardApproved;
				deptSumItem.EntryCategory = "LT";
				deptSumItem.CollapseGroup = "#zzqq" + item.Department.Split(" - ")[0].ToString();
				DeptSummaryDataList.Add(deptSumItem);
			}



			var grandTotalSummaryDepartment = departmentSummaryData.GroupBy(a => "Total")
				.Select(a => new { NextYearBoardApproved = a.Sum(x => x.NextYearBoardApproved), ThisYearActuals = a.Sum(x => x.ThisYearActuals), ThisYearForecast = a.Sum(x => x.ThisYearForecast), NextYearBudget = a.Sum(x => x.NextYearBudget)}).ToList();
			foreach (var item in grandTotalSummaryDepartment)
			{
				Budgeting deptSumItem = new Budgeting();
				deptSumItem.DepartmentName = "00aa";
				deptSumItem.ProjectName = "00aa";
				deptSumItem.ThisYearActuals = item.ThisYearActuals;
				deptSumItem.ThisYearForecast = item.ThisYearForecast;
				deptSumItem.NextYearBudget = item.NextYearBudget;
				deptSumItem.NextYearBoardApproved = item.NextYearBoardApproved;
				deptSumItem.EntryCategory = "GT";
				DeptSummaryDataList.Add(deptSumItem);
			}



			List<Budgeting> AllData = new List<Budgeting>();

			List<Budgeting> listForecast = _context.Budgeting.Where(a => a.DepartmentId == departmentid && a.ProjectId == projectid && a.LocationId == locationid && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).ToList();

			Budgeting GrandTotal = new Budgeting();
			GrandTotal.ThisYearActuals = listForecast.Sum(a => a.ThisYearActuals);
            GrandTotal.ThisYearForecast = listForecast.Sum(a => a.ThisYearForecast);
            GrandTotal.NextYearBudget = listForecast.Sum(a => a.NextYearBudget);
			GrandTotal.NextYearBoardApproved = listForecast.Sum(a => a.NextYearBoardApproved);
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




			List<Budgeting> AccountCategoryTotals = new List<Budgeting>();
			var tempList = listForecast.GroupBy(a => new { a.AccountCategoryId, a.AccountCategory }).Select(a => new { NextYearBoardApproved = a.Sum(x => x.NextYearBoardApproved),  ThisYearActuals = a.Sum(x => x.ThisYearActuals), ThisYearForecast = a.Sum(x => x.ThisYearForecast), NextYearBudget = a.Sum(x => x.NextYearBudget), AccountCategory = a.Key.AccountCategory, AccountCategoryId = a.Key.AccountCategoryId }).ToList();
			foreach (var item in tempList)
			{
				Budgeting sf = new Budgeting();
				sf.ThisYearActuals = item.ThisYearActuals;
				sf.ThisYearForecast = item.ThisYearForecast;
				sf.NextYearBudget = item.NextYearBudget;
				sf.NextYearBoardApproved = item.NextYearBoardApproved;
				sf.DepartmentId = departmentid;
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

				AccountCategoryTotals.Add(sf);
			}



			List<Budgeting> MainAccountTotals = new List<Budgeting>();
			listForecast.Select(a => new { a.AccountCategoryId, a.MainAccountId }).Distinct().ToList();


			var tempMainAccountList = listForecast.GroupBy(a => new { a.AccountCategoryId, a.AccountCategory, a.MainAccountId, a.MainAccountName }).Select(a => new { NextYearBoardApproved = a.Sum(x => x.NextYearBoardApproved), ThisYearActuals = a.Sum(x => x.ThisYearActuals), ThisYearForecast = a.Sum(x => x.ThisYearForecast), NextYearBudget = a.Sum(x => x.NextYearBudget), AccountCategory = a.Key.AccountCategory, AccountCategoryId = a.Key.AccountCategoryId, MainAccountName = a.Key.MainAccountName, MainAccountId = a.Key.MainAccountId }).ToList();

			foreach (var item in tempMainAccountList)
			{
				Budgeting sf = new Budgeting();
				sf.ThisYearActuals = item.ThisYearActuals;
				sf.ThisYearForecast = item.ThisYearForecast;
				sf.NextYearBudget = item.NextYearBudget;	
				sf.NextYearBoardApproved = item.NextYearBoardApproved;
				sf.DepartmentId = departmentid;
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

				MainAccountTotals.Add(sf);
			}



			var tempLineItemList = listForecast.GroupBy(a => new { a.AccountCategoryId, a.AccountCategory, a.MainAccountId, a.MainAccountName, a.ProjectId, a.ProjectName, a.DepartmentId, a.DepartmentName, a.ReferenceCodeId, a.ReferenceCodeName, a.LocationId, a.LocationName, a.VendorId, a.VendorName, a.Note }).Select(a => new { NextYearBoardApproved = a.Sum(x => x.NextYearBoardApproved), ThisYearActuals = a.Sum(x => x.ThisYearActuals), ThisYearForecast = a.Sum(x => x.ThisYearForecast), NextYearBudget = a.Sum(x => x.NextYearBudget), AccountCategory = a.Key.AccountCategory, AccountCategoryId = a.Key.AccountCategoryId, MainAccountName = a.Key.MainAccountName, MainAccountId = a.Key.MainAccountId, VendorName = a.Key.VendorName, VendorId = a.Key.VendorId, ReferenceCodeName = a.Key.ReferenceCodeName, ReferenceCodeId = a.Key.ReferenceCodeId, ProjectId = a.Key.ProjectId, ProjectName = a.Key.ProjectName, DepartmentName = a.Key.DepartmentName, DepartmentId = a.Key.DepartmentId, LocationId = a.Key.LocationId, LocationName = a.Key.LocationName, Note = a.Key.Note }).ToList();
			List<Budgeting> LineItemTotals = new List<Budgeting>();

			foreach (var item in tempLineItemList)
			{
				Budgeting sf = new Budgeting();
				sf.ThisYearActuals = item.ThisYearActuals;
				sf.NextYearBoardApproved = item.NextYearBoardApproved;
				sf.ThisYearForecast = item.ThisYearForecast;
				sf.NextYearBudget = item.NextYearBudget;
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
			var model = AllData;
			return Json(model);

		}



		[HttpGet]
		public JsonResult DeleteBudgetItem(string deleteid)
		{
			var array = deleteid.Split('|');
			string departmentid = array[0].ToString();
			string projectid = array[1].ToString();
			string locationid = array[2].ToString();
			string mainaccountid = array[3].ToString();
			string vendorid = array[4].ToString();
			string referencecodeid = array[5].ToString();

			var deleteItem = _context.Budgeting.Where(a => a.DepartmentId == departmentid && a.ProjectId == projectid && a.LocationId == locationid && a.MainAccountId == mainaccountid && a.VendorId == vendorid && a.ReferenceCodeId == referencecodeid).FirstOrDefault();
			_context.Budgeting.Remove(deleteItem);
			_context.SaveChanges();
			return Json("OK");
		}
		[HttpGet]
		public JsonResult GetBudgetingLocationSummary()
		{

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
			var security = _context.Security.Where(a => a.Username == User.Identity.Name.ToLower()).Select(a => a.DepartmentId).Distinct().ToList();

			var securityUsers = _context.Security.Where(a => a.Username == User.Identity.Name.ToLower()).Select(a => a.SecurityUsers).ToList();

			int AllowedAccountCategories = securityUsers.Contains("All") ? 100 : 15;

			if (security.Contains("All"))
			{
				departmentSecuritySummary = _context.Departments.Distinct().Select(a => a.DepartmentId).ToList();
				allowedDepartments = _context.Departments.Distinct().ToList().Select(a => a.DepartmentId).ToList();
			}
			else
			{
				departmentSecuritySummary = security;
				allowedDepartments = _context.Departments.Where(a => security.Contains(a.DepartmentId)).Distinct().Select(a => a.DepartmentId).ToList();
			}

			var data = _context.Budgeting.Where(a => departmentSecuritySummary.Contains(a.DepartmentId) && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).ToList();
			var GrandTotal = data.GroupBy(a => a.DepartmentId != "None").Select(a =>
									new {

										NextYearBudget = a.Sum(a => a.NextYearBudget),
										NextYearBoardApproved = a.Sum(a => a.NextYearBoardApproved),
										ThisYearForecast = a.Sum(a => a.ThisYearForecast),
										DepartmentId = "All",
										DepartmentName = "Grand Total",
										LocationName = "",
										LocationId = "",
										ProjectId = "",
										EntryCategory = "GT"
									}
								).FirstOrDefault();
			List<object> AllData = new List<object>();
			var DepartmentTotal = data.GroupBy(a => new { a.DepartmentId, a.DepartmentName }).Select(a => 
									new {
										NextYearBudget = a.Sum(a => a.NextYearBudget),
										NextYearBoardApproved = a.Sum(a => a.NextYearBoardApproved),
										ThisYearForecast = a.Sum(a => a.ThisYearForecast),
										DepartmentId = a.Key.DepartmentId,
										DepartmentName = a.Key.DepartmentName,
										LocationName = "",
										LocationId = "",
										ProjectId = "",
										EntryCategory = "DT"
									}
								).OrderBy(a => a.DepartmentId);
			var ProjectTotal = data.GroupBy(a => new { a.DepartmentId, a.DepartmentName, a.ProjectId, a.ProjectName })
								   .Select(a =>
										new
										{
											NextYearBudget = a.Sum(a => a.NextYearBudget),
											NextYearBoardApproved = a.Sum(a => a.NextYearBoardApproved),
											ThisYearForecast = a.Sum(a => a.ThisYearForecast),
											DepartmentId = a.Key.DepartmentId,
											DepartmentName = a.Key.DepartmentName,
											LocationName = "",
											LocationId = "",
											ProjectId = a.Key.ProjectId,
											ProjectName = a.Key.ProjectName,
											EntryCategory = "PT"
										}
									).OrderBy(a => a.DepartmentId).ThenBy(a => a.ProjectId);

			var LocationTotal = data.GroupBy(a => new { a.DepartmentId, a.DepartmentName, a.ProjectId, a.ProjectName, a.LocationId, a.LocationName })
									.Select(a =>
										new
										{
										 
											NextYearBudget = a.Sum(a => a.NextYearBudget),
											NextYearBoardApproved = a.Sum(a => a.NextYearBoardApproved),
											ThisYearForecast = a.Sum(a => a.ThisYearForecast),
											DepartmentId = a.Key.DepartmentId,
											DepartmentName = a.Key.DepartmentName,
											LocationName = a.Key.LocationName,
											LocationId = a.Key.LocationId,
											ProjectId = a.Key.ProjectId,
											ProjectName = a.Key.ProjectName,
											EntryCategory = "LT"
										}
									).OrderBy(a => a.DepartmentId).ThenBy(a => a.ProjectId).ThenBy(a => a.LocationName).ToList();



			// AllData.Insert(0, GrandTotal);
			AllData.Add(GrandTotal);
			foreach (var dept in DepartmentTotal)
			{
				AllData.Add(dept);
				var projects = ProjectTotal.Where(a => a.DepartmentId == dept.DepartmentId).OrderBy(a => a.ProjectId).ToList();
				foreach (var proj in projects)
				{
					AllData.Add(proj);
					var locations = LocationTotal.Where(a => a.DepartmentId == proj.DepartmentId && a.ProjectId == proj.ProjectId).OrderBy(a => a.LocationName).ToList();
					AllData.AddRange(locations);
				}
			}
			
			return Json(AllData);

		}


		[HttpGet]
		public JsonResult GetForecastLocation(string departmentid, string projectid, string locationid)
		{


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

			var departmentSummaryData = _context.Forecasting.Where(a => departmentSecuritySummary.Contains(a.DepartmentId) && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).ToList();
			var summarizedDepartmentSummaryData = departmentSummaryData.GroupBy(a => new { a.DepartmentId, a.DepartmentName, a.ProjectId, a.ProjectName }).Select(a => new {Actuals = a.Sum(x => x.Actuals), Forecast = a.Sum(x => x.Forecast), WorkingBudget = a.Sum(x => x.WorkingBudget), HardCommitments = a.Sum(x => x.HardCommitments), Requisitions = a.Sum(x => x.Requisitions), TotalCommitted = a.Sum(x => x.TotalCommitted), Department = a.Key.DepartmentId + " - " + a.Key.DepartmentName, Project = a.Key.ProjectId + " - " + a.Key.ProjectName }).ToList();
			List<Forecasting> DeptSummaryDataList = new List<Forecasting>();

			var deptOnlySummaryTotals = departmentSummaryData.GroupBy(a => new { a.DepartmentId, a.DepartmentName }).Select(a => new { Actuals = a.Sum(x => x.Actuals), Forecast = a.Sum(x => x.Forecast), WorkingBudget = a.Sum(x => x.WorkingBudget), HardCommitments = a.Sum(x => x.HardCommitments), Requisitions = a.Sum(x => x.Requisitions), TotalCommitted = a.Sum(x => x.TotalCommitted), Department = a.Key.DepartmentId + " - " + a.Key.DepartmentName }).ToList();

			foreach (var item in deptOnlySummaryTotals)
			{
				Forecasting deptSumItem = new Forecasting();
				deptSumItem.DepartmentName = item.Department;
				deptSumItem.ProjectName = "00aa";
				deptSumItem.LocationName = "00aa";
				deptSumItem.Actuals = item.Actuals;
				deptSumItem.Forecast = item.Forecast;
				deptSumItem.WorkingBudget = item.WorkingBudget;
				deptSumItem.TotalCommitted = item.TotalCommitted;
				deptSumItem.Requisitions = item.Requisitions;
				deptSumItem.HardCommitments = item.HardCommitments;
				deptSumItem.EntryCategory = "DT";
				deptSumItem.CollapseGroup = "#zzqq" + item.Department.Split(" - ")[0].ToString();
				DeptSummaryDataList.Add(deptSumItem);
			}


			foreach (var item in summarizedDepartmentSummaryData)
			{
				Forecasting deptSumItem = new Forecasting();
				deptSumItem.DepartmentName = item.Department;
				deptSumItem.ProjectName = item.Project;
				deptSumItem.LocationName = "00bb";
				deptSumItem.Actuals = item.Actuals;
				deptSumItem.Forecast = item.Forecast;
				deptSumItem.WorkingBudget = item.WorkingBudget;
				deptSumItem.TotalCommitted = item.TotalCommitted;
				deptSumItem.Requisitions = item.Requisitions;
				deptSumItem.HardCommitments = item.HardCommitments;
				deptSumItem.EntryCategory = "PT";
				deptSumItem.CollapseGroup = "#zzqq" + item.Department.Split(" - ")[0].ToString();



				DeptSummaryDataList.Add(deptSumItem);
			}

			var summarizedDepartmentLocationProjectSummaryData = departmentSummaryData.GroupBy(a => new { a.DepartmentId, a.DepartmentName, a.ProjectId, a.ProjectName, a.LocationName }).Select(a => new { Actuals = a.Sum(x => x.Actuals), Forecast = a.Sum(x => x.Forecast), WorkingBudget = a.Sum(x => x.WorkingBudget), HardCommitments = a.Sum(x => x.HardCommitments), Requisitions = a.Sum(x => x.Requisitions), TotalCommitted = a.Sum(x => x.TotalCommitted), Department = a.Key.DepartmentId + " - " + a.Key.DepartmentName, Project = a.Key.ProjectId + " - " + a.Key.ProjectName, Location = a.Key.LocationName }).ToList();
			foreach (var item in summarizedDepartmentLocationProjectSummaryData)
			{
				Forecasting deptSumItem = new Forecasting();
				deptSumItem.DepartmentName = item.Department;
				deptSumItem.ProjectName = item.Project;
				deptSumItem.LocationName = item.Location;
				deptSumItem.Actuals = item.Actuals;
				deptSumItem.Forecast = item.Forecast;
				deptSumItem.WorkingBudget = item.WorkingBudget;
				deptSumItem.TotalCommitted = item.TotalCommitted;
				deptSumItem.Requisitions = item.Requisitions;
				deptSumItem.HardCommitments = item.HardCommitments;
				deptSumItem.EntryCategory = "LT";
				deptSumItem.CollapseGroup = "#zzqq" + item.Department.Split(" - ")[0].ToString();
				DeptSummaryDataList.Add(deptSumItem);
			}



			var grandTotalSummaryDepartment = departmentSummaryData.GroupBy(a => "Total").Select(a => new { Actuals = a.Sum(x => x.Actuals), Forecast = a.Sum(x => x.Forecast), WorkingBudget = a.Sum(x => x.WorkingBudget), HardCommitments = a.Sum(x => x.HardCommitments), Requisitions = a.Sum(x => x.Requisitions), TotalCommitted = a.Sum(x => x.TotalCommitted) }).ToList();
			foreach (var item in grandTotalSummaryDepartment)
			{
				Forecasting deptSumItem = new Forecasting();
				deptSumItem.DepartmentName = "00aa";
				deptSumItem.ProjectName = "00aa";
				deptSumItem.Actuals = item.Actuals;
				deptSumItem.Forecast = item.Forecast;
				deptSumItem.WorkingBudget = item.WorkingBudget;
				deptSumItem.TotalCommitted = item.TotalCommitted;
				deptSumItem.Requisitions = item.Requisitions;
				deptSumItem.HardCommitments = item.HardCommitments;
				deptSumItem.EntryCategory = "GT";
				DeptSummaryDataList.Add(deptSumItem);
			}



			List<Forecasting> AllData = new List<Forecasting>();

			List<Forecasting> listForecast = _context.Forecasting.Where(a => a.DepartmentId == departmentid && a.ProjectId == projectid && a.LocationId == locationid && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).ToList();

			Forecasting GrandTotal = new Forecasting();
			GrandTotal.Actuals = listForecast.Sum(a => a.Actuals);
			GrandTotal.Forecast = listForecast.Sum(a => a.Forecast);
			GrandTotal.WorkingBudget = listForecast.Sum(a => a.WorkingBudget);
			GrandTotal.TotalCommitted = listForecast.Sum(a => a.TotalCommitted);
			GrandTotal.HardCommitments = listForecast.Sum(a => a.HardCommitments);
			GrandTotal.Requisitions = listForecast.Sum(a => a.Requisitions);
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




			List<Forecasting> AccountCategoryTotals = new List<Forecasting>();
			var tempList = listForecast.GroupBy(a => new { a.AccountCategoryId, a.AccountCategory }).Select(a => new { Actuals = a.Sum(x => x.Actuals), Forecast = a.Sum(x => x.Forecast), WorkingBudget = a.Sum(x => x.WorkingBudget), HardCommitments = a.Sum(x => x.HardCommitments), Requisitions = a.Sum(x => x.Requisitions), TotalCommitted = a.Sum(x => x.TotalCommitted), AccountCategory = a.Key.AccountCategory, AccountCategoryId = a.Key.AccountCategoryId }).ToList();
			foreach (var item in tempList)
			{
				Forecasting sf = new Forecasting();
				sf.Actuals = item.Actuals;
				sf.Forecast = item.Forecast;
				sf.WorkingBudget = item.WorkingBudget;
				sf.HardCommitments = item.HardCommitments;
				sf.Requisitions = item.Requisitions;
				sf.TotalCommitted = item.TotalCommitted;
				sf.DepartmentId = departmentid;
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

				AccountCategoryTotals.Add(sf);
			}



			List<Forecasting> MainAccountTotals = new List<Forecasting>();
			listForecast.Select(a => new { a.AccountCategoryId, a.MainAccountId }).Distinct().ToList();


			var tempMainAccountList = listForecast.GroupBy(a => new { a.AccountCategoryId, a.AccountCategory, a.MainAccountId, a.MainAccountName }).Select(a => new { Actuals = a.Sum(x => x.Actuals), Forecast = a.Sum(x => x.Forecast), WorkingBudget = a.Sum(x => x.WorkingBudget), HardCommitments = a.Sum(x => x.HardCommitments), Requisitions = a.Sum(x => x.Requisitions), TotalCommitted = a.Sum(x => x.TotalCommitted), AccountCategory = a.Key.AccountCategory, AccountCategoryId = a.Key.AccountCategoryId, MainAccountName = a.Key.MainAccountName, MainAccountId = a.Key.MainAccountId }).ToList();

			foreach (var item in tempMainAccountList)
			{
				Forecasting sf = new Forecasting();
				sf.Actuals = item.Actuals;
				sf.Forecast = item.Forecast;
				sf.WorkingBudget = item.WorkingBudget;
				sf.HardCommitments = item.HardCommitments;
				sf.Requisitions = item.Requisitions;
				sf.TotalCommitted = item.TotalCommitted;
				sf.DepartmentId = departmentid;
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

				MainAccountTotals.Add(sf);
			}



			var tempLineItemList = listForecast.GroupBy(a => new { a.AccountCategoryId, a.AccountCategory, a.MainAccountId, a.MainAccountName, a.ProjectId, a.ProjectName, a.DepartmentId, a.DepartmentName, a.ReferenceCodeId, a.ReferenceCodeName, a.LocationId, a.LocationName, a.VendorId, a.VendorName, a.Note }).Select(a => new { Actuals = a.Sum(x => x.Actuals), Forecast = a.Sum(x => x.Forecast), WorkingBudget = a.Sum(x => x.WorkingBudget), HardCommitments = a.Sum(x => x.HardCommitments), Requisitions = a.Sum(x => x.Requisitions), TotalCommitted = a.Sum(x => x.TotalCommitted), AccountCategory = a.Key.AccountCategory, AccountCategoryId = a.Key.AccountCategoryId, MainAccountName = a.Key.MainAccountName, MainAccountId = a.Key.MainAccountId, VendorName = a.Key.VendorName, VendorId = a.Key.VendorId, ReferenceCodeName = a.Key.ReferenceCodeName, ReferenceCodeId = a.Key.ReferenceCodeId, ProjectId = a.Key.ProjectId, ProjectName = a.Key.ProjectName, DepartmentName = a.Key.DepartmentName, DepartmentId = a.Key.DepartmentId, LocationId = a.Key.LocationId, LocationName = a.Key.LocationName, Note = a.Key.Note }).ToList();
			List<Forecasting> LineItemTotals = new List<Forecasting>();

			foreach (var item in tempLineItemList)
			{
				Forecasting sf = new Forecasting();
				sf.Actuals = item.Actuals;
				sf.Forecast = item.Forecast;
				sf.WorkingBudget = item.WorkingBudget;
				sf.HardCommitments = item.HardCommitments;
				sf.Requisitions = item.Requisitions;
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
			var model = AllData;
			return Json(model);

		}


		[HttpGet]
		public JsonResult GetForecastLocationSummary()
		{

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
			var security = _context.Security.Where(a => a.Username == User.Identity.Name.ToLower()).Select(a => a.DepartmentId).Distinct().ToList();

			var securityUsers = _context.Security.Where(a => a.Username == User.Identity.Name.ToLower()).Select(a => a.SecurityUsers).ToList();

			int AllowedAccountCategories = securityUsers.Contains("All") ? 100 : 15;

			if (security.Contains("All"))
			{
				departmentSecuritySummary = _context.Departments.Distinct().Select(a => a.DepartmentId).ToList();
				allowedDepartments = _context.Departments.Distinct().ToList().Select(a => a.DepartmentId).ToList();
			}
			else
			{
				departmentSecuritySummary = security;
				allowedDepartments = _context.Departments.Where(a => security.Contains(a.DepartmentId)).Distinct().Select(a => a.DepartmentId).ToList();
			}

			var data = _context.Forecasting.Where(a => departmentSecuritySummary.Contains(a.DepartmentId) && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).ToList();
			var GrandTotal = data.GroupBy(a => a.DepartmentId != "None").Select(a =>
									new
									{
										Actuals = a.Sum(a => a.Actuals),
										Forecast = a.Sum(a => a.Forecast),
										WorkingBudget = a.Sum(a => a.WorkingBudget),
										TotalCommitted = a.Sum(a => a.TotalCommitted),
										HardCommitments = a.Sum(a => a.HardCommitments),
										Requisitions = a.Sum(a => a.Requisitions),
										DepartmentId = "All",
										DepartmentName = "Grand Total",
										LocationName = "",
										LocationId = "",
										ProjectId = "",
										EntryCategory = "GT"
									}
								).FirstOrDefault();
			List<object> AllData = new List<object>();
			var DepartmentTotal = data.GroupBy(a => new { a.DepartmentId, a.DepartmentName }).Select(a =>
									new
									{
										Actuals = a.Sum(a => a.Actuals),
										Forecast = a.Sum(a => a.Forecast),
										WorkingBudget = a.Sum(a => a.WorkingBudget),
										TotalCommitted = a.Sum(a => a.TotalCommitted),
										HardCommitments = a.Sum(a => a.HardCommitments),
										Requisitions = a.Sum(a => a.Requisitions),
										DepartmentId = a.Key.DepartmentId,
										DepartmentName = a.Key.DepartmentName,
										LocationName = "",
										LocationId = "",
										ProjectId = "",
										EntryCategory = "DT"
									}
								).OrderBy(a => a.DepartmentId);
			var ProjectTotal = data.GroupBy(a => new { a.DepartmentId, a.DepartmentName, a.ProjectId, a.ProjectName })
								   .Select(a =>
										new
										{
											Actuals = a.Sum(a => a.Actuals),
											Forecast = a.Sum(a => a.Forecast),
											WorkingBudget = a.Sum(a => a.WorkingBudget),
											TotalCommitted = a.Sum(a => a.TotalCommitted),
											HardCommitments = a.Sum(a => a.HardCommitments),
											Requisitions = a.Sum(a => a.Requisitions),
											DepartmentId = a.Key.DepartmentId,
											DepartmentName = a.Key.DepartmentName,
											LocationName = "",
											LocationId = "",
											ProjectId = a.Key.ProjectId,
											ProjectName = a.Key.ProjectName,
											EntryCategory = "PT"
										}
									).OrderBy(a => a.DepartmentId).ThenBy(a => a.ProjectId);

			var LocationTotal = data.GroupBy(a => new { a.DepartmentId, a.DepartmentName, a.ProjectId, a.ProjectName, a.LocationId, a.LocationName })
									.Select(a =>
										new
										{
											Actuals = a.Sum(a => a.Actuals),
											Forecast = a.Sum(a => a.Forecast),
											WorkingBudget = a.Sum(a => a.WorkingBudget),
											TotalCommitted = a.Sum(a => a.TotalCommitted),
											HardCommitments = a.Sum(a => a.HardCommitments),
											Requisitions = a.Sum(a => a.Requisitions),
											DepartmentId = a.Key.DepartmentId,
											DepartmentName = a.Key.DepartmentName,
											LocationName = a.Key.LocationName,
											LocationId = a.Key.LocationId,
											ProjectId = a.Key.ProjectId,
											ProjectName = a.Key.ProjectName,
											EntryCategory = "LT"
										}
									).OrderBy(a => a.DepartmentId).ThenBy(a => a.ProjectId).ThenBy(a => a.LocationName).ToList();



			// AllData.Insert(0, GrandTotal);
			AllData.Add(GrandTotal);
			foreach (var dept in DepartmentTotal)
			{
				AllData.Add(dept);
				var projects = ProjectTotal.Where(a => a.DepartmentId == dept.DepartmentId).OrderBy(a => a.ProjectId).ToList();
				foreach (var proj in projects)
				{
					AllData.Add(proj);
					var locations = LocationTotal.Where(a => a.DepartmentId == proj.DepartmentId && a.ProjectId == proj.ProjectId).OrderBy(a => a.LocationName).ToList();
					AllData.AddRange(locations);
				}
			}

			return Json(AllData);

		}




		[HttpPost]
		public JsonResult UpdateNextYearForecastItem([FromBody] PostNextYearBudgetItem postnybi)
		{



			string note = postnybi.note;
			var items = postnybi.query.ToString().Split("|");
			var department = items[0];
			var project = items[1];
			var location = items[2];
			var mainaccount = items[3];
			var vendor = items[4];
			var referencecode = items[5];
			string forecast = items[6];

			var updateditem = _context.Forecasting.Where(a => a.DepartmentId == department && a.ProjectId == project && a.LocationId == location && a.MainAccountId == mainaccount && a.VendorId == vendor && a.ReferenceCodeId == referencecode).FirstOrDefault();
			updateditem.Forecast = Convert.ToInt32(forecast);
			updateditem.Note = note;
			updateditem.NoteWriter = User.Identity.Name.ToLower();

			_context.Forecasting.Update(updateditem);
			_context.SaveChanges();




			return Json("success");

		}

		public IActionResult ForecastLocation(string departmentid, string projectid, string locationid, string hasdata)
		{

			ViewBag.OfficeLocation = _context.UserInfo.Where(x => x.FullUserName == User.Identity.Name).Select(x => x.OfficeLocation).FirstOrDefault().ToString();
			// ViewBag.WeatherUrl = _context.WeatherWidgets.Where(a => a.Location == _context.UserInfo.Where(x => x.FullUserName == User.Identity.Name).Select(x => x.OfficeLocation).Distinct().ToString()).Select(a => a.WeatherURL).ToString();
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

			var departmentSummaryData = _context.Forecasting.Where(a => departmentSecuritySummary.Contains(a.DepartmentId) && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).ToList();
			var summarizedDepartmentSummaryData = departmentSummaryData.GroupBy(a => new { a.DepartmentId, a.DepartmentName, a.ProjectId, a.ProjectName }).Select(a => new { Actuals = a.Sum(x => x.Actuals), Forecast = a.Sum(x => x.Forecast), WorkingBudget = a.Sum(x => x.WorkingBudget), Requisitions = a.Sum(x => x.Requisitions), HardCommitments = a.Sum(x => x.HardCommitments), TotalCommitted = a.Sum(x => x.TotalCommitted), Department = a.Key.DepartmentId + " - " + a.Key.DepartmentName, Project = a.Key.ProjectId + " - " + a.Key.ProjectName }).ToList();
			List<Forecasting> DeptSummaryDataList = new List<Forecasting>();

			var deptOnlySummaryTotals = departmentSummaryData.GroupBy(a => new { a.DepartmentId, a.DepartmentName }).Select(a => new { Actuals = a.Sum(x => x.Actuals), Forecast = a.Sum(x => x.Forecast), WorkingBudget = a.Sum(x => x.WorkingBudget), Requisitions = a.Sum(x => x.Requisitions), HardCommitments = a.Sum(x => x.HardCommitments), TotalCommitted = a.Sum(x => x.TotalCommitted), Department = a.Key.DepartmentId + " - " + a.Key.DepartmentName }).ToList();

			foreach (var item in deptOnlySummaryTotals)
			{
				Forecasting deptSumItem = new Forecasting();
				deptSumItem.DepartmentName = item.Department;
				deptSumItem.ProjectName = "00aa";
				deptSumItem.LocationName = "00aa";
				deptSumItem.Actuals = item.Actuals;
				deptSumItem.Forecast = item.Forecast;
				deptSumItem.WorkingBudget = item.WorkingBudget;
				deptSumItem.TotalCommitted = item.TotalCommitted;
				deptSumItem.Requisitions = item.Requisitions;
				deptSumItem.HardCommitments = item.HardCommitments;
				deptSumItem.EntryCategory = "DT";
				deptSumItem.CollapseGroup = "#zzqq" + item.Department.Split(" - ")[0].ToString();
				DeptSummaryDataList.Add(deptSumItem);
			}


			foreach (var item in summarizedDepartmentSummaryData)
			{
				Forecasting deptSumItem = new Forecasting();
				deptSumItem.DepartmentName = item.Department;
				deptSumItem.ProjectName = item.Project;
				deptSumItem.LocationName = "00bb";
				deptSumItem.Actuals = item.Actuals;
				deptSumItem.Forecast = item.Forecast;
				deptSumItem.WorkingBudget = item.WorkingBudget;
				deptSumItem.TotalCommitted = item.TotalCommitted;
				deptSumItem.Requisitions = item.Requisitions;
				deptSumItem.HardCommitments = item.HardCommitments;
				deptSumItem.EntryCategory = "PT";
				deptSumItem.CollapseGroup = "#zzqq" + item.Department.Split(" - ")[0].ToString();



				DeptSummaryDataList.Add(deptSumItem);
			}

			var summarizedDepartmentLocationProjectSummaryData = departmentSummaryData.GroupBy(a => new { a.DepartmentId, a.DepartmentName, a.ProjectId, a.ProjectName, a.LocationName }).Select(a => new { Actuals = a.Sum(x => x.Actuals), Forecast = a.Sum(x => x.Forecast), WorkingBudget = a.Sum(x => x.WorkingBudget), HardCommitments = a.Sum(x => x.HardCommitments), Requisitions = a.Sum(x => x.Requisitions), TotalCommitted = a.Sum(x => x.TotalCommitted), Department = a.Key.DepartmentId + " - " + a.Key.DepartmentName, Project = a.Key.ProjectId + " - " + a.Key.ProjectName, Location = a.Key.LocationName }).ToList();
			foreach (var item in summarizedDepartmentLocationProjectSummaryData)
			{
				Forecasting deptSumItem = new Forecasting();
				deptSumItem.DepartmentName = item.Department;
				deptSumItem.ProjectName = item.Project;
				deptSumItem.LocationName = item.Location;
				deptSumItem.Actuals = item.Actuals;
				deptSumItem.Forecast = item.Forecast;
				deptSumItem.WorkingBudget = item.WorkingBudget;
				deptSumItem.TotalCommitted = item.TotalCommitted;
				deptSumItem.Requisitions = item.Requisitions;
				deptSumItem.HardCommitments = item.HardCommitments;
				deptSumItem.EntryCategory = "LT";
				deptSumItem.CollapseGroup = "#zzqq" + item.Department.Split(" - ")[0].ToString();
				DeptSummaryDataList.Add(deptSumItem);
			}



			var grandTotalSummaryDepartment = departmentSummaryData.GroupBy(a => "Total").Select(a => new { Actuals = a.Sum(x => x.Actuals), Forecast = a.Sum(x => x.Forecast), WorkingBudget = a.Sum(x => x.WorkingBudget), HardCommitments = a.Sum(x => x.HardCommitments), Requisitions = a.Sum(x => x.Requisitions), TotalCommitted = a.Sum(x => x.TotalCommitted) }).ToList();
			foreach (var item in grandTotalSummaryDepartment)
			{
				Forecasting deptSumItem = new Forecasting();
				deptSumItem.DepartmentName = "00aa";
				deptSumItem.ProjectName = "00aa";
				deptSumItem.Actuals = item.Actuals;
				deptSumItem.Forecast = item.Forecast;
				deptSumItem.WorkingBudget = item.WorkingBudget;
				deptSumItem.TotalCommitted = item.TotalCommitted;
				deptSumItem.Requisitions = item.Requisitions;
				deptSumItem.HardCommitments = item.HardCommitments;
				deptSumItem.EntryCategory = "GT";
				DeptSummaryDataList.Add(deptSumItem);
			}


			ViewBag.DepartmentSummaryData = DeptSummaryDataList.OrderBy(a => a.DepartmentName).ThenBy(a => a.ProjectName).ThenBy(a => a.LocationName).ToList();



			if (hasdata != "false")
			{
				List<Forecasting> AllData = new List<Forecasting>();

				List<Forecasting> listForecast = _context.Forecasting.Where(a => a.DepartmentId == departmentid && a.ProjectId == projectid && a.LocationId == locationid && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).ToList();

				Forecasting GrandTotal = new Forecasting();
				GrandTotal.Actuals = listForecast.Sum(a => a.Actuals);
				GrandTotal.Forecast = listForecast.Sum(a => a.Forecast);
				GrandTotal.WorkingBudget = listForecast.Sum(a => a.WorkingBudget);
				GrandTotal.TotalCommitted = listForecast.Sum(a => a.TotalCommitted);
				GrandTotal.HardCommitments = listForecast.Sum(a => a.HardCommitments);
				GrandTotal.Requisitions= listForecast.Sum(a => a.Requisitions);
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




				List<Forecasting> AccountCategoryTotals = new List<Forecasting>();
				var tempList = listForecast.GroupBy(a => new { a.AccountCategoryId, a.AccountCategory }).Select(a => new { Actuals = a.Sum(x => x.Actuals), Forecast = a.Sum(x => x.Forecast), WorkingBudget = a.Sum(x => x.WorkingBudget), TotalCommitted = a.Sum(x => x.TotalCommitted), HardCommitments = a.Sum(x => x.HardCommitments), Requisitions = a.Sum(x => x.Requisitions), AccountCategory = a.Key.AccountCategory, AccountCategoryId = a.Key.AccountCategoryId }).ToList();
				foreach (var item in tempList)
				{
					Forecasting sf = new Forecasting();
					sf.Actuals= item.Actuals;
					sf.Forecast = item.Forecast;
					sf.WorkingBudget = item.WorkingBudget;
					sf.HardCommitments = item.HardCommitments;
					sf.Requisitions = item.Requisitions;
					sf.TotalCommitted = item.TotalCommitted;
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

					AccountCategoryTotals.Add(sf);
				}



				List<Forecasting> MainAccountTotals = new List<Forecasting>();
				listForecast.Select(a => new { a.AccountCategoryId, a.MainAccountId }).Distinct().ToList();


				var tempMainAccountList = listForecast.GroupBy(a => new { a.AccountCategoryId, a.AccountCategory, a.MainAccountId, a.MainAccountName }).Select(a => new { Actuals = a.Sum(x => x.Actuals), Forecast = a.Sum(x => x.Forecast), WorkingBudget = a.Sum(x => x.WorkingBudget), HardCommitments = a.Sum(x => x.HardCommitments), Requisitions = a.Sum(x => x.Requisitions), TotalCommitted = a.Sum(x => x.TotalCommitted), AccountCategory = a.Key.AccountCategory, AccountCategoryId = a.Key.AccountCategoryId, MainAccountName = a.Key.MainAccountName, MainAccountId = a.Key.MainAccountId }).ToList();

				foreach (var item in tempMainAccountList)
				{
					Forecasting sf = new Forecasting();
					sf.Actuals = item.Actuals;
					sf.Forecast = item.Forecast;
					sf.WorkingBudget = item.WorkingBudget;
					sf.HardCommitments = item.HardCommitments;
					sf.Requisitions = item.Requisitions;
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

					MainAccountTotals.Add(sf);
				}



				var tempLineItemList = listForecast.GroupBy(a => new { a.AccountCategoryId, a.AccountCategory, a.MainAccountId, a.MainAccountName, a.ProjectId, a.ProjectName, a.DepartmentId, a.DepartmentName, a.ReferenceCodeId, a.ReferenceCodeName, a.LocationId, a.LocationName, a.VendorId, a.VendorName, a.Note }).Select(a => new { Actuals = a.Sum(x => x.Actuals), Forecast = a.Sum(x => x.Forecast), WorkingBudget = a.Sum(x => x.WorkingBudget), HardCommitments = a.Sum(x => x.HardCommitments), Requisitions = a.Sum(x => x.Requisitions), TotalCommitted = a.Sum(x => x.TotalCommitted), AccountCategory = a.Key.AccountCategory, AccountCategoryId = a.Key.AccountCategoryId, MainAccountName = a.Key.MainAccountName, MainAccountId = a.Key.MainAccountId, VendorName = a.Key.VendorName, VendorId = a.Key.VendorId, ReferenceCodeName = a.Key.ReferenceCodeName, ReferenceCodeId = a.Key.ReferenceCodeId, ProjectId = a.Key.ProjectId, ProjectName = a.Key.ProjectName, DepartmentName = a.Key.DepartmentName, DepartmentId = a.Key.DepartmentId, LocationId = a.Key.LocationId, LocationName = a.Key.LocationName, Note = a.Key.Note }).ToList();
				List<Forecasting> LineItemTotals = new List<Forecasting>();

				foreach (var item in tempLineItemList)
				{
					Forecasting sf = new Forecasting();
					sf.Actuals = item.Actuals;
					sf.Forecast = item.Forecast;
					sf.WorkingBudget = item.WorkingBudget;
					sf.HardCommitments = item.HardCommitments;
					sf.Requisitions = item.Requisitions;
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

				var allowedProjects = _context.Forecasting.Where(a => allowedDepartments.Contains(a.DepartmentId) && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).Select(a => a.ProjectId).Distinct().ToList();

				ViewBag.Projects = _context.Projects.Where(a => allowedProjects.Contains(a.ProjectId)).OrderBy(a => a.ProjectId).ToList();
				ViewBag.DistinctDimensions = JsonConvert.SerializeObject(_context.Forecasting.Select(a => new { a.DepartmentId, a.ProjectId, a.LocationId }).Distinct().ToList()).ToString();
				ViewBag.Locations = _context.Locations.Where(a => allowedRegions.Contains(a.RegionName)).OrderBy(a => a.LocationId).ToList();



				return View(model);
			}
			else
			{



				ViewBag.Vendors = _context.Vendors.Distinct().OrderBy(a => a.VendorName).ToList();
				ViewBag.MainAccounts = _context.MainAccounts.Distinct().OrderBy(a => a.MainAccountId).ToList();
				ViewBag.Department = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Department).FirstOrDefault().ToString();
				ViewBag.ReferenceCode = _context.ReferenceCode.OrderBy(a => a.ReferenceCodeName).ToList();
				var allowedProjects = _context.Forecasting.Where(a => allowedDepartments.Contains(a.DepartmentId) && allowedRegions.Contains(a.RegionName) && Convert.ToInt32(a.AccountCategoryId) < AllowedAccountCategories).Select(a => a.ProjectId).Distinct().ToList();
				ViewBag.Projects = _context.Projects.Where(a => allowedProjects.Contains(a.ProjectId)).OrderBy(a => a.ProjectId).ToList();
				ViewBag.HasData = "false";
				ViewBag.DistinctDimensions = JsonConvert.SerializeObject(_context.Budgeting.Select(a => new { a.DepartmentId, a.ProjectId, a.LocationId }).Distinct().ToList()).ToString();
				ViewBag.Locations = _context.Locations.Where(a => allowedRegions.Contains(a.RegionName)).OrderBy(a => a.LocationId).ToList();
				var model = _context.Forecasting.ToList();
				return View(model);
			}

		}

		[HttpGet]
		public JsonResult PostNewNextYearForecastData(string project, string location, string department, string vendor, string mainaccount, string referencecode)
		{


			bool isAlreadyInTable;


			try
			{
				isAlreadyInTable = _context.Forecasting.Contains(_context.Forecasting.Where(a => a.ProjectId == project && a.LocationId == location.Replace("%20", " ") && a.DepartmentId == department && a.VendorId == vendor && a.MainAccountId == mainaccount && a.ReferenceCodeId == referencecode).FirstOrDefault());

			}
			catch (Exception)
			{

				isAlreadyInTable = false;
			}

			if (!isAlreadyInTable)
			{

				Forecasting item = new Forecasting();
				item.ProjectId = project;
				item.ProjectName = _context.Projects.Where(a => a.ProjectId == project).Select(a => a.ProjectName).First().ToString();
				item.LocationId = location;
				item.ReferenceCodeId = referencecode;
				item.Note = "...";

				if (referencecode != "None")
				{
					item.ReferenceCodeName = referencecode;
				}
				else
				{
					item.ReferenceCodeName = "None";
				}


				
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

				_context.Forecasting.Add(item);
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
			else
			{
				return Json("failure");
			}
		}

		[HttpGet]
		public JsonResult DeleteForecastItem(string deleteid)
		{
			var array = deleteid.Split('|');
		    string departmentid = array[0].ToString();
			string projectid = array[1].ToString();
			string locationid = array[2].ToString();
			string mainaccountid = array[3].ToString();
			string vendorid = array[4].ToString();
			string referencecodeid = array[5].ToString();

			var deleteItem = _context.Forecasting.Where(a => a.DepartmentId == departmentid && a.ProjectId == projectid && a.LocationId == locationid && a.MainAccountId == mainaccountid && a.VendorId == vendorid && a.ReferenceCodeId == referencecodeid).FirstOrDefault();
			_context.Forecasting.Remove(deleteItem);
			_context.SaveChanges();
			return Json("OK");
		}

	}
}
