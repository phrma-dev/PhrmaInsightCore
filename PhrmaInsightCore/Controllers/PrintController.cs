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

namespace PhrmaInsightCore.Controllers
{

    [Authorize]
    public class PrintController : Controller
    {
        DatabaseContext _context;

        public PrintController(DatabaseContext context)
        {
            _context = context;

        }



        [HttpPost]
        [AllowAnonymous]
        public string SPUserActivity([FromBody] SharePointUserActivity sharePointUserActivity)
        {
            var activity = new SharePointUserActivity();
            activity.Email = sharePointUserActivity.Email;
            activity.URL = sharePointUserActivity.URL;
            var fed = activity.URL.IndexOf("Communities/fedsection") != -1;
            var sra = activity.URL.IndexOf("Communities/SRAKITs") != -1;
            var sad = activity.URL.IndexOf("Communities/State") != -1;
            var paf = activity.URL.IndexOf("Communities/PA") != -1;
            var tlk = activity.URL.IndexOf("toolkitssite") != -1;
            var stf = activity.URL.IndexOf("StaffDirectory") != -1;
            var cpk = activity.URL.IndexOf("memberresources/ChartPacks") != -1;
            var mkt = activity.URL.IndexOf("Communities/MarketAccess") != -1;
            var sfr = activity.URL.IndexOf("Communities/SRAKITs/FederalRegister") != -1;
            var site = activity.URL.IndexOf("sites/inside/") != -1 ? "Inside" : activity.URL.IndexOf("sites/connect/") != -1 ? "Connect" : "Not Assigned";
            activity.Site = site;

            
            
            
            var section = fed ? "Federal Advocacy" : sfr ? "SRA Federal Register" : sra ? "SRA Work Groups" : sad ? "State Advocacy" : paf ? "Public Affairs" : tlk ? "Toolkits" : stf ? "Staff Directory" : cpk ? "Chart Packs" : mkt ? "Market Access Platform" : "Not Assigned";
            var company = GetCompany(sharePointUserActivity.Email.ToString());
            activity.Company = company;
            activity.IsExternal = company != "PhRMA" ? "true" : "false";
            activity.Section = section;
            activity.Department = company == "PhRMA" ? _context.AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME.ToLower() == sharePointUserActivity.Email.ToLower()).Select(a => a.DEPARTMENT).FirstOrDefault() : "External";
            DateTime dateTime = DateTime.UtcNow.Subtract(TimeSpan.FromHours(5));
            activity.Year = dateTime.Year.ToString();
            activity.Month = dateTime.Month.ToString();
            activity.Day = dateTime.Day.ToString();
            activity.Hour = dateTime.Hour.ToString();
            activity.Date = dateTime.ToString().Substring(0, 10);
            if (sharePointUserActivity.URL.Contains("payload|"))
            {
                var count = _context.SharePointUserActivity.Where(a => a.Email == sharePointUserActivity.Email && a.ActivityTime.Date.Day == sharePointUserActivity.ActivityTime.Day && a.ActivityTime.Date.Month == sharePointUserActivity.ActivityTime.Month && a.ActivityTime.Date.Year == sharePointUserActivity.ActivityTime.Year && a.ActivityTime.Date.Hour == sharePointUserActivity.ActivityTime.Hour).ToList().Count();
                if (count == 0)
                {
                    _context.SharePointUserActivity.Add(activity);
                    _context.SaveChanges();
                }

            }
            else
            {
                _context.SharePointUserActivity.Add(activity);
                _context.SaveChanges();
            }
            
            


            return "ok";
        }


        [HttpGet]
        public async Task<string> Test()
        {
            var url = "https://api.alertmedia.com/api/users";
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("Authorization", "Basic OTBkZTBmNjMtYjdhOS00MDU3LTgzMDAtMGNhYTQ0M2VjMzVmOmIwNjQ5YzNmN2JlMGY4NGFhODI0N2ZmN2Q2OGVlMTdm");
            var response = await client.GetAsync(url);
            var data = response.Content.ReadAsStringAsync();
            var json = JArray.Parse(data.Result);

            var userlist = _context.AzureADUsers.Select(a => a.USER_PRINCIPAL_NAME).ToList();
            string stringreturn = "";

            foreach (var item in json)
            {
                stringreturn += item["email"] + " - ";
            }

            return stringreturn;
        }

