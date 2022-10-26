using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class BoardMember
    {
        public int Id { get; set; }
        public string BoardMemberName { get; set; }
        public string Organization { get; set; }
        public string BoardMemberImage { get; set; }
     
        
    }
}
