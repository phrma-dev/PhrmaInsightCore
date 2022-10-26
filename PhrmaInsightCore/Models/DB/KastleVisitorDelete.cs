using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class KastleVisitorDelete
    {
        public int Id { get; set; }
        public string GroupId { get; set; }
        public string UId { get; set; }
        public string RecurringId { get; set; }
        public string RecurringInterval { get; set; }
        public string MeetingDate { get; set; }
        public string NiceMeetingDate { get; set; }
        public string NiceMeetingTime { get; set; }
        public string Subject { get; set; }
        public string Host { get; set; }
        public string NotifyEmail { get; set; }
        public string VisitorFirstName { get; set; }
        public string VisitorLastName { get; set; }
        public string VisitorEmail { get; set; }
        public string VisitorOrganization { get; set; }
        public string EndDate { get; set; }
        public string HostDepartment { get; set; }
        public string MeetingOrganizer { get; set; }
        public string MeetingOrganizerEmail { get; set; }
        public string SequenceId { get; set; }
        public DateTime Timestamp { get => DateTime.UtcNow.Subtract(TimeSpan.FromHours(5)); set => value = DateTime.UtcNow.Subtract(TimeSpan.FromHours(5)); }

    }
}