        [HttpGet]
        public string GetCompany(string email) 
        {
            List<string> companies = new List<string>();
            companies.Add("Abbvie|_abbvie.com");
            companies.Add("Allergan|_allergan.com");
            companies.Add("Amgen|_amgen.com");
            companies.Add("Astellas|_astellas.com");
            companies.Add("Astra Zeneca|_astrazeneca.com");
            companies.Add("Bayer|_bayer.com");
            companies.Add("Biogen|_biogen.com");
            companies.Add("BMRN|_bmrn.com");
            companies.Add("BMS|_bms.com");
            companies.Add("Boehringer-Ingelheim|_boehringer-ingelheim.com");
            companies.Add("Daiichi Sankyo|_daiichisankyo.co.jp");
            companies.Add("DSI|_dsi.com");
            companies.Add("EMD Serono|_emdserono.com");
            companies.Add("Eisai|_eisai.com");
            companies.Add("Gene|_gene.com");
            companies.Add("Gilead|_gilead.com");
            companies.Add("GSK|_gsk.com");
            companies.Add("Incyte|_incyte.com");
            companies.Add("Ipsen|_ipsen.com");
            companies.Add("Johnson and Johnson|_its.jnj.com");
            companies.Add("Lilly|_lilly.com");
            companies.Add("Lundbeck|_lundbeck.com");
            companies.Add("Merck|_merck.com");
            companies.Add("Novartis|_novartis.com");
            companies.Add("NovoNordisk|_novonordisk.com");
            companies.Add("Otsuka|_otsuka-us");
            companies.Add("Pfizer|_pfizer.com");
            companies.Add("PhRMA|phrma.org");
            companies.Add("PhRMA|powerbiprodsvc@phrma.onmicrosoft.com");
            companies.Add("Sanofi|_sanofi.com");
            companies.Add("Sunovion|_sunovion.com");
            companies.Add("Takeda|_takeda.com");
            companies.Add("UCB|_ucb.com");

            string returnCompany = "Not Assigned";
            foreach (var company in companies)
            {
                if (email.ToLower().Contains(company.Split("|")[1].ToString()) == true)
                {
                    returnCompany = company.Split("|")[0].ToString();
                }
            }

            return returnCompany;
        }

        public IActionResult Org() 
        {

            return View();
        }

