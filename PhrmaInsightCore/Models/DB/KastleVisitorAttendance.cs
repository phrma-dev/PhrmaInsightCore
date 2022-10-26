using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class KastleVisitorAttendance
    {
        public int Id { get; set; }
        public string MeetingDate { get; set; }
        public string CheckedIn { get; set; }
        public string CheckedInDateTime { get; set; }
        public string Host { get; set; }
        public string VisitorFirstName { get; set; }
        public string VisitorLastName { get; set; }
    
        public DateTime Timestamp { get => DateTime.UtcNow.Subtract(TimeSpan.FromHours(5)); set => value = DateTime.UtcNow.Subtract(TimeSpan.FromHours(5)); }

    }
}
