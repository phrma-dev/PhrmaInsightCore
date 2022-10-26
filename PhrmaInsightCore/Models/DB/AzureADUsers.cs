using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class AzureADUsers
    {
        public int Id { get; set; }
        public string USER_PRINCIPAL_NAME { get; set; }
        public string GIVEN_NAME { get; set; }
        public string SURNAME { get; set; }
        public string MOBILE_PHONE { get; set; }
        public string OFFICE_PHONE { get; set; }
        public string OFFICE_LOCATION { get; set; }
        public string DEPARTMENT { get; set; }
        public string TITLE { get; set; }
        public string alertmediaid { get; set; }
        public string AZURE_AD_MANAGER { get; set; }

    }
}