        public IActionResult SharePointOrg()
        {

            return View();
        }
        public class AzureADUsers_IssueAreas
        {
         public int Id { get; set; }
            public string USER_PRINCIPAL_NAME { get; set; }
            public string GIVEN_NAME { get; set; }
            public string SURNAME { get; set; }
            public string MOBILE_PHONE { get; set; }
            public string OFFICE_LOCATION { get; set; }
            public string DEPARTMENT { get; set; }
            public string TITLE { get; set; }
            public string alertmediaid { get; set; }
            public string AZURE_AD_MANAGER { get; set; }
            public string issueareas { get; set; }

        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetUsers(string employee) 
        {
            try
            {
                HttpClient client = new HttpClient();
                var response = await client.GetAsync("https://prod-54.eastus.logic.azure.com:443/workflows/6932ffb74b784735a6f3114db26ebba0/triggers/manual/paths/invoke?api-version=2016-10-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=F6atxo7wclHe5yqPR-zo4tnZGZN--KvMofRxvREUwaU");
                var result = response.Content.ReadAsStringAsync();
                Newtonsoft.Json.Linq.JObject data = JObject.Parse(result.Result.ToString());
                var employees = data["value"].OrderBy(a => a["surname"]).ToList();
                List<AzureADUsers> userList = new List<AzureADUsers>();
                userList =  _context.AzureADUsers.ToList();
                List<AzureADUsers_IssueAreas> users = new List<AzureADUsers_IssueAreas>();
            
                for(var q = 0; q < employees.Count(); q++)
                {
                    AzureADUsers_IssueAreas user = new AzureADUsers_IssueAreas();
                    try 
	                {	        
		               var user_tmp = _context.AzureADUsers.Where(a => a.USER_PRINCIPAL_NAME == employees[q]["userPrincipalName"].ToString().ToLower()).FirstOrDefault();
                       user = new AzureADUsers_IssueAreas()
                        {
                            Id = user_tmp.Id,
                            USER_PRINCIPAL_NAME = user_tmp.USER_PRINCIPAL_NAME,
                            GIVEN_NAME = user_tmp.GIVEN_NAME,
                            SURNAME = user_tmp.SURNAME,
                            MOBILE_PHONE = user_tmp.MOBILE_PHONE,
                            OFFICE_LOCATION = user_tmp.OFFICE_LOCATION,
                            DEPARTMENT = user_tmp.DEPARTMENT,
                            TITLE = user_tmp.TITLE,
                            AZURE_AD_MANAGER = user_tmp.AZURE_AD_MANAGER

                        };

                        var issues = _context.UserIssueAreas.ToList();
                    
                        foreach (var item in issues.Where(a => a.UserEmail == user.USER_PRINCIPAL_NAME))
                        {
                            user.issueareas += "|" + item.IssueArea;
                        }
                        //var count = _context.UserInfo.Where(a => a.FullUserName == employees[q]["userPrincipalName"].ToString().ToLower()).Count();
                        //if (count > 0)
                        //{
                        //    user.alertmediaid = _context.UserInfo.Where(a => a.FullUserName == employees[q]["userPrincipalName"].ToString().ToLower()).Select(a => a.Image).FirstOrDefault().ToString();
                        //}
                        //else {
                        //    user.alertmediaid = "../../img/nophoto.png";
                        //}
                        try
                        {
                            user.alertmediaid = _context.UserInfo.Where(a => a.FullUserName == employees[q]["userPrincipalName"].ToString().ToLower()).Select(a => a.Image).FirstOrDefault().ToString();
                        }
                        catch (Exception)
                        {
                            user.alertmediaid = "../../img/nophoto.png";
                        }

                        users.Add(user);

                   
	                }

	                catch (Exception)
	                {


	                }
             
                }
          
                return Json(users);
            }
            catch (Exception)
            {
                return Json("Error");
               
            }


        }





