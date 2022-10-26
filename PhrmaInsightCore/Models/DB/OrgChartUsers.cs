using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class OrgChartUsers
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string MobilePhone { get; set; }
        public string OfficePhone { get; set; }
        public string OfficeLocation { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }
        public string Photo { get; set; }
        public string ManagerEmail { get; set; }
    }
}
