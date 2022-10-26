using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class Senators
    {
        public int Id { get; set; }
        public string StateId { get; set; }
        public string State { get; set; }
        public string Party { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Image { get; set; }
        public string ProRepublicaId { get; set; }
    }
}