        [HttpGet]
        public FileResult Users()
        {
            byte[] fileContents;


      

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Sheet1");

                var data = _context.AzureADUsers.ToList();
               
               
                var k = 0;
                var userPhotos = _context.UserInfo.ToList();
                //for (int i = 0; i < data.Count(); i++)
                for (int i = 0; i < 20; i++)
                    {
                    var photo = "";
                    try
                    {
                        var count = userPhotos.Where(a => a.FullUserName == data[i].USER_PRINCIPAL_NAME).Select(a => a.Image).Count();
                        if (count == 0)
                        {
                            photo = "http://phrma.azurewebsites.net/img/phrmatoplogo.jpg";
                            WebRequest req1 = WebRequest.Create(photo);
                            WebResponse response1 = req1.GetResponse();
                            Stream stream1 = response1.GetResponseStream();
                            Image img = Image.FromStream(stream1);
                            stream1.Close();
                            var name = "img" + i;
                            var pic1 = worksheet.Drawings.AddPicture(name, img);
                            var row = (150 * i) + 130;
                            pic1.SetPosition(row, 10);
                        } else { 

                            photo = userPhotos.Where(a => a.FullUserName == data[i].USER_PRINCIPAL_NAME).Select(a => a.Image).FirstOrDefault().ToString();

                            var base64Data = Regex.Match(photo, @"data:image/(?<type>.+?),(?<data>.+)").Groups["data"].Value;
                            var binData = Convert.FromBase64String(base64Data);

                            using (var strm = new MemoryStream(binData))
                            {
                                Image img1 = Image.FromStream(strm);
                                var row1 = (150 * i) + 130;
                                var name2 = "img" + i;
                                var pic2 = worksheet.Drawings.AddPicture(name2, img1);
                                pic2.SetPosition(row1, 10);
                                strm.Close();
                            }

                        }                   
                    }
                    catch (Exception)
                    {

                        photo = "http://phrma.azurewebsites.net/img/phrmatoplogo.jpg";
                        WebRequest req1 = WebRequest.Create(photo);
                        WebResponse response1 = req1.GetResponse();
                        Stream stream1 = response1.GetResponseStream();
                        Image img = Image.FromStream(stream1);
                        stream1.Close();
                        var name = "img" + i;
                        var pic1 = worksheet.Drawings.AddPicture(name, img);
                        var row = (150 * i) + 130;
                        pic1.SetPosition(row, 10);
                    }

                    //photo = "http://phrma.azurewebsites.net/img/phrmatoplogo.jpg";
                    //WebRequest req1 = WebRequest.Create(photo);
                    //WebResponse response1 = req1.GetResponse();
                    //Stream stream1 = response1.GetResponseStream();
                    //Image img = Image.FromStream(stream1);
                    //stream1.Close();
                    //var name = "img" + i;
                    //var pic1 = worksheet.Drawings.AddPicture(name, img);
                    //var row = (150 * (i + 1));
                    //pic1.SetPosition(row, 10);

                    worksheet.Cells[6 + k, 5].Value = "Name";
                    worksheet.Cells[6 + k, 6].Value = data[i].GIVEN_NAME + " " + data[i].SURNAME;
                    worksheet.Cells[6 + k + 1, 5].Value = "Department";
                    worksheet.Cells[6 + k + 1, 6].Value = data[i].DEPARTMENT;
                    worksheet.Cells[6 + k + 2, 5].Value = "Office";
                    worksheet.Cells[6 + k + 2, 6].Value = data[i].OFFICE_LOCATION;
                    worksheet.Cells[6 + k + 3, 5].Value = "Title";
                    worksheet.Cells[6 + k + 3, 6].Value = data[i].TITLE;
                    worksheet.Cells[6 + k + 4, 5].Value = "Email";
                    worksheet.Cells[6 + k + 4, 6].Value = data[i].USER_PRINCIPAL_NAME;
                    k += 7;
                }

                //for (int i = 0; i < data.Count(); i++)
                //{
                //    for (int k = 0; k < data[i].GetType().GetProperties().Count(); k++)
                //    {

                //        var q = data[i].GetType().GetProperties()[k].Name;
                //        var val = data[i].GetType().GetProperty(q).GetValue(data[i], null);
                //        worksheet.Cells[i + 6, k + 2].Value = val;
                //        if (i % 2 == 0)
                //        {
                //            worksheet.Cells[i + 6, k + 2].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //            worksheet.Cells[i + 6, k + 2].Style.Fill.BackgroundColor.SetColor(100, 236, 244, 250);

                //        }
                //        if (k + 2 > 8 && k + 2 < 13)
                //        {
                //            worksheet.Cells[i + 6, k + 2].Style.Numberformat.Format = "#,##";

                //        }


                //    }

                //}


                worksheet.View.ShowGridLines = false;
                worksheet.PrinterSettings.RepeatRows = new ExcelAddress("1:5");
                worksheet.PrinterSettings.PrintArea = worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column];
                worksheet.PrinterSettings.PaperSize = ePaperSize.Letter;



                worksheet.PrinterSettings.Orientation = eOrientation.Portrait;
                worksheet.PrinterSettings.TopMargin = 0.3M;
                worksheet.PrinterSettings.BottomMargin = 0.3M;
                worksheet.PrinterSettings.LeftMargin = 0.3M;
                worksheet.PrinterSettings.RightMargin = 0.3M;
                worksheet.PrinterSettings.HeaderMargin = 0.0M;
                worksheet.PrinterSettings.FooterMargin = 0.2M;
                worksheet.PrinterSettings.FitToHeight = 0;
                worksheet.PrinterSettings.FitToWidth = 1;
                var allCells = worksheet.Cells[1, 1, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column];
                var cellFont = allCells.Style.Font;

