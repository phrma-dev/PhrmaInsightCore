using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class SharePointUserActivity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string URL { get; set; }
        public string Section { get; set; }
        public string SubSection { get; set; }
        public string Company { get; set; }
        public string Site { get; set; }
        public string Department { get; set; }
        public string IsExternal { get; set; }
        public DateTime ActivityTime { get => DateTime.UtcNow.Subtract(TimeSpan.FromHours(5)); set => value = DateTime.UtcNow.Subtract(TimeSpan.FromHours(5)); }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Day { get; set; }
        public string Hour { get; set; }
        public string Date { get; set; }
    }
}
