using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class SharePointUserCompany
    {
        public int Id { get; set; }
        public string EmailDomain { get; set; }
        public string Company { get; set; }

      
    }
}
