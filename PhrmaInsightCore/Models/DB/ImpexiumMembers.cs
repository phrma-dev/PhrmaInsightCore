using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class ImpexiumMembers
    {
        public int Id { get; set; }
        public string First_Name{ get; set; }
        public string Last_Name { get; set; }
        public string Email { get; set; }

    }
}
