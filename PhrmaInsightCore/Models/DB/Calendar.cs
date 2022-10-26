using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class Calendar
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Start { get; set; }
        public string End { get; set; }


    }
}