                cellFont.SetFromFont(new Font("Segoe UI", 9));
                // worksheet.View.FreezePanes(6, 16384);
                ExcelHeaderFooterText footerEvenPage = worksheet.HeaderFooter.EvenFooter;
                ExcelHeaderFooterText footerOddPage = worksheet.HeaderFooter.OddFooter;
                footerEvenPage.RightAlignedText = "&K696969" + ExcelHeaderFooter.PageNumber + " / " + ExcelHeaderFooter.NumberOfPages;
                footerOddPage.RightAlignedText = "&K696969" + ExcelHeaderFooter.PageNumber + " / " + ExcelHeaderFooter.NumberOfPages;



                for (int i = worksheet.Dimension.End.Column + 2; i < 16385; i++)
                {
                    worksheet.Column(i).Hidden = true;
                }

                for (int i = 1; i <= worksheet.Dimension.End.Column; i++)
                {
                    worksheet.Column(i).AutoFit();
                    worksheet.Column(i).PageBreak = false;
                }

                worksheet.Column(worksheet.Dimension.End.Column).PageBreak = true;


                worksheet.Column(4).Width = worksheet.Column(2).Width;
               //worksheet.Column(12).Width = worksheet.Column(3).Width;
               // worksheet.Column(7).Width = 35;
                worksheet.Column(1).Width = 2.5;


                //using (ExcelRange autoFilterCells = worksheet.Cells[5, 2, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column])
                //{
                //    autoFilterCells.AutoFilter = true;
                //}
                //worksheet.PrinterSettings.Scale = 53;
                //var wsPvt = package.Workbook.Worksheets.Add("Pivot");
                //var dataRange = worksheet.Cells[5, 2, worksheet.Dimension.End.Row, worksheet.Dimension.End.Column];
                //var pivotTable = wsPvt.PivotTables.Add(wsPvt.Cells["B5"], dataRange, "Pivot");

                //label field
                //pivotTable.RowFields.Add(pivotTable.Fields[data[0].GetType().GetProperties()[0].Name]);
                //pivotTable.DataOnRows = false;

                //pivotTable.RowFields.Add(pivotTable.Fields[data[0].GetType().GetProperties()[1].Name]);
                //pivotTable.DataOnRows = false;
                //pivotTable.RowFields[0].Outline = true;
                //pivotTable.RowFields[0].SubTotalFunctions = eSubTotalFunctions.Sum;
                //pivotTable.RowFields[0].ShowAll = true;
                //pivotTable.RowFields[0].SubtotalTop = true;

                //pivotTable.RowFields.Add(pivotTable.Fields[data[0].GetType().GetProperties()[4].Name]);
                //pivotTable.DataOnRows = false;
                //pivotTable.RowFields[1].Outline = false;
                //pivotTable.RowFields[1].SubTotalFunctions = eSubTotalFunctions.Sum;
                //pivotTable.RowFields[1].ShowAll = false;
                //pivotTable.RowFields[1].SubtotalTop = false;

                //pivotTable.RowFields.Add(pivotTable.Fields[data[0].GetType().GetProperties()[2].Name]);
                //pivotTable.DataOnRows = false;
                //pivotTable.RowFields[2].Outline = true;
                //pivotTable.RowFields[2].SubTotalFunctions = eSubTotalFunctions.Sum;
                //pivotTable.RowFields[2].ShowAll = true;
                //pivotTable.RowFields[2].SubtotalTop = true;

