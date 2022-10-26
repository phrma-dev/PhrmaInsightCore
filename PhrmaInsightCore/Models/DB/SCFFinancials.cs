using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PhrmaInsightCore.Models.DB
{
    public class SCFFinancials
    {
        public int Id { get; set; }
        public string DIMMAINACCOUNTID { get; set; }
        public string DIMDEPARTMENTID { get; set; }
        public string DIMPROJECTID  { get; set; }
        public string DIMLOCATIONID { get; set; }
        public string DIMVENDORID { get; set; }
        public string DIMREFERENCECODEID { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Actuals	{ get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Requisitions	{ get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal POBalance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PendingAPInvoices { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal WorkingBudget { get; set; }
    }
}
