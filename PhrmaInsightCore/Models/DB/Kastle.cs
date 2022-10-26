using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class Kastle
    {
        public int Id { get; set; }
        public string UID { get; set; }
        public string Sequence { get; set; }
        public string VisitDateTime { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Organization { get; set; }
        public string Email { get; set; }
        public string Json { get; set; }
    }
}