                //pivotTable.RowFields.Add(pivotTable.Fields[data[0].GetType().GetProperties()[5].Name]);
                //pivotTable.DataOnRows = false;
                //pivotTable.RowFields[3].Outline = true;
                //pivotTable.RowFields[3].SubTotalFunctions = eSubTotalFunctions.Sum;
                //pivotTable.RowFields[3].ShowAll = true;
                //pivotTable.RowFields[3].SubtotalTop = true;

                //pivotTable.RowFields.Add(pivotTable.Fields[data[0].GetType().GetProperties()[6].Name]);
                //pivotTable.DataOnRows = false;
                //pivotTable.RowFields[4].Outline = false;
                //pivotTable.RowFields[4].SubTotalFunctions = eSubTotalFunctions.None;
                //pivotTable.RowFields[4].ShowAll = false;
                //pivotTable.RowFields[4].SubtotalTop = false;



                //pivotTable.RowFields.Add(pivotTable.Fields[data[0].GetType().GetProperties()[7].Name]);
                //pivotTable.DataOnRows = false;
                //pivotTable.RowFields[5].Compact = false;
                //pivotTable.RowFields[5].Outline = false;
                //pivotTable.RowFields[5].SubTotalFunctions = eSubTotalFunctions.None;
                //pivotTable.RowFields[5].ShowAll = false;
                //pivotTable.RowFields[5].SubtotalTop = false;

                //pivotTable.RowFields.Add(pivotTable.Fields[data[0].GetType().GetProperties()[15].Name]);
                //pivotTable.DataOnRows = false;
                //pivotTable.RowFields[6].Compact = false;
                //pivotTable.RowFields[6].Outline = false;
                //pivotTable.RowFields[6].SubTotalFunctions = eSubTotalFunctions.None;
                //pivotTable.RowFields[6].Outline = false;
                //pivotTable.RowFields[6].Compact = false;
                //pivotTable.RowFields[6].ShowAll = false;
                //pivotTable.RowFields[6].SubtotalTop = false;

                //pivotTable.RowFields.Add(pivotTable.Fields[data[0].GetType().GetProperties()[16].Name]);
                //pivotTable.DataOnRows = false;
                //pivotTable.RowFields[7].Compact = false;
                //pivotTable.RowFields[7].Outline = false;
                //pivotTable.RowFields[7].SubTotalFunctions = eSubTotalFunctions.None;
                //pivotTable.RowFields[7].Outline = false;
                //pivotTable.RowFields[7].Compact = false;
                //pivotTable.RowFields[7].ShowAll = false;
                //pivotTable.RowFields[7].SubtotalTop = false;



                //pivotTable.UseAutoFormatting = true;
                //pivotTable.CompactData = false;
                //pivotTable.Indent = 0;
                //pivotTable.ShowMemberPropertyTips = false;
                //pivotTable.DataOnRows = false;




                //var field = pivotTable.DataFields.Add(pivotTable.Fields[data[0].GetType().GetProperties()[9].Name]);
                //field.Name = "Working Budget";
                //field.Function = DataFieldFunctions.Sum;
                //field.Format = "#,##";
                //field.Field.Compact = false;
                //field.Field.SubTotalFunctions = eSubTotalFunctions.None;

                //field = pivotTable.DataFields.Add(pivotTable.Fields[data[0].GetType().GetProperties()[10].Name]);
                //field.Name = "Actuals";
                //field.Function = DataFieldFunctions.Sum;
                //field.Format = "#,##";
                //field.Field.Compact = false;
                //field.Field.SubTotalFunctions = eSubTotalFunctions.None;

                //field = pivotTable.DataFields.Add(pivotTable.Fields[data[0].GetType().GetProperties()[11].Name]);
                //field.Name = "PO Balance";
                //field.Function = DataFieldFunctions.Sum;
                //field.Format = "#,##";
                //field.Field.Compact = false;
                //field.Field.SubTotalFunctions = eSubTotalFunctions.None;

                //field = pivotTable.DataFields.Add(pivotTable.Fields[data[0].GetType().GetProperties()[12].Name]);
                //field.Name = "Requistions";
                //field.Function = DataFieldFunctions.Sum;
                //field.Format = "#,##";
                //field.Field.Compact = false;
                //field.Field.SubTotalFunctions = eSubTotalFunctions.None;

