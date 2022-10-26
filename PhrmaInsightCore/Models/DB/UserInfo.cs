using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class UserInfo
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string FullUserName { get; set; }
        public string Name { get; set; }
        public string Title { get; set; }
        public string OfficeLocation { get; set; }
        public string Department { get; set; }
        public Guid Guid { get => Guid.NewGuid(); set => value = Guid.NewGuid(); }
        public DateTime LastUpdate { get => DateTime.UtcNow.Subtract(TimeSpan.FromHours(4)); set => value = DateTime.UtcNow.Subtract(TimeSpan.FromHours(4)); }
      
    }
}
