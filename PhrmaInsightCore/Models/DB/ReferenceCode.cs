using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class ReferenceCode
    { 
        public int Id { get; set; }
        public string ReferenceCodeId { get; set; }
        public string ReferenceCodeName { get; set; }
      
    }
}
