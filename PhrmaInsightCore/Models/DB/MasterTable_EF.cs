using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PhrmaInsightCore.Models.DB
{
    public class MasterTable_EF
    {
        public int Id { get; set; }
        public string DefaultDimension { get; set; }
        public string Line_Item_Description { get; set; }
        public string dataAreaId { get; set; }
        public string InventDate { get; set; }
        public string InvoiceDate { get; set; }
        public string PurchID { get; set; }
        public string Qty { get; set; }
        public string InvoiceId { get; set; }
        public string ProcurementCategory { get; set; }
        public string LineNumber { get; set; }
        public string LedgerVoucher { get; set; }
        public string VendTransVoucher { get; set; }
        public string VendLastSettleVoucher { get; set; }
        public string LineAmount { get; set; }
        public string RecId { get; set; }
        public string VendorId { get; set; }
        public string Vendor { get; set; }
        public string ProjectId { get; set; }
        public string Project { get; set; }
        public string Unit { get; set; }
        public string Budget_Year { get; set; }
        public string DepartmentId { get; set; }
        public string Department { get; set; }
        public string MainAccountId { get; set; }
        public string Main_Account	{ get; set; }
        public string Account_Category	{ get; set; }
        public string Account_Category_Description { get; set; }
        public string Lobby_Amount { get; set; }
        public string ProjectManagerId { get; set; }
        public string Project_Manager	{ get; set; }
        public string Reference_Code { get; set; }
        public string LocationId { get; set; }
        public string Location { get; set; }
        public string Region { get; set; }
        public string EmployeeId { get; set; }
        public string Employee { get; set; }
        public int IsPending { get; set; }
        public string PaymentType { get; set; }
        public string Source { get; set; }
        public int Year { get; set; }
        public string PO_Link	{ get; set; }
        public string Req_Link	{ get; set; }
        public string LineDeliveryType { get; set; }
        public string PurchReqId { get; set; }
        public string PurchStatus { get; set; }
        public string RemainInventFinancial { get; set; }
        public string RemainInventPhysical { get; set; }
        public string RemainPurchFinancial { get; set; }
        public string RemainPurchPhysical { get; set; }
        public string RequesterId { get; set; }
        public string Requester { get; set; }
        public string Requisition_Num	{ get; set; }
        public string RequiredDate { get; set; }
        public string Requisition_Status	{ get; set; }
        public string RequisitionPurpose { get; set; }
        public string PurchReqName { get; set; }
        public string PurchReqType { get; set; }
        public string PurchReqLineRecId { get; set; }
        public string ProcurementCategoryName { get; set; }
        public string OriginatorId { get; set; }
        public string Originator { get; set; }
        public string RequisitionerId { get; set; }
        public string Requisitioner { get; set; }
        public string TransDate { get; set; }
        public string DeliveryDate { get; set; }
        public string BudgetCode { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BudgetedAmount { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal BankCurrencyAmount { get; set; }
        public string ChequeNum { get; set; }
        public string ChequeTransDate { get; set; }
        public string ChequeLocationId { get; set; }
        public string RecipientAccountNum { get; set; }
        public string BankNegInstRecipientName { get; set; }
        public string RecipientType { get; set; }
        public string ChequeStatus { get; set; }
        public string ChequeVoucher { get; set; }
        public string ChequeRecipientTransVoucher { get; set; }
        public string ChequeAddress { get; set; }
        public string ChequePayee { get; set; }
        public string IsPaid { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount_Paid_All_Inclusive	{ get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount_Paid_All_Account	{ get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PO_Amount_Ordered { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Req_Amount	{ get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Open_Invoices	{ get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Pending_AP_Invoices { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Pending_GL_Entries { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Pending { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount_Cancelled	{ get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Remaining_Amount	{ get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Working_Budget	{ get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Remaining_Working_Budget { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal PO_Amt_Ordered_And_Soft_Commitments { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Transfers { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Board_Approved	{ get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Carry_Forward	{ get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Prior_Year_Expenses { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Revisions { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Soft_Commitments	{ get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount_Cancelled_for_Hard_Commitments { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Hard_Commitments	{ get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal HardCommitments_Requisitions { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Requisitions { get; set; }
        public int Account_Sort	{ get; set; }
        public string Tier_1	{ get; set; }
        public string Tier_2	{ get; set; }
        public string Tier_3	{ get; set; }
        public string ValueandProject { get; set; }
        public string Tier2andTier3 { get; set; }
        public string Print_Page	{ get; set; }
        public string Print_Department	{ get; set; }
        public string Print_Department_and_Project	{ get; set; }
        public string CFO_Report_Lines { get; set; }
        public string CFO_Report_Main_Groups	{ get; set; }
        public string CFO_Sort	{ get; set; }
        public string TandEEmployee	{ get; set; }
        public string Created_by	{ get; set; }
        public string RequestedDeliveryDate { get; set; }
        public string OrganizationType { get; set; }
        public string PostingDate { get; set; }
        public string InvoiceDescription { get; set; }
        public string ProjectProjectId { get; set; }
        public string RefCode_Description { get; set; }

    }
}
