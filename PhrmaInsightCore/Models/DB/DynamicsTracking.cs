using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class DynamicsTracking
    {
        public int Id { get; set; }
        public int Priority { get; set; }
        public string Category { get; set; }
        public string Task { get; set; }
        public string PhrmaLead { get; set; }
        public string RSMLead { get; set; }
        public DateTime EstimatedCompletionDate { get; set; }
        public DateTime CompletionDate { get; set; }
        public string Notes { get; set; }
      
    }
}
