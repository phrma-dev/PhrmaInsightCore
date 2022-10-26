using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class SharePointDocument
    {
        public int Id { get; set; }
        public string Document_Name { get; set; }
        public string Web_Url { get; set; }
        public string Document_Type { get; set; }
        public string State { get; set; }
        public List<string> Members { get; set; }
        public string MembersString { get; set; }
        public string DateOfMeeting { get; set; }

    }

    public class SharePointDocumentStore
    {
        public int Id { get; set; }
        public string Document_Name { get; set; }
        public string Web_Url { get; set; }
        public string Document_Type { get; set; }
        public string State { get; set; }
        public string MembersString { get; set; }
        public string DateOfMeeting { get; set; }

    }
}
