using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class MainAccounts
    {
        public int Id { get; set; }
        public string MainAccountId { get; set; }
        public string MainAccountName { get; set; }
        public string AccountCategoryId { get; set; }
        public string AccountCategory { get; set; }



      
    }
}
