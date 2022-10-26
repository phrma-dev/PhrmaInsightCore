using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class FederalAdvocacyFiles
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string Folder { get; set; }
        public string DocumentCategory { get; set; }
        public string MeetingDate { get; set; }
        public string Members { get; set; }
        public string Year { get; set; }
        public string State { get; set; }

    }
}
