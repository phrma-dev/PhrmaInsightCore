using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class ITActuals
    {
        public int Id { get; set; }
        public decimal Actuals { get; set; }
        public decimal Budget { get; set; }
      
    }
}
