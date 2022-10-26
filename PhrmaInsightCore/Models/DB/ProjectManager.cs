using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class ProjectManager
    {
        public int Id { get; set; }
        public string ProjectManagerId { get; set; }
        public string ProjectManagerName { get; set; }
      
    }
}
