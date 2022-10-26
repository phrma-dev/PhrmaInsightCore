using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhrmaInsightCore.Models.DB
{
    public class Prophix_Departments
    {
        public int Id { get; set; }
        public string DepartmentsKey { get; set; }
        public string DepartmentsName { get; set; }

    }
}
