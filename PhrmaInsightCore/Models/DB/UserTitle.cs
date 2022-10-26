using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class UserTitle
    {
        public int Id { get; set; }
        public string FullUserName { get; set; }
        public string Title { get; set; }

      
    }
}
