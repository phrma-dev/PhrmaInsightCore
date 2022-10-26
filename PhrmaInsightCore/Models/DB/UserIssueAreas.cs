using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class UserIssueAreas
    {
        public int Id { get; set; }
        public string UserEmail{ get; set; }
        public string IssueArea { get; set; }

      
    }
}
