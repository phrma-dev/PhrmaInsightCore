using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class MemberNote
    {
        public int Id { get; set; }
        public string MemberId { get; set; }
        public string Note { get; set; }
        public string User { get; set; }
      
    }
}
