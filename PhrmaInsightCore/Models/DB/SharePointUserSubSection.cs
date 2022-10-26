using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class SharePointUserSubSection
    {
        public int Id { get; set; }        
        public string Section { get; set; }
        public string SubSection { get; set; }
        public string Template { get; set; }

    }
}
