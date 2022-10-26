using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class Departments
    {
        public int Id { get; set; }
        public string DepartmentId { get; set; }
        public string DepartmentName { get; set; }
      
    }
}
