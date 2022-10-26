using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class WeatherWidget
    {
        public int Id { get; set; }
        public string Location { get; set; }
        public string WeatherURL { get; set; }

    }
}