                //field = pivotTable.DataFields.Add(pivotTable.Fields[data[0].GetType().GetProperties()[13].Name]);
                //field.Name = "Total Committed";
                //field.Function = DataFieldFunctions.Sum;
                //field.Format = "#,##";
                //field.Field.Compact = false;
                //field.Field.SubTotalFunctions = eSubTotalFunctions.None;

                //field = pivotTable.DataFields.Add(pivotTable.Fields[data[0].GetType().GetProperties()[14].Name]);
                //field.Name = "Forecast";
                //field.Function = DataFieldFunctions.Sum;
                //field.Format = "#,##";
                //field.Field.Compact = false;
                //field.Field.SubTotalFunctions = eSubTotalFunctions.None;


                WebRequest req = WebRequest.Create("http://phrma.azurewebsites.net/img/phrmatoplogo.jpg");
                WebResponse response = req.GetResponse();
                Stream stream = response.GetResponseStream();


                Image logo = Image.FromStream(stream);
                stream.Close();
                var picture = worksheet.Drawings.AddPicture("logo", logo);
                //var pic2 = wsPvt.Drawings.AddPicture("logo", logo);
                //picture.SetPosition(0, 0);
                //pic2.SetPosition(0, 0);





                //for (int i = 1; i <= 24; i++)
                //{
                //    wsPvt.Column(i).AutoFit();
                //}

                //wsPvt.PrinterSettings.Orientation = eOrientation.Landscape;
                //wsPvt.PrinterSettings.TopMargin = 0.3M;
                //wsPvt.PrinterSettings.BottomMargin = 0.3M;
                //wsPvt.PrinterSettings.LeftMargin = 0.3M;
                //wsPvt.PrinterSettings.RightMargin = 0.3M;
                //wsPvt.PrinterSettings.HeaderMargin = 0.0M;
                //wsPvt.PrinterSettings.FooterMargin = 0.2M;

                //wsPvt.View.FreezePanes(7, 16384);
                //ExcelHeaderFooterText footerEvenPage2 = wsPvt.HeaderFooter.EvenFooter;
                //ExcelHeaderFooterText footerOddPage2 = wsPvt.HeaderFooter.OddFooter;
                //footerEvenPage2.RightAlignedText = "&K696969" + ExcelHeaderFooter.PageNumber + " / " + ExcelHeaderFooter.NumberOfPages;
                //footerOddPage2.RightAlignedText = "&K696969" + ExcelHeaderFooter.PageNumber + " / " + ExcelHeaderFooter.NumberOfPages;

                //wsPvt.View.ShowGridLines = false;

                //wsPvt.PrinterSettings.RepeatRows = new ExcelAddress("1:6");
                //wsPvt.Cells.Style.Font.Size = 9;
                //wsPvt.Cells.Style.Font.SetFromFont(new Font("Segoe UI", 9));


                // Put whatever you want here in the sheet
                // For example, for cell on row1 col1
                //worksheet.Cells[1, 1].Value = "Long text";
                //worksheet.Cells[1, 1].Style.Font.Size = 12;
                //worksheet.Cells[1, 1].Style.Font.Bold = true;
                //worksheet.Cells[1, 1].Style.Fill.PatternType = ExcelFillStyle.Solid;
                //worksheet.Cells[1, 1].Style.Fill.BackgroundColor.SetColor(1, 123, 202, 202);
                //worksheet.Cells[1, 1].Style.Border.Top.Style = ExcelBorderStyle.Hair;

                // So many things you can try but you got the idea.

                // Finally when you're done, export it to byte array.
                fileContents = package.GetAsByteArray();
            }

            if (fileContents == null || fileContents.Length == 0)
            {
                // return NotFound();
            }


            return File(
                fileContents: fileContents,
                contentType: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                fileDownloadName: "Requisition_Detail.xlsx"
            );
        }
    }
}