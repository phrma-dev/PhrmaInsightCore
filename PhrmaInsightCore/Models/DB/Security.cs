using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace PhrmaInsightCore.Models.DB
{
    public class Security
    {
        public int Id { get; set; }
        public string DepartmentId { get; set; }
        public string Username { get; set; }
        public string SecurityUsers { get; set; }
        public string Region { get; set; }
        public string DefaultUserView { get; set; }
        public string UserGUID { get; set; }

    }
}
