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
    public class Test : Controller
    {
        DatabaseContext _context;

        public Test(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {

            return View();
        }
        public IActionResult CRM()
        {
            return View();
        }
    }
}