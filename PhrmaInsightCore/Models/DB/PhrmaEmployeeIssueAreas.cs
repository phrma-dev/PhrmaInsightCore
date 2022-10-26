using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class PhrmaEmployeeIssueAreas
    {
        public int Id { get; set; }
        public string EMPLOYEE_NAME { get; set; }
        public string EMPLOYEE_EMAIL { get; set; }
        public string ISSUE_AREA { get; set; }

    }
}
