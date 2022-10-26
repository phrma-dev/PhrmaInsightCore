using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class UserActivity
    {
        public int Id { get; set; }
        public string FullUserName { get; set; }
        public string Page { get; set; }
        public string Activity { get; set; }
        public DateTime ActivityTime { get => DateTime.UtcNow.Subtract(TimeSpan.FromHours(4)); set => value = DateTime.UtcNow.Subtract(TimeSpan.FromHours(4)); }
      
    }
}
