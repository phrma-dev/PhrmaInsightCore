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
    public class SoftCommitmentForecast
    {
        public int Id { get; set; }
        public string DepartmentId { get; set; }
        public string MainAccountId { get; set; }
        public string ProjectId { get; set; }
        public string EntryCategory { get; set; }
        public string LocationId { get; set; }
        public string VendorId { get; set; }
        public string Year { get; set; }
        public string LastModifiedByUser { get; set; }
        // public string LastModifiedByUserImage { get; set; }
        public string LastModifiedDateTime { get; set; }
        public decimal WorkingBudget { get; set; }
        public decimal Actuals { get; set; }
        public decimal NextYearBudget { get; set; }
        public decimal ForecastBudget { get; set; }
        public decimal HardCommitments { get; set; }
        public decimal Requisitions { get; set; }
        public decimal TotalCommitted { get; set; }
        public string MonthId { get; set; }
        public string VendorName { get; set; }
        public string MainAccountName { get; set; }
        public string AccountCategory { get; set; }
        public string AccountCategoryId { get; set; }
        public string ProjectName { get; set; }
        public string DepartmentName { get; set; }
        public string LocationName { get; set; }
        public string RegionName { get; set; }
        public string ReferenceCodeId { get; set; }
        public string ReferenceCodeName { get; set; }
        public string ArrowIndicator { get; set; }
        public string CollapseGroup { get; set; }
        public string Note { get; set; }
        public string NoteWriter { get; set; }
        
    }
}
