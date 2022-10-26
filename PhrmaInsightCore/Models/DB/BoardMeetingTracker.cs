using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class BoardMeetingTracker
    {
        public int Id { get; set; }
        public string MemberName { get; set; }
        public string HouseOrSenate { get; set; }
        public string StateCode { get; set; }
        public string State { get; set; }
        public string District { get; set; }
        public string RepresentativeImage { get; set; }
        public string Party { get; set; }
        public string Position { get; set; }
        public DateTime Date { get; set; }
        public string Interaction { get; set; }
        public string Attendee { get; set; }
        public string Phrma_Attendees { get; set; }
        public string Board_Attendees { get; set; }
        public string Organization { get; set; }
        public string Topic { get; set; }
        public string Notes { get; set; }
        
    }
}
