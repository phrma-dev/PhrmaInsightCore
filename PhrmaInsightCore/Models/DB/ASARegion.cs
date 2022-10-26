using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class ASARegion
    {
        public int Id { get; set; }
        public string ASA_REGION { get; set; }
        public string LOCATIONID { get; set; }

    }
}
