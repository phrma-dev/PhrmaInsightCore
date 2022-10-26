using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace PhrmaInsightCore.Models.DB
{
    public class TransactionDetail
    {
        public int Id { get; set; }
        public string Source { get; set; }

        public string DIMYEAR { get; set; }
        public string DIMACCOUNTCATEGORYNAME { get; set; }
        public string DIMACCOUNTCATEGORYID { get; set; }
        public string DIMMAINACCOUNTID { get; set; }
        public string DIMMAINACCOUNTNAME { get; set; }
        public string DIMMAINACCOUNTIDANDNAME { get; set; }
        public string DIMDEPARTMENTID { get; set; }
        public string DIMDEPARTMENTNAME { get; set; }
        public string DIMPROJECTCATEGORY { get; set; }
        public string DIMPROJECTID { get; set; }
        public string DIMPROJECTNAME { get; set; }
        public string DIMPROJECTIDANDNAME { get; set; }
        public string Tier1 { get; set; }
        public string Tier2 { get; set; }
        public string Tier3 { get; set; }
        public string Order { get; set; }
        public string ValueandProject { get; set; }
        public string Tier2andTier3 { get; set; }
        public string DIMLOCATIONID { get; set; }
        public string DIMLOCATIONNAME { get; set; }
        public string Unit { get; set; }
        public string DIMREGIONNAME { get; set; }
        public string DIMVENDORID { get; set; }
        public string DIMVENDORNAME { get; set; }
        public string DIMPROJECTMANAGERID { get; set; }
        public string DIMPROJECTMANAGERNAME { get; set; }
        public string DIMREFERENCECODEID { get; set; }
        public string ReferenceCodeName { get; set; }
        public string DIMEMPLOYEEID { get; set; }
        public string EmployeeName { get; set; }
        public string DIMLOBBYAMOUNT { get; set; }
        public string DIMAPPROVERID { get; set; }
        public string DIMREQUESTERID { get; set; }
        public string DIMPOCREATEDBYID { get; set; }
        public string PurchId { get; set; }
        public string POLineNumber { get; set; }
        public string RequestedDeliveryDate { get; set; }
        public string RequisitionId { get; set; }
        public string InvoiceId { get; set; }
        public string InvoiceLineNumber { get; set; }
        public string HeaderDescription { get; set; }
        public string LineDescription { get; set; }
        public string InvoiceCategory { get; set; }
        public string ProcurementCategory { get; set; }
        public string LedgerVoucher { get; set; }
        public float OpenInvoices { get; set; }
        public float Actuals { get; set; }
        public string LastSettleVoucher { get; set; }
        public string PaymentReference { get; set; }
        public string JournalBatchNumber { get; set; }
        public string DueDate { get; set; }
        public string PaymentMethod { get; set; }
        public string Posted { get; set; }
        public string PostedDate { get; set; }
        public string PostedYear { get; set; }
        public string InvoiceDate { get; set; }
        public string ApprovedDate { get; set; }
        public string POLink { get; set; }
        public string ReqLink { get; set; }
        public string DeliveryDate { get; set; }
        public float POAmountOrdered { get; set; }
        public float POBalance { get; set; }
        public float PendingAPInvoices { get; set; }
        public float PendingGLEntries { get; set; }
        public float WorkingBudget { get; set; }
        public float Transfers { get; set; }
        public float BoardApproved { get; set; }
        public float CarryForward { get; set; }
        public float PriorYearExpenses { get; set; }
        public float Revisions { get; set; }
        public float ForecastBudget { get; set; }
        public string MonthId { get; set; }
        public string Requisitioner { get; set; }
        public float Requisitions { get; set; }
        public string RequisitionStatus { get; set; }
        public string Originator { get; set; }
        public string CheckNumber { get; set; }
        public string PaymentDate { get; set; }
        public string RemittanceAddress { get; set; }
        public string Payee { get; set; }
        public string OrganizationType { get; set; }
        public string NewMainAccountId { get; set; }
        public string NewDepartmentId { get; set; }
        public string NewProjectId { get; set; }
        public string NewLocationId { get; set; }
        public string ModifiedPOYN { get; set; }
        public string Justification { get; set; }


    }
}
