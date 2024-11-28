using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using static Dugros_Api.Controllers.PurchaseBillController;
using System.Data.SqlClient;
using System.Data;

namespace Dugros_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TallyController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TallyController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class GetExport
        {
            public Guid trn_id { get; set; }
            public string voucher_name { get; set; }
            public string bill_number { get; set; }
            public string bill_date { get; set; }
            public decimal amount { get; set; }
            public string supplier_name { get; set; }
            public string supplier_gstin { get; set; }
            public string supplier_address { get; set; }
            //public Guid bill_state_id { get; set; }
            public string supplier_state { get; set; }
            public string supplier_country { get; set; }
            public string supplier_pincode { get; set; }
            // public string buyer_name { get; set; }
            // public string buyer_address { get; set; }
            // public string buyer_state { get; set; }
            // public string buyer_country { get; set; }
            // public string buyer_pincode { get; set; }
            // public string buyer_gstin { get; set; }


            public List<po_details> po_details { get; set; }
            public List<getitemsPB_Export> details { get; set; }
            // Change to List<Ledger>
            public List<Ledger> ledger { get; set; }

        }
        public class po_details
        {
            public string voucher_no { get; set; }
            public string voucher_date { get; set; }
        }

        public class getitemsPB_Export
        {
            public string tracking_no { get; set; }
            public string item_name { get; set; }
            public string godown_name { get; set; }
            public decimal rate { get; set; }

            public decimal qty { get; set; }
            public decimal amount { get; set; }
            public string hsn_code { get; set; }

            public decimal sgst { get; set; }
            public decimal cgst { get; set; }
            public decimal igst { get; set; }



        }
        public class gettermsPB_Export
        {
            // public string sgst_name { get; set; }
            // public string cgst_name { get; set; }
            // public string igst_name { get; set; }
            // public decimal amt_cgst { get; set; }
            // public decimal amt_sgst { get; set; }
            // public decimal amt_igst { get; set; }
            public decimal rounding_off { get; set; }
            public string? ledger_name { get; set; }
            public decimal amount { get; set; }


        }

        public class ImportDataRequest
        {
            public List<TrnIdData> Data { get; set; }
        }

        public class TrnIdData
        {
            public Guid TrnId { get; set; }
        }

        public class TrnIdResponse
        {
            public Guid TrnId { get; set; }
            public string message { get; set; }
        }
        public class GetPB
        {
            public Guid purchase_Bill_trn_id { get; set; }
            public string voucher_id { get; set; }
            public string voucher_type { get; set; }
            public string doc_no { get; set; }
            public string order_no { get; set; }
            public string doc_date { get; set; }
            public string vendor_id { get; set; }
            public string vendor_name { get; set; }

            public string vendor_ref_no { get; set; }
            public string billing_location { get; set; }
            //public Guid bill_state_id { get; set; }
            public string bill_state { get; set; }
            public string bill_country { get; set; }
            public string bill_pin { get; set; }
            public string bill_gst_reg_type { get; set; }
            public string bill_gst_uin_no { get; set; }
            public string shipper_name { get; set; }
            public string shipper_address { get; set; }
            public string shp_state { get; set; }
            public string shp_country { get; set; }
            public string shp_pin { get; set; }
            public string shp_gst_reg_type { get; set; }
            public string shp_gst_uin_no { get; set; }
            public decimal total_gross_amt { get; set; }
            public decimal taxable_amt { get; set; }
            public decimal tax_amt { get; set; }
            public decimal total_bill_amt { get; set; }
            public decimal rounding_off { get; set; }
            public decimal net_bill_amt { get; set; }
            //public DateTime exp_bill_date { get; set; }
            //public DateTime po_due_date { get; set; }
            public List<getitemsPB> item_details { get; set; }
            public List<gettermsPB> term_details { get; set; }
            public List<dispatchPB> dispatch_Details { get; set; }
            //public string    bill_address { get; set; }
            //public string    ship_address { get; set; }
            public string remarks { get; set; }
            public string warehouse_id { get; set; }
            public string warehouse_name { get; set; }
            public string approval_status { get; set; }
            public string status { get; set; }
        }
        public class PostPB
        {
            public string voucher_id { get; set; }
            public string voucher_type { get; set; }
            public string doc_no { get; set; }
            public string order_no { get; set; }
            public DateTime doc_date { get; set; }
            public string vendor_id { get; set; }
            public string vendor_name { get; set; }
            public string warehouse_id { get; set; } // item_details godown _id
            public string vendor_ref_no { get; set; }
            public string billing_address { get; set; }
            //public Guid bill_state_id { get; set; }
            public string bill_state { get; set; }
            public string bill_pin { get; set; }
            public string bill_gst_reg_type { get; set; }
            public string bill_gst_uin_no { get; set; }
            public string shipper_name { get; set; }
            public string shp_state { get; set; }
            //public Guid shp_state_id { get; set; }
            public string shp_pin { get; set; }
            public string shp_gst_reg_type { get; set; }
            public string shp_gst_uin_no { get; set; }
            public decimal total_gross_amt { get; set; }
            public decimal taxable_amt { get; set; }
            public decimal tax_amt { get; set; }
            public decimal total_bill_amt { get; set; }
            public decimal rounding_off { get; set; }
            public decimal net_bill_amt { get; set; }
            //public DateTime exp_bill_date { get; set; }
            //public DateTime po_due_date { get; set; }
            public List<itemsPB> item_details { get; set; }
            public List<termsPB> term_details { get; set; }
            public List<dispatchPB> dispatch_details { get; set; }
            //public string    bill_address { get; set; }
            public string shipper_address { get; set; }
            public string remarks { get; set; }
            public bool cheque_applicable { get; set; }
            public string status { get; set; }
        }

        public class itemsPB
        {
            public Guid purchase_order_id { get; set; }
            public string item_name { get; set; }
            public string item_id { get; set; }
            public decimal po_qty { get; set; }
            public string uom_primary { get; set; }
            public decimal rate { get; set; }
            public decimal discount_ratio { get; set; }
            public decimal discount_amt { get; set; }
            public decimal amt_before_dis { get; set; }
            public decimal amt_after_dis { get; set; }
            public decimal mrp { get; set; }
            // public string   gl_ledger { get; set; }
            // public string gl_ledger_id { get; set; }
            public string tax_type { get; set; }
            //public decimal gst_ratio { get; set; }
            public decimal amt_sgst { get; set; }
            public decimal amt_cgst { get; set; }
            public decimal amt_igst { get; set; }
            public decimal amt_total { get; set; }
            public string base_reference_no { get; set; }
            public string base_reference_date { get; set; }
            public string base_reference_type { get; set; }
            //public string voucher_no { get; set; }
            //public DateTime voucher_date { get; set; }
            //public decimal indent_qty { get; set; }
            //public decimal pending_qty { get; set; }
            //public string godown_id { get; set; }
            //public string godown_name { get; set; }


        }
        public class termsPB
        {
            public string gl_ledger_name { get; set; }
            public decimal amt { get; set; }
            public string tax_type { get; set; }
            public decimal gst_ratio { get; set; }
            public decimal amt_cgst { get; set; }
            public decimal amt_sgst { get; set; }
            public decimal amt_igst { get; set; }
            public decimal amt_total { get; set; }

        }

        public class dispatchPB
        {
            public DateTime dispatch_dt { get; set; }
            public string dispatch_mode { get; set; }
            public DateTime expected_arrival_date { get; set; }
            public string vehicle_no { get; set; }
            public string contact_no { get; set; }

        }
        public class getitemsPB
        {
            public string item_name { get; set; }
            public string item_id { get; set; }
            public decimal po_qty { get; set; }
            public string uom_primary { get; set; }
            public string uom_primary_name { get; set; }
            public decimal rate { get; set; }
            public decimal discount_ratio { get; set; }
            public decimal discount_amt { get; set; }
            public decimal amt_before_dis { get; set; }
            public decimal amt_after_dis { get; set; }
            public decimal mrp { get; set; }
            // public string   gl_ledger { get; set; }
            // public string gl_ledger_id { get; set; }
            public string tax_type { get; set; }
            //public decimal gst_ratio { get; set; }
            public decimal amt_sgst { get; set; }
            public decimal amt_cgst { get; set; }
            public decimal amt_igst { get; set; }
            public decimal amt_total { get; set; }
            public string base_reference_no { get; set; }
            public DateTime base_reference_date { get; set; }
            public string base_reference_type { get; set; }
            //public string voucher_no { get; set; }
            //public DateTime voucher_date { get; set; }
            //public decimal indent_qty { get; set; }
            //public decimal pending_qty { get; set; }
            //public string warehouse_id { get; set; }
            //public string warehouse_name { get; set; }


        }
        public class gettermsPB
        {
            public Guid gl_ledger_id { get; set; }
            public string gl_ledger_name { get; set; }
            public decimal amt { get; set; }
            public string tax_type { get; set; }
            //public decimal gst_ratio { get; set; }
            public decimal amt_cgst { get; set; }
            public decimal amt_sgst { get; set; }
            public decimal amt_igst { get; set; }
            public decimal amt_total { get; set; }

        }
        public class GetPO_Selection
        {
            public Guid purchase_order_id { get; set; }
            public string voucher_no { get; set; }
            public string voucher_date { get; set; }
            public string vendor_id { get; set; }
            public string vendor_name { get; set; }
            public decimal net_bill_amt { get; set; }
            public string warehouse_id { get; set; }
            public string warehouse_name { get; set; }
            public string billing_location { get; set; }
            public string bill_state { get; set; }
            public string bill_country { get; set; }
            public string bill_pin { get; set; }
            public string bill_gst_reg_type { get; set; }
            public string bill_gst_uin_no { get; set; }
            public string shipper_name { get; set; }
            public string shp_state { get; set; }
            public string shp_country { get; set; }
            public string shp_pin { get; set; }
            public string shp_gst_reg_type { get; set; }
            public string shp_gst_uin_no { get; set; }
            public string shipper_address { get; set; }
            public string item_id { get; set; }
            public string item_name { get; set; }
            public decimal pending_qty { get; set; }
        }

        public class GetPO
        {
            public Guid purchase_Order_trn_id { get; set; }
            public string voucher_id { get; set; }
            public string voucher_type { get; set; }
            public string doc_no { get; set; }
            public string order_no { get; set; }
            public string doc_date { get; set; }
            public string vendor_id { get; set; }
            public string vendor_name { get; set; }
            public string vendor_ref_no { get; set; }
            public string billing_location { get; set; }
            public string bill_country { get; set; }
            public string bill_state { get; set; }
            public string bill_pin { get; set; }
            public string bill_gst_reg_type { get; set; }
            public string bill_gst_uin_no { get; set; }
            public string shipper_name { get; set; }
            public string shipper_address { get; set; }
            public string shp_state { get; set; }
            public string shp_country { get; set; }
            public string shp_pin { get; set; }
            public string shp_gst_reg_type { get; set; }
            public string shp_gst_uin_no { get; set; }
            public decimal total_gross_amt { get; set; }
            public decimal taxable_amt { get; set; }
            public decimal tax_amt { get; set; }
            public decimal total_bill_amt { get; set; }
            public decimal rounding_off { get; set; }
            public decimal net_bill_amt { get; set; }
            public DateTime po_due_date { get; set; }
            public string remarks { get; set; }
            public string warehouse_id { get; set; }
            public string warehouse_name { get; set; }
            public string approval_status { get; set; }
            public List<getitems> item_details { get; set; }
            public List<getterms> term_details { get; set; }
        }

        public class getitems
        {
            public string item_name { get; set; }
            public string item_id { get; set; }
            public decimal po_qty { get; set; }
            public string uom_primary { get; set; }
            public string uom_primary_name { get; set; }
            public decimal rate { get; set; }
            public decimal discount_ratio { get; set; }
            public decimal discount_amt { get; set; }
            public decimal amt_before_dis { get; set; }
            public decimal amt_after_dis { get; set; }
            public decimal mrp { get; set; }
            public string tax_type { get; set; }
            public decimal amt_sgst { get; set; }
            public decimal amt_cgst { get; set; }
            public decimal amt_igst { get; set; }
            public decimal amt_total { get; set; }
            public string base_reference_no { get; set; }
            public DateTime base_reference_date { get; set; }
            public string base_reference_type { get; set; }
            public decimal pending_qty { get; set; }
        }

        public class getterms
        {
            public string gl_ledger_id { get; set; }
            public string gl_ledger_name { get; set; }
            public decimal amt { get; set; }
            public decimal amt_cgst { get; set; }
            public decimal amt_sgst { get; set; }
            public decimal amt_igst { get; set; }
            public decimal amt_total { get; set; }
            public string tax_type { get; set; }
        }

        public class multipletrn
        {
            public string trn_ids { get; set; }
        }
        public class filter_selection
        {
            public Guid user_id { get; set; }
            public string warehouse_id { get; set; }
            public string vendor_id { get; set; }
            public string item_name { get; set; }
        }
        public class Ledger
        {
            public string ledger_name { get; set; }
            public decimal amount { get; set; }
        }

        public class LOI
        {
            public Guid purchase_Order_trn_id { get; set; }
            public string doc_no { get; set; }
            public string warehouse_id { get; set; }
            public string warehouse_name { get; set; }
            public string vendor_id { get; set; }
            public string vendor_name { get; set; }
            public string item_id { get; set; }
            public string item_name { get; set; }
            public decimal amt_igst { get; set; }
            public decimal amt_cgst { get; set; }
            public decimal amt_sgst { get; set; }
            public string uom_primary { get; set; }
            public string uom_primary_name { get; set; }
            public decimal rate { get; set; }
            //public decimal po_qty { get; set; }
            public decimal pending_qty { get; set; }
            public string tax_type { get; set; }
            public decimal discount_ratio { get; set; }
            public decimal discount_amt { get; set; }
            public decimal amt_before_dis { get; set; }
            public decimal amt_after_dis { get; set; }
            public decimal mrp { get; set; }
            public decimal amt_total { get; set; }
            public decimal sgst_rate { get; set; }
            public decimal cgst_rate { get; set; }
            public decimal igst_rate { get; set; }
            public string base_reference_no { get; set; }
            public string base_reference_date { get; set; }
            public string base_reference_type { get; set; }
            public string billing_location { get; set; }
            public string bill_state { get; set; }
            public string bill_country { get; set; }
            public string bill_pin { get; set; }
            public string bill_gst_reg_type { get; set; }
            public string bill_gst_uin_no { get; set; }
            public string shipper_name { get; set; }
            public string shp_state { get; set; }
            public string shp_country { get; set; }
            public string shp_pin { get; set; }
            public string shp_gst_reg_type { get; set; }
            public string shp_gst_uin_no { get; set; }
            public string shipper_address { get; set; }
            public decimal net_bill_amt { get; set; }
        }
        public class terms_bill
        {
            public string gl_ledger_name { get; set; }
            public decimal amt { get; set; }
            public string tax_type { get; set; }
            public decimal gst_ratio { get; set; }
            public decimal amt_cgst { get; set; }
            public decimal amt_sgst { get; set; }
            public decimal amt_igst { get; set; }
            public decimal amt_total { get; set; }
            public decimal sgst_rate { get; set; }
            public decimal cgst_rate { get; set; }
            public decimal igst_rate { get; set; }

        }

        public class ApiResponse
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public List<Warehoseoutput> Warehousename { get; set; }
        }

        public class Warehoseoutput
        {
            public string warehouse { get; set; }
        }
        public class warehousePayload
        {
            public List<AddWarehouse> godowns { get; set; }
        }
        public class AddWarehouse
        {
            public string? id { get; set; }
            public string? name { get; set; }
            public string? address { get; set; }
            public string? parentID { get; set; }
            public string? parent { get; set; }
            public bool? hasNoSpace { get; set; }
            public bool? isExternal { get; set; }
            public bool? isInternal { get; set; }
            public string? lastUpdatedBy { get; set; }
            public string? alias { get; set; }
            public string? level { get; set; }
        }
        public class Warehouse
        {
            public string? ID { get; set; }
            public string? Name { get; set; }
            public string? Address { get; set; }
            public string? parentID { get; set; }
            public string? parent { get; set; }
            public bool? hasNoSpace { get; set; }
            public bool? isExternal { get; set; }
            public bool? isInternal { get; set; }
            public string? LastUpdatedBy { get; set; }
            public string? alias { get; set; }
            public string? level { get; set; }
        }

        public class ApiResponse1
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public List<GroupmasterOutput> Groupmastername { get; set; }
        }

        public class GroupmasterOutput
        {
            public string Groupmaster { get; set; }
        }
        public class GroupmasterPayload
        {
            public List<AddGroupmaster> groupMasters { get; set; }
        }
        public class AddGroupmaster
        {
            public string? acc_Group_ID { get; set; }
            public string? name { get; set; }
            public string? parentID { get; set; }
            public string? parent { get; set; }
            public bool? isAddable { get; set; }
            public bool? isRevenue { get; set; }
            public string? primaryGroupID { get; set; }
            public string? primaryGroup { get; set; }
            public Int32? level { get; set; }
            public string? firstAlias { get; set; }
            public string? hsnDescription { get; set; }
            public string? hsnCode { get; set; }
            public decimal? integratedTax { get; set; }
            public decimal? centralTax { get; set; }
            public decimal? stateTax { get; set; }
            public decimal? cess { get; set; }
            public string? lastUpdatedBy { get; set; }
        }
        public class GetGroupmaster
        {
            public string? ID { get; set; }
            public string? Name { get; set; }
            public string? parentID { get; set; }
            public string? parent { get; set; }
            public bool? isAddable { get; set; }
            public Int32? level { get; set; }
            public string? firstAlias { get; set; }
            public string? hsnDescription { get; set; }
            public string? hsnCode { get; set; }
            public decimal? integratedTax { get; set; }
            public decimal? centralTax { get; set; }
            public decimal? stateTax { get; set; }
            public decimal? cess { get; set; }
            public string? lastUpdatedBy { get; set; }
        }

        public class ApiResponse2
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public List<ledgeroutput> ledgername { get; set; }
        }

        public class ledgeroutput
        {
            public string ledger { get; set; }
        }
        public class LedgerPayload
        {
            public List<AddLedger> ledgers { get; set; }
        }
        public class AddLedger
        {
            public string? ledger_ID { get; set; }
            public string? name { get; set; }
            public string? parentID { get; set; }
            public string? parent { get; set; }
            public string? primaryGroupID { get; set; }
            public string? primaryGroup { get; set; }
            public string? currencyName { get; set; }
            public string? eMail { get; set; }
            public string? priorStateName { get; set; }
            public string? website { get; set; }
            public string? incomeTaxNumber { get; set; }
            public string? gstRegistrationType { get; set; }
            public string? priceLevel { get; set; }
            public string? narration { get; set; }
            public string? tdsApplicable { get; set; }
            public string? tcsApplicable { get; set; }
            public string? gstApplicable { get; set; }
            public string? billCreditPeriod { get; set; }
            public string? msmeRegNumber { get; set; }
            public string? countryofResidence { get; set; }
            public string? emailCC { get; set; }
            public string? description { get; set; }
            public string? ledgerPhone { get; set; }
            public string? ledgerFax { get; set; }
            public string? ledgerContact { get; set; }
            public string? ledgerMobile { get; set; }
            public string? ledgercountryISDcode { get; set; }
            public string? gstTypeofSupply { get; set; }
            public string? roundingMethod { get; set; }
            public string? tdsDeducteeType { get; set; }
            public bool? isBillWiseOn { get; set; }
            public bool? isCostCentresOn { get; set; }
            public bool? isInterestOn { get; set; }
            public bool? forPayroll { get; set; }
            public bool? interestonBillwise { get; set; }
            public bool? isTCSApplicable { get; set; }
            public bool? isTDSApplicable { get; set; }
            public bool? overRideCreditLimit { get; set; }
            public decimal? openingBalance { get; set; }
            public decimal? creditLimit { get; set; }
            public string? mailingName { get; set; }
            public string? address1 { get; set; }
            public string? address2 { get; set; }
            public string? address3 { get; set; }
            public string? address4 { get; set; }
            public string? address5 { get; set; }
            public string? pinCode { get; set; }
            public string? firstAlias { get; set; }
            public string? hsnDescription { get; set; }
            public string? hsnCode { get; set; }
            public string? partyGSTIN { get; set; }
            public decimal? integratedTax { get; set; }
            public decimal? centralTax { get; set; }
            public decimal? stateTax { get; set; }
            public decimal? cess { get; set; }
            public string? bankingConfigBank { get; set; }
            public string? bankAccHolderName { get; set; }
            public string? ledgerAccountNumber { get; set; }
            public string? ifsCode { get; set; }
            public string? branchName { get; set; }
            public string? bankBSRCode { get; set; }
            public string? swiftCode { get; set; }
            public string? transactaionType { get; set; }
            public string? favouringName { get; set; }
            public string? partyAccountNumber { get; set; }
            public string? partyIFSCode { get; set; }
            public string? partyBank { get; set; }
            public string? partySWIFTCode { get; set; }
            public string? lastUpdatedBy { get; set; }

        }
        public class GetLedger
        {
            public string? ledger_ID { get; set; }
            public string? name { get; set; }
            public string? parentID { get; set; }
            public string? parent { get; set; }
            public string? primaryGroupID { get; set; }
            public string? primaryGroup { get; set; }
            public string? currencyName { get; set; }
            public string? EMail { get; set; }
            public string? priorStateName { get; set; }
            public string? website { get; set; }
            public string? incomeTaxNumber { get; set; }
            public string? gstRegistrationType { get; set; }
            public string? priceLevel { get; set; }
            public string? narration { get; set; }
            public string? tdsApplicable { get; set; }
            public string? tcsApplicable { get; set; }
            public string? gstApplicable { get; set; }
            public string? billCreditPeriod { get; set; }
            public string? msmeRegNumber { get; set; }
            public string? countryofResidence { get; set; }
            public string? emailCC { get; set; }
            public string? description { get; set; }
            public string? ledgerPhone { get; set; }
            public string? ledgerFax { get; set; }
            public string? ledgerContact { get; set; }
            public string? ledgerMobile { get; set; }
            public string? ledgercountryISDcode { get; set; }
            public string? gstTypeofSupply { get; set; }
            public string? roundingMethod { get; set; }
            public string? tdsDeducteeType { get; set; }
            public bool? isBillWiseOn { get; set; }
            public bool? isCostCentresOn { get; set; }
            public bool? isInterestOn { get; set; }
            public bool? forPayroll { get; set; }
            public bool? interestonBillwise { get; set; }
            public bool? isTCSApplicable { get; set; }
            public bool? isTDSApplicable { get; set; }
            public bool? overRideCreditLimit { get; set; }
            public decimal? openingBalance { get; set; }
            public decimal? creditLimit { get; set; }
            public string? mailingName { get; set; }
            public string? address1 { get; set; }
            public string? address2 { get; set; }
            public string? address3 { get; set; }
            public string? address4 { get; set; }
            public string? address5 { get; set; }
            public string? pinCode { get; set; }
            public string? firstAlias { get; set; }
            public string? hsnDescription { get; set; }
            public string? hsnCode { get; set; }
            public string? partyGSTIN { get; set; }
            public decimal? integratedTax { get; set; }
            public decimal? centralTax { get; set; }
            public decimal? stateTax { get; set; }
            public decimal? cess { get; set; }
            public string? bankingConfigBank { get; set; }
            public string? bankAccHolderName { get; set; }
            public string? ledgerAccountNumber { get; set; }
            public string? ifsCode { get; set; }
            public string? branchName { get; set; }
            public string? bankBSRCode { get; set; }
            public string? swiftCode { get; set; }
            public string? transactaionType { get; set; }
            public string? favouringName { get; set; }
            public string? partyAccountNumber { get; set; }
            public string? partyIFSCode { get; set; }
            public string? partyBank { get; set; }
            public string? partySWIFTCode { get; set; }
            public string? lastUpdatedBy { get; set; }

        }

        public class ApiResponse3
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public List<Categoryoutput> Categoryname { get; set; }
        }

        public class Categoryoutput
        {
            public string Category { get; set; }
        }
        public class CategoryPayload
        {
            public List<AddCategory> categories { get; set; }
        }
        public class AddCategory
        {
            public string? id { get; set; }
            public string? name { get; set; }
            public string? parentID { get; set; }
            public string? parent { get; set; }
            public string? lastUpdatedBy { get; set; }
            public string? level { get; set; }
        }

        public class GetStockCategory
        {
            public string? ID { get; set; }
            public string? Name { get; set; }
            public string? parentID { get; set; }
            public string? parent { get; set; }
            public string? lastUpdatedBy { get; set; }
            public string? level { get; set; }
        }

        public class ApiResponse4
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public List<Groupoutput> Groupname { get; set; }
        }

        public class Groupoutput
        {
            public string Group { get; set; }
        }
        public class GroupPayload
        {
            public List<AddGroup> groups { get; set; }
        }
        public class AddGroup
        {
            public string? id { get; set; }
            public string? name { get; set; }
            public string? parentID { get; set; }
            public string? parent { get; set; }
            public bool? isAddable { get; set; }
            public string? firstAlias { get; set; }
            public string? hsnCode { get; set; }
            public string? hsnDescription { get; set; }
            public decimal? integratedTax { get; set; }
            public decimal? centralTax { get; set; }
            public decimal? stateTax { get; set; }
            public decimal? cess { get; set; }
            public string? lastUpdatedBy { get; set; }
            public string? level { get; set; }
        }

        public class GetStockGroup
        {
            public string? ID { get; set; }
            public string? Name { get; set; }
            public string? parentID { get; set; }
            public string? parent { get; set; }
            public bool? isAddable { get; set; }
            public string? firstAlias { get; set; }
            public string? hsnCode { get; set; }
            public string? hsnDescription { get; set; }
            public decimal? integratedTax { get; set; }
            public decimal? centralTax { get; set; }
            public decimal? stateTax { get; set; }
            public decimal? cess { get; set; }
            public string? lastUpdatedBy { get; set; }
            public int? level { get; set; }

        }

        public class ApiResponse5
        {
            public bool Success { get; set; }
            public string? Message { get; set; }
            public List<InsertedInfo>? returnedInfo { get; set; }
        }

        public class InsertedInfo
        {
            public string? msg { get; set; }
        }

        public class StockItemPayload
        {
            public List<AddStockItem>? stockItems { get; set; }
        }

        public class AddStockItem
        {
            public string? item_ID { get; set; }
            public string? name { get; set; }
            public string? parentID { get; set; }
            public string? parent { get; set; }
            public string? categoryID { get; set; }
            public string? category { get; set; }
            public string? mailingName { get; set; }
            public string? narration { get; set; }
            public string? tcsApplicable { get; set; }
            public string? gstApplicable { get; set; }
            public string? description { get; set; }
            public string? gstTypeofSupply { get; set; }
            public string? costingMethod { get; set; }
            public string? valuationMethod { get; set; }
            public string? baseUnitId { get; set; }
            public string? baseUnits { get; set; }
            public string? addUnitId { get; set; }
            public string? additionalUnits { get; set; }
            public bool? isBatchWiseOn { get; set; }
            public bool? isPerishableOn { get; set; }
            public bool? ignoreNegativeStock { get; set; }
            public bool? treatSalesasManufactured { get; set; }
            public bool? treatPurchasesasConsumed { get; set; }
            public bool? treatRejectsasScrap { get; set; }
            public bool? hasMfgDate { get; set; }
            public bool? allowUseofExpiredItems { get; set; }
            public bool? inclusiveTax { get; set; }
            public decimal? denominator { get; set; }
            public decimal? conversion { get; set; }
            public decimal? openingBalance { get; set; }
            public decimal? openingValue { get; set; }
            public decimal? reorderBase { get; set; }
            public decimal? minimumOrderBase { get; set; }
            public decimal? openingRate { get; set; }
            public string? firstAlias { get; set; }
            public string? hsnDescription { get; set; }
            public string? hsnCode { get; set; }
            public decimal? integratedTax { get; set; }
            public decimal? centralTax { get; set; }
            public decimal? stateTax { get; set; }
            public decimal? cess { get; set; }
            public bool? applicableForReverseCharge { get; set; }
            public string? lastUpdatedBy { get; set; }
            public List<PurchasePrice>? costPrice { get; set; }
            public List<SalesPrice>? salesPrice { get; set; }
        }

        public class PurchasePrice
        {
            public string? effectDate { get; set; }
            public decimal? price { get; set; }
            public string? unitId { get; set; }
            public string? unitName { get; set; }
        }

        public class SalesPrice
        {
            public string? effectDate { get; set; }
            public decimal? price { get; set; }
            public string? unitId { get; set; }
            public string? unitName { get; set; }
        }

        public class GetStockItem
        {
            public string? item_ID { get; set; }
            public string? name { get; set; }
            public string? parent { get; set; }
            public string? category { get; set; }
            public string? baseUnits { get; set; }
        }
        public class Reportitem
        {
            public string item { get; set; }
            public string group { get; set; }
            public string category { get; set; }
        }

        public class ApiResponse6
        {
            public bool Success { get; set; }
            public string Message { get; set; }
            public List<Creditoutput> Creditname { get; set; }
        }

        public class Creditoutput
        {
            public string Credit { get; set; }
        }
        public class CreditPayload
        {
            public List<AddCredit> data { get; set; }
        }
        public class AddCredit
        {
            public string? tally_trn_id { get; set; }
            public string? item { get; set; }
            public string? primary_total_qty { get; set; }
            public string? secondary_total_qty { get; set; }
            public List<Details> details { get; set; }
        }

        public class Details
        {
            public string? warehouse { get; set; }
            public string? batch_no { get; set; }
            public string? primary_closing_qty { get; set; }
            public string? secondary_closing_qty { get; set; }
            public string? price_per_item { get; set; }
            public string? stock_value { get; set; }
        }

       


        [HttpGet("Export/GetPurchaseBills")]
        public async Task<IActionResult> GetPurchaseBills()
        {
            try
            {
                var purchaseOrders = new List<GetExport>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();
                    using (var command = new SqlCommand("dbo.Export_Get", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            while (await reader.ReadAsync())
                            {
                                var purchaseOrder = new GetExport
                                {
                                    trn_id = reader.IsDBNull(reader.GetOrdinal("trn_id")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("trn_id")),
                                    voucher_name = reader.IsDBNull(reader.GetOrdinal("voucher_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("voucher_name")),
                                    bill_number = reader.IsDBNull(reader.GetOrdinal("bill_number")) ? string.Empty : reader.GetString(reader.GetOrdinal("bill_number")),
                                    bill_date = reader.IsDBNull(reader.GetOrdinal("bill_date")) ? string.Empty : reader.GetString(reader.GetOrdinal("bill_date")),
                                    amount = reader.IsDBNull(reader.GetOrdinal("amount")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amount")),
                                    supplier_name = reader.IsDBNull(reader.GetOrdinal("vendor_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("vendor_name")),
                                    supplier_address = reader.IsDBNull(reader.GetOrdinal("supplier_address")) ? string.Empty : reader.GetString(reader.GetOrdinal("supplier_address")),
                                    supplier_country = reader.IsDBNull(reader.GetOrdinal("supplier_country")) ? string.Empty : reader.GetString(reader.GetOrdinal("supplier_country")),
                                    supplier_state = reader.IsDBNull(reader.GetOrdinal("supplier_state")) ? string.Empty : reader.GetString(reader.GetOrdinal("supplier_state")),
                                    supplier_pincode = reader.IsDBNull(reader.GetOrdinal("supplier_pincode")) ? string.Empty : reader.GetString(reader.GetOrdinal("supplier_pincode")),
                                    supplier_gstin = reader.IsDBNull(reader.GetOrdinal("supplier_gstin")) ? string.Empty : reader.GetString(reader.GetOrdinal("supplier_gstin")),
                                    // buyer_name = reader.IsDBNull(reader.GetOrdinal("buyer_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("buyer_name")),
                                    // buyer_address = reader.IsDBNull(reader.GetOrdinal("buyer_address")) ? string.Empty : reader.GetString(reader.GetOrdinal("buyer_address")),
                                    // buyer_state = reader.IsDBNull(reader.GetOrdinal("buyer_state")) ? string.Empty : reader.GetString(reader.GetOrdinal("buyer_state")),
                                    // buyer_country = reader.IsDBNull(reader.GetOrdinal("buyer_country")) ? string.Empty : reader.GetString(reader.GetOrdinal("buyer_country")),
                                    // buyer_pincode = reader.IsDBNull(reader.GetOrdinal("buyer_pincode")) ? string.Empty : reader.GetString(reader.GetOrdinal("buyer_pincode")),
                                };

                                // Deserialize JSON properties
                                var detailsDetailsJson = reader.IsDBNull(reader.GetOrdinal("details")) ? "[]" : reader.GetString(reader.GetOrdinal("details"));
                                var itemList = JsonConvert.DeserializeObject<List<getitemsPB_Export>>(detailsDetailsJson);

                                // Deserialize JSON properties
                                var termsDetailsJson = reader.IsDBNull(reader.GetOrdinal("ledger")) ? "[]" : reader.GetString(reader.GetOrdinal("ledger"));
                                var termsList = JsonConvert.DeserializeObject<List<gettermsPB_Export>>(termsDetailsJson);

                                var po_details_json = reader.IsDBNull(reader.GetOrdinal("po_details")) ? "[]" : reader.GetString(reader.GetOrdinal("po_details"));
                                var po_details = JsonConvert.DeserializeObject<List<po_details>>(po_details_json);

                                // Transform to the desired format
                                var transformedLedger = new List<Ledger>();

                                if (termsList != null && termsList.Count > 0)
                                {
                                    foreach (var terms in termsList)
                                    {
                                        transformedLedger.Add(new Ledger
                                        {
                                            ledger_name = terms.ledger_name,
                                            amount = terms.amount
                                        });
                                    }
                                    transformedLedger.Add(new Ledger
                                    {
                                        ledger_name = "ROUND OFF (PURCHASE)",
                                        amount = (decimal)reader["rounding_off"]
                                    });
                                }
                                purchaseOrder.details = itemList;
                                purchaseOrder.ledger = transformedLedger;
                                purchaseOrder.po_details = po_details;

                                // Add the purchase order to the list
                                purchaseOrders.Add(purchaseOrder);
                            }
                        }
                    }
                }

                // Create an object to hold the data array
                var result = new { data = purchaseOrders };

                // Return the object containing the data array
                return Ok(result);
            }
            catch (Exception ex)
            {
                // Log the exception (Consider using a logging library here)
                Console.WriteLine($"Error: {ex.Message}");

                // Return error message
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("Import/ImportData")]
        public IActionResult ImportData([FromBody] ImportDataRequest request)
        {
            if (request == null || request.Data == null || !request.Data.Any())
            {
                return BadRequest("The data field is required and must contain at least one TRN ID.");
            }

            try
            {
                var trnIds = request.Data.Select(d => d.TrnId.ToString()).ToArray();
                var trnIdsString = string.Join(",", trnIds);

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.ImportData_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@TrnIds", trnIdsString);

                        using (var reader = command.ExecuteReader())
                        {
                            var result = new List<TrnIdResponse>();
                            while (reader.Read())
                            {
                                result.Add(new TrnIdResponse
                                {
                                    TrnId = reader.GetGuid(reader.GetOrdinal("trn_id")),
                                    message = reader.GetString("message")
                                });
                            }

                            return Ok(new { data = result });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("Godown")]
        public IActionResult GetWarehouse()
        {
            try
            {
                List<Warehouse> warehouse = new List<Warehouse>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT [ID],[Name],[Address],[ParentID],[ParentName],[HasNoSpace],[IsExternal],[IsInternal],[LastUpdatedBy],[Alias],[GroupLevel]  FROM [Universal].[dbo].[tbl_mst_Stock_Godown]", connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            warehouse.Add(new Warehouse
                            {
                                ID = reader["ID"].ToString(),
                                Name = reader["Name"].ToString(),
                                Address = reader["Address"].ToString(),
                                parentID = reader["ParentID"].ToString(),
                                parent = reader["ParentName"].ToString(),
                                hasNoSpace = (bool)reader["HasNoSpace"],
                                isExternal = (bool)reader["IsExternal"],
                                isInternal = (bool)reader["IsInternal"],
                                alias = reader["Alias"].ToString(),
                                LastUpdatedBy = reader["LastUpdatedBy"].ToString(),
                                level = reader["GroupLevel"].ToString(),
                            });
                        }
                    }
                }

                return Ok(warehouse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("Godown/Godown")]
        public IActionResult CreateWarehouse([FromBody] warehousePayload payload)
        {
            try
            {
                List<string> Warehousename = new List<string>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    foreach (var addWarehouse in payload.godowns)
                    {
                        string jsonData = JsonConvert.SerializeObject(addWarehouse);

                        using (var command = new SqlCommand("sp_mst_Stock_Godown_CRUD", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@jsonData", jsonData);

                            SqlParameter outputParameter = new SqlParameter();
                            outputParameter.ParameterName = "@Warehouse";
                            outputParameter.SqlDbType = SqlDbType.NVarChar;
                            outputParameter.Size = -1;
                            outputParameter.Direction = ParameterDirection.Output;
                            command.Parameters.Add(outputParameter);

                            command.ExecuteNonQuery();

                            string insertedWarehouseName = outputParameter.Value.ToString();
                            Warehousename.Add(insertedWarehouseName);
                        }
                    }
                }

                List<Warehoseoutput> warehouselist = Warehousename.ConvertAll(wh => new Warehoseoutput { warehouse = wh });

                var response = new ApiResponse
                {
                    Success = true,
                    Message = "Warehouses created successfully!",
                    Warehousename = warehouselist
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Warehousename = null
                };

                return StatusCode(500, response);
            }
        }

        [HttpGet("GroupMaster")]
        public IActionResult GetGroup()
        {
            try
            {
                List<GetGroupmaster> group = new List<GetGroupmaster>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT [ID],[Name],[ParentID],[Parent],[IsAddable],[FirstAlias],[HSNDescription],[HSNCode],[IntegratedTax],[CentralTax],[StateTax],[Cess],[LastModifiedBy],[Group_Level]  FROM [Universal].[dbo].[tbl_mst_Stock_Group]", connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            group.Add(new GetGroupmaster
                            {
                                ID = reader["ID"].ToString(),
                                Name = reader["Name"].ToString(),
                                parentID = reader["ParentID"].ToString(),
                                parent = reader["Parent"].ToString(),
                                firstAlias = reader["FirstAlias"].ToString(),
                                hsnDescription = reader["HSNDescription"].ToString(),
                                isAddable = (bool)reader["IsAddable"],
                                hsnCode = reader["HSNCode"].ToString(),
                                level = int.Parse(reader["Group_Level"].ToString()),
                                integratedTax = decimal.Parse(reader["IntegratedTax"].ToString()),
                                centralTax = decimal.Parse(reader["CentralTax"].ToString()),
                                stateTax = decimal.Parse(reader["StateTax"].ToString()),
                                cess = decimal.Parse(reader["Cess"].ToString()),
                                lastUpdatedBy = reader["LastModifiedBy"].ToString(),
                            });
                        }
                    }
                }

                return Ok(group);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpPost("GroupMaster/MultipleGroupMaster")]
        public IActionResult CreateGroupmaster([FromBody] GroupmasterPayload payload)
        {
            try
            {
                List<string> Groupmastername = new List<string>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    foreach (var groupmaster in payload.groupMasters)
                    {
                        string jsonData = JsonConvert.SerializeObject(groupmaster);

                        using (var command = new SqlCommand("sp_mst_Accounting_Group_CRUD", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@jsonData", jsonData);

                            // Output parameter to capture inserted item names
                            SqlParameter outputParameter = new SqlParameter();
                            outputParameter.ParameterName = "@Groupmaster";
                            outputParameter.SqlDbType = SqlDbType.NVarChar;
                            outputParameter.Size = -1;
                            outputParameter.Direction = ParameterDirection.Output;
                            command.Parameters.Add(outputParameter);

                            command.ExecuteNonQuery();

                            // Retrieve inserted item name from output parameter
                            string insertedGroupmastername = outputParameter.Value.ToString();

                            // Add inserted item name to the list
                            Groupmastername.Add(insertedGroupmastername);
                        }
                    }
                }

                List<GroupmasterOutput> Groupmasterlist = Groupmastername.ConvertAll(groupmaster => new GroupmasterOutput { Groupmaster = groupmaster });

                var response = new ApiResponse1
                {
                    Success = true,
                    Message = "Group created successfully!",
                    Groupmastername = Groupmasterlist
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse1
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Groupmastername = null
                };

                return StatusCode(500, response);
            }
        }

        [HttpGet("LedgerMaster/GetLedgers")]
        public IActionResult GetLedgers()
        {
            try
            {
                List<GetLedger> ledgers = new List<GetLedger>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT [Ledger_ID],[Name],[ParentID],[Parent],[PrimaryGroupID],[PrimaryGroup],[CurrencyName],[EMail],[PriorStateName],[Website],[IncomeTaxNumber],[GSTRegistrationType],[PriceLevel],[Narration],[TDSApplicable],[TCSApplicable],[GSTApplicable],[BillCreditPeriod],[MSMERegNumber],[CountryofResidence],[EmailCC],[Description],[LedgerPhone],[LedgerFax],[LedgerContact],[LedgerMobile],[LedgercountryISDcode],[GSTTypeofSupply],[RoundingMethod],[TDSDeducteeType],[IsBillWiseOn],[IsCostCentresOn],[IsInterestOn],[ForPayroll],[InterestonBillwise],[IsTCSApplicable],[IsTDSApplicable],[OverRideCreditLimit],[OpeningBalance],[CreditLimit],[MailingName],[Address1],[Address2],[Address3],[Address4],[Address5],[PINCode],[FirstAlias],[HSNDescription],[HSNCode],[PartyGSTIN],[IntegratedTax],[CentralTax],[StateTax],[Cess],[BankingConfigBank],[BankAccHolderName],[LedgerAccountNumber],[IFSCode],[BranchName],[BankBSRCode],[SwiftCode],[TransactaionType],[FavouringName],[PartyAccountNumber],[PartyIFSCode],[PartyBank],[PartySWIFTCode],[LastUpdatedBy]  FROM [Universal].[dbo].[tbl_mst_Accounting_LedgerType]", connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ledgers.Add(new GetLedger
                            {
                                ledger_ID = reader["Ledger_ID"].ToString(),
                                name = reader["Name"].ToString(),
                                parentID = reader["ParentID"].ToString(),
                                parent = reader["Parent"].ToString(),
                                primaryGroupID = reader["PrimaryGroupID"].ToString(),
                                primaryGroup = reader["PrimaryGroup"].ToString(),
                                currencyName = reader["CurrencyName"].ToString(),
                                EMail = reader["EMail"].ToString(),
                                priorStateName = reader["PriorStateName"].ToString(),
                                website = reader["Website"].ToString(),
                                incomeTaxNumber = reader["IncomeTaxNumber"].ToString(),
                                gstRegistrationType = reader["GSTRegistrationType"].ToString(),
                                priceLevel = reader["PriceLevel"].ToString(),
                                narration = reader["Narration"].ToString(),
                                tdsApplicable = reader["TDSApplicable"].ToString(),
                                tcsApplicable = reader["TCSApplicable"].ToString(),
                                gstApplicable = reader["GSTApplicable"].ToString(),
                                billCreditPeriod = reader["BillCreditPeriod"].ToString(),
                                msmeRegNumber = reader["MSMERegNumber"].ToString(),
                                countryofResidence = reader["CountryofResidence"].ToString(),
                                emailCC = reader["EmailCC"].ToString(),
                                description = reader["Description"].ToString(),
                                ledgerPhone = reader["LedgerPhone"].ToString(),
                                ledgerFax = reader["LedgerFax"].ToString(),
                                ledgerContact = reader["LedgerContact"].ToString(),
                                ledgerMobile = reader["LedgerMobile"].ToString(),
                                ledgercountryISDcode = reader["LedgercountryISDcode"].ToString(),
                                gstTypeofSupply = reader["GSTTypeofSupply"].ToString(),
                                roundingMethod = reader["RoundingMethod"].ToString(),
                                tdsDeducteeType = reader["TDSDeducteeType"].ToString(),
                                isBillWiseOn = (bool)reader["IsBillWiseOn"],
                                isCostCentresOn = (bool)reader["IsCostCentresOn"],
                                isInterestOn = (bool)reader["IsInterestOn"],
                                forPayroll = (bool)reader["ForPayroll"],
                                interestonBillwise = (bool)reader["InterestonBillwise"],
                                isTCSApplicable = (bool)reader["IsTCSApplicable"],
                                isTDSApplicable = (bool)reader["IsTDSApplicable"],
                                overRideCreditLimit = (bool)reader["OverRideCreditLimit"],
                                openingBalance = decimal.Parse(reader["OpeningBalance"].ToString()),
                                creditLimit = decimal.Parse(reader["CreditLimit"].ToString()),
                                mailingName = reader["MailingName"].ToString(),
                                address1 = reader["Address1"].ToString(),
                                address2 = reader["Address2"].ToString(),
                                address3 = reader["Address3"].ToString(),
                                address4 = reader["Address4"].ToString(),
                                address5 = reader["Address5"].ToString(),
                                pinCode = reader["PINCode"].ToString(),
                                firstAlias = reader["FirstAlias"].ToString(),
                                hsnDescription = reader["HSNDescription"].ToString(),
                                hsnCode = reader["HSNCode"].ToString(),
                                partyGSTIN = reader["PartyGSTIN"].ToString(),
                                integratedTax = decimal.Parse(reader["IntegratedTax"].ToString()),
                                centralTax = decimal.Parse(reader["CentralTax"].ToString()),
                                stateTax = decimal.Parse(reader["StateTax"].ToString()),
                                cess = decimal.Parse(reader["Cess"].ToString()),
                                bankingConfigBank = reader["BankingConfigBank"].ToString(),
                                bankAccHolderName = reader["BankAccHolderName"].ToString(),
                                ledgerAccountNumber = reader["LedgerAccountNumber"].ToString(),
                                ifsCode = reader["IFSCode"].ToString(),
                                branchName = reader["BranchName"].ToString(),
                                bankBSRCode = reader["BankBSRCode"].ToString(),
                                swiftCode = reader["SwiftCode"].ToString(),
                                transactaionType = reader["TransactaionType"].ToString(),
                                favouringName = reader["FavouringName"].ToString(),
                                partyAccountNumber = reader["PartyAccountNumber"].ToString(),
                                partyIFSCode = reader["PartyIFSCode"].ToString(),
                                partyBank = reader["PartyBank"].ToString(),
                                partySWIFTCode = reader["PartySWIFTCode"].ToString(),
                                lastUpdatedBy = reader["LastUpdatedBy"].ToString(),

                            });
                        }
                    }
                }

                return Ok(ledgers);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("LedgerMaster/MultipleUnit")]
        public IActionResult CreateUnit([FromBody] LedgerPayload payload)
        {
            try
            {
                List<string> ledgername = new List<string>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    foreach (var voucher in payload.ledgers)
                    {
                        string jsonData = JsonConvert.SerializeObject(voucher);

                        using (var command = new SqlCommand("sp_mst_Accounting_LedgerType_CRUD", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@jsonData", jsonData);

                            // Output parameter to capture inserted item names
                            SqlParameter outputParameter = new SqlParameter();
                            outputParameter.ParameterName = "@Ledger";
                            outputParameter.SqlDbType = SqlDbType.NVarChar;
                            outputParameter.Size = -1;
                            outputParameter.Direction = ParameterDirection.Output;
                            command.Parameters.Add(outputParameter);

                            command.ExecuteNonQuery();

                            // Retrieve inserted item name from output parameter
                            string insertedVouchername = outputParameter.Value.ToString();

                            // Add inserted item name to the list
                            ledgername.Add(insertedVouchername);
                        }
                    }
                }

                List<ledgeroutput> ledgerlist = ledgername.ConvertAll(voucher => new ledgeroutput { ledger = voucher });

                var response = new ApiResponse2
                {
                    Success = true,
                    Message = "Ledger created successfully!",
                    ledgername = ledgerlist
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse2
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    ledgername = null
                };

                return StatusCode(500, response);
            }
        }

        [HttpGet("StockCategory")]
        public IActionResult GetCategory()
        {
            try
            {
                List<GetStockCategory> stockcategory = new List<GetStockCategory>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT [Stock_Category_ID],[Name],[ParentID],[Parent],[LastUpdatedBy],[Group_Level]  FROM [Universal].[dbo].[tbl_mst_Stock_Category]", connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stockcategory.Add(new GetStockCategory
                            {
                                ID = reader["Stock_Category_ID"].ToString(),
                                Name = reader["Name"].ToString(),
                                parentID = reader["ParentID"].ToString(),
                                parent = reader["Parent"].ToString(),
                                lastUpdatedBy = reader["LastUpdatedBy"].ToString(),
                                level = reader["Group_Level"].ToString(),
                            });
                        }
                    }
                }

                return Ok(stockcategory);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpPost("StockCategory/MultipleCategory")]
        public IActionResult CreateCategory([FromBody] CategoryPayload payload)
        {
            try
            {
                List<string> Categoryname = new List<string>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    foreach (var category in payload.categories)
                    {
                        string jsonData = JsonConvert.SerializeObject(category);

                        using (var command = new SqlCommand("sp_mst_Stock_Category_CRUD", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@jsonData", jsonData);

                            // Output parameter to capture inserted item names
                            SqlParameter outputParameter = new SqlParameter();
                            outputParameter.ParameterName = "@Category";
                            outputParameter.SqlDbType = SqlDbType.NVarChar;
                            outputParameter.Size = -1;
                            outputParameter.Direction = ParameterDirection.Output;
                            command.Parameters.Add(outputParameter);

                            command.ExecuteNonQuery();

                            // Retrieve inserted item name from output parameter
                            string insertedCategoryname = outputParameter.Value.ToString();

                            // Add inserted item name to the list
                            Categoryname.Add(insertedCategoryname);
                        }
                    }
                }

                List<Categoryoutput> categorylist = Categoryname.ConvertAll(category => new Categoryoutput { Category = category });

                var response = new ApiResponse3
                {
                    Success = true,
                    Message = "Stock Category created successfully!",
                    Categoryname = categorylist
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse3
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Categoryname = null
                };

                return StatusCode(500, response);
            }
        }


        [HttpGet("StockGroup")]
        public IActionResult GetStock()
        {
            try
            {
                List<GetStockGroup> stockgroup = new List<GetStockGroup>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("SELECT [ID],[Name],[ParentID],[Parent],[IsAddable],[FirstAlias],[HSNDescription],[HSNCode],[IntegratedTax],[CentralTax],[StateTax],[Cess],[LastModifiedBy],[Group_Level]  FROM [Universal].[dbo].[tbl_mst_Stock_Group]", connection))
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            stockgroup.Add(new GetStockGroup
                            {
                                ID = reader["ID"].ToString(),
                                Name = reader["Name"].ToString(),
                                parentID = reader["ParentID"].ToString(),
                                parent = reader["Parent"].ToString(),
                                firstAlias = reader["FirstAlias"].ToString(),
                                hsnDescription = reader["HSNDescription"].ToString(),
                                isAddable = (bool)reader["IsAddable"],
                                hsnCode = reader["HSNCode"].ToString(),
                                level = int.Parse(reader["Group_Level"].ToString()),
                                integratedTax = decimal.Parse(reader["IntegratedTax"].ToString()),
                                centralTax = decimal.Parse(reader["CentralTax"].ToString()),
                                stateTax = decimal.Parse(reader["StateTax"].ToString()),
                                cess = decimal.Parse(reader["Cess"].ToString()),
                                lastUpdatedBy = reader["LastModifiedBy"].ToString(),
                            });
                        }
                    }
                }

                return Ok(stockgroup);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("StockGroup/MultipleGroup")]
        public IActionResult CreateGroup([FromBody] GroupPayload payload)
        {
            try
            {
                List<string> Groupname = new List<string>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    foreach (var group in payload.groups)
                    {
                        string jsonData = JsonConvert.SerializeObject(group);

                        using (var command = new SqlCommand("sp_mst_Stock_Group_CRUD", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@jsonData", jsonData);

                            // Output parameter to capture inserted item names
                            SqlParameter outputParameter = new SqlParameter();
                            outputParameter.ParameterName = "@Group";
                            outputParameter.SqlDbType = SqlDbType.NVarChar;
                            outputParameter.Size = -1;
                            outputParameter.Direction = ParameterDirection.Output;
                            command.Parameters.Add(outputParameter);

                            command.ExecuteNonQuery();

                            // Retrieve inserted item name from output parameter
                            string insertedGroupname = outputParameter.Value.ToString();

                            // Add inserted item name to the list
                            Groupname.Add(insertedGroupname);
                        }
                    }
                }

                List<Groupoutput> groupList = Groupname.ConvertAll(group => new Groupoutput { Group = group });

                var response = new ApiResponse4
                {
                    Success = true,
                    Message = "Group created successfully!",
                    Groupname = groupList
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse4
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Groupname = null
                };

                return StatusCode(500, response);
            }
        }

        [HttpPost("StockItem/AddMultipleItems")]
        public IActionResult InsertOrUpdatePersonal([FromBody] StockItemPayload payload)
        {
            try
            {
                List<string> executedInformation = new List<string>();
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("sp_mst_Stock_Item_CRUD", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue(
                            "@jsonData",
                            JsonConvert.SerializeObject(payload)
                        );

                        // Define output parameter to capture inserted invoice numbers
                        SqlParameter outputParameter = new SqlParameter();
                        outputParameter.ParameterName = "@executedMessage";
                        outputParameter.SqlDbType = SqlDbType.NVarChar;
                        outputParameter.Size = -1;
                        outputParameter.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputParameter);

                        using (var reader = command.ExecuteReader())
                        {
                            // Process the success message if needed
                        }

                        // Retrieve inserted invoice numbers from output parameter
                        string executedInformationString = command
                            .Parameters["@executedMessage"]
                            .Value.ToString();

                        // Convert comma-separated string to list of inserted invoice numbers
                        executedInformation = new List<string>(
                            executedInformationString.Split(',')
                        );
                    }
                }

                // Create list of InsertedInvoice objects
                List<InsertedInfo> insertedInvoiceList = executedInformation.ConvertAll(
                    inv => new InsertedInfo { msg = inv }
                );

                var response = new ApiResponse5
                {
                    Success = true,
                    Message = "Records Created successfully!",
                    returnedInfo = insertedInvoiceList
                };

                return Ok(response);

                //return Task.FromResult<IActionResult>(Ok("Data inserted or updated successfully."));
            }
            catch (Exception ex)
            {
                var response = new ApiResponse5
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    returnedInfo = null
                };

                return StatusCode(500, response);
            }
        }

        // [HttpGet("GetItems")]
        // public IActionResult StockItem()
        // {
        //     try
        //     {
        //         List<GetStockItem> getstockitem = new List<GetStockItem>();

        //         using (var connection = new SqlConnection(_config.GetConnectionString("ApiConn")))
        //         {
        //             connection.Open();
        //             using (var command = new SqlCommand("SELECT [Item_ID],[Name],[Parent],[Category],[BaseUnits]  FROM [tbl_mst_Stock_Item]", connection))
        //             using (var reader = command.ExecuteReader())
        //             {
        //                 while (reader.Read())
        //                 {
        //                     getstockitem.Add(new GetStockItem
        //                     {
        //                         item_ID = reader["Item_ID"].ToString(),
        //                         name = reader["Name"].ToString(),
        //                         parent = reader["Parent"].ToString(),
        //                         category = reader["Category"].ToString(),
        //                         baseUnits = reader["BaseUnits"].ToString(),
        //                     });
        //                 }
        //             }
        //         }

        //         return Ok(getstockitem);
        //     }
        //     catch (Exception ex)
        //     {
        //         return StatusCode(500, $"Error: {ex.Message}");
        //     }
        // }

        [HttpGet("StockItem/GetItems")]
        public IActionResult GetStockItems(string groupId = null)
        {
            try
            {
                List<GetStockItem> getStockItems = new List<GetStockItem>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Construct the SQL query with conditional WHERE clause based on groupId
                    string sqlQuery =
                        @"
                SELECT SI.[Item_ID], SI.[Name], SI.[Parent], SI.[Category], SI.[BaseUnits]
                FROM tbl_mst_Stock_Item SI";

                    // Add WHERE clause based on groupId parameter
                    if (!string.IsNullOrEmpty(groupId))
                    {
                        sqlQuery += " WHERE SI.[ParentID] = @GroupId";
                    }

                    using (var command = new SqlCommand(sqlQuery, connection))
                    {
                        // Add the groupId parameter to the command if it's provided
                        if (!string.IsNullOrEmpty(groupId))
                        {
                            command.Parameters.AddWithValue("@GroupId", groupId);
                        }

                        // Execute the command and read results
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                getStockItems.Add(
                                    new GetStockItem
                                    {
                                        item_ID = reader["Item_ID"].ToString(),
                                        name = reader["Name"].ToString(),
                                        parent = reader["Parent"].ToString(),
                                        category = reader["Category"].ToString(),
                                        baseUnits = reader["BaseUnits"].ToString(),
                                    }
                                );
                            }
                        }
                    }
                }

                return Ok(getStockItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpPost("StockItem/GetByStockItem")]
        public IActionResult GetItems([FromBody] Reportitem irequest)
        {
            try
            {
                List<GetStockItem> ilist_Gets = new List<GetStockItem>();

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand command = new SqlCommand("sp_get_Stockitem_filter", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;


                        string item = string.IsNullOrWhiteSpace(irequest.item) ? "NA" : irequest.item;
                        string group = string.IsNullOrWhiteSpace(irequest.group) ? "NA" : irequest.group;
                        string category = string.IsNullOrWhiteSpace(irequest.category) ? "NA" : irequest.category;


                        command.Parameters.AddWithValue("@item", item);
                        command.Parameters.AddWithValue("@group", group);
                        command.Parameters.AddWithValue("@category", category);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                GetStockItem idata_get = new GetStockItem
                                {
                                    item_ID = reader["Item_ID"].ToString(),
                                    name = reader["Name"].ToString(),
                                    parent = reader["Parent"].ToString(),
                                    category = reader["Category"].ToString(),
                                    baseUnits = reader["BaseUnits"].ToString(),
                                };
                                ilist_Gets.Add(idata_get);
                            }
                        }
                    }
                }

                return Ok(ilist_Gets);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("StockBalance/Multiplebalance")]
        public IActionResult CreateCredit([FromBody] CreditPayload payload)
        {
            try
            {
                List<string> Creditname = new List<string>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    foreach (var credit in payload.data)
                    {
                        string jsonData = JsonConvert.SerializeObject(credit);

                        using (var command = new SqlCommand("[sp_mst_Stock_Creditbalance_CRUD]", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@jsonData", jsonData);

                            SqlParameter outputParameter = new SqlParameter();
                            outputParameter.ParameterName = "@Credit";
                            outputParameter.SqlDbType = SqlDbType.NVarChar;
                            outputParameter.Size = -1;
                            outputParameter.Direction = ParameterDirection.Output;
                            command.Parameters.Add(outputParameter);

                            command.ExecuteNonQuery();

                            string insertedCreditname = outputParameter.Value.ToString();
                            Creditname.Add(insertedCreditname);
                        }
                    }
                }

                List<Creditoutput> creditList = Creditname.ConvertAll(credit => new Creditoutput { Credit = credit });

                var response = new ApiResponse6
                {
                    Success = true,
                    Message = "Date inserted successfully!",
                    Creditname = creditList
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                var response = new ApiResponse6
                {
                    Success = false,
                    Message = $"Error: {ex.Message}",
                    Creditname = null
                };

                return StatusCode(500, response);
            }
        }

    }
}
