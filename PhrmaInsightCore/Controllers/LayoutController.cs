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
    public class LayoutController : Controller
    {
        private readonly ILogger<LayoutController> _logger;
        DatabaseContext _context;

        public LayoutController(ILogger<LayoutController> logger, DatabaseContext context)
        {
            _logger = logger;
            _context = context;
        }


        [HttpGet]
        public string UserPhoto() 
        {
            var photo = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name.ToLower()).Select(a => a.Image).First().ToString();
            return photo;
        }

        [HttpGet]
        public string UserName()
        {
            var user_name = _context.UserInfo.Where(a => a.FullUserName == User.Identity.Name).Select(a => a.Name).FirstOrDefault().ToString();
            return user_name;
        }

    }
}
