using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class Locations
    {
        public int Id { get; set; }
        public string LocationId { get; set; }
        public string LocationName { get; set; }
        public string RegionName { get; set; }
      
    }
}
