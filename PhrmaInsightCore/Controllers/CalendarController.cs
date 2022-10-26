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
	[Authorize]
	public class CalendarController : Controller
	{

		DatabaseContext _context;

		public CalendarController(DatabaseContext context)
		{
			_context = context;

		}

		public IActionResult MyCalendar() 
		{
			return View();
		}
		

	}
}
