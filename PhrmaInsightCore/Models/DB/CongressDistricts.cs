using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class CongressDistricts
    {
        public int Id { get; set; }
        public string StateId { get; set; }
        public string StateCode{ get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string RepresentativeName { get; set; }
        public string RepresentativeImage { get; set; }
        public string Party { get; set; }
        public string OfficeRoom { get; set; }
        public string Phone { get; set; }

    }
}
