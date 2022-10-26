using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class Vendors
    {
        public int Id { get; set; }
        public string VendorId { get; set; }
        public string VendorName { get; set; }
      
    }
}
