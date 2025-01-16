using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace Dugros_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SyncController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public SyncController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class PurchaseBill
        {
         
           // public int countPurchaseBill { get; set; }
            public Guid PurchaseBillTrnId { get; set; }
            public string PurchaseBillTallyTrnId { get; set; }
            public string VoucherTypeId { get; set; }
            public string VoucherType { get; set; }
            public string VoucherNo { get; set; }
            public string OrderNo { get; set; }
            public DateTime TransactionDate { get; set; }
            public string SupplierInvoice { get; set; }
            public DateTime SupplierInvDate { get; set; }
            public string SupplierId { get; set; }
            public string SupplierName { get; set; }
            public string FrmBillingAddress { get; set; }
            public Guid FrmStateId { get; set; }
            public string FrmState { get; set; }
            public Guid FrmCountryId { get; set; }
            public string FrmCountry { get; set; }
            public string FrmPin { get; set; }
            public string FrmGstRegType { get; set; }
            public string FrmGstUinNo { get; set; }
            public string ShipperName { get; set; }
            public string FrmShippingAddress { get; set; }
            public string ShpState { get; set; }
            public Guid ShpStateId { get; set; }
            public string ShpCountry { get; set; }
            public Guid ShpCountryId { get; set; }
            public string ShpPin { get; set; }
            public string ShpGstRegType { get; set; }
            public string ShpGstUinNo { get; set; }
            public string PlaceOfSupply { get; set; }
            public string CostCenter { get; set; }
            public Guid CostCenterId { get; set; }
            public string Narration { get; set; }
            public string TypeOfReference { get; set; }
            public string Name { get; set; }
            public DateTime DueDate { get; set; }
            public decimal TotalGrossAmt { get; set; }
            public decimal TaxableAmt { get; set; }
            public decimal TaxAmt { get; set; }
            public decimal TotalBillAmt { get; set; }
            public decimal RoundingOff { get; set; }
            public decimal NetBillAmt { get; set; }
            public Guid StatusId { get; set; }
            public string Status { get; set; }
            public string PlaceReceiptShipper { get; set; }
            public string VesselFlightNo { get; set; }
            public string PortOfLoading { get; set; }
            public string PortOfDischarge { get; set; }
            public string CountryTo { get; set; }
            public string PortCode { get; set; }
            public string CarrierName { get; set; }
            public string RrLrNo { get; set; }
            public string ModeTransPayment { get; set; }
            public string OtherReference { get; set; }
            public string TermsDelivery { get; set; }
            public string GrnDespatchThrough { get; set; }
            public string GrnDestination { get; set; }
            public string WarehouseId { get; set; }
            public bool ChequeApplicable { get; set; }
            public string ApprovalStatusAbbr { get; set; }
            public Guid CreatedBy { get; set; }
            public DateTime CreatedDate { get; set; }
            public bool IsImported { get; set; }
            public bool IsActive { get; set; }
            public decimal ItemTotal { get; set; }
            public decimal OtherChargesTotal { get; set; }
            public Guid BillAddressId { get; set; }
            public DateTime TallySyncTime { get; set; }
        }


        public class PurchaseBillCount
        {

            public int countPurchaseBill { get; set; }
           
        }

        public class StockItemData
        {
            public string ItemId { get; set; }
            public string Name { get; set; }
            public string ParentId { get; set; }
            public string Parent { get; set; }
            public string CategoryId { get; set; }
            public string Category { get; set; }
            public string MailingName { get; set; }
            public string Narration { get; set; }
            public string TCSApplicable { get; set; }
            public string GSTApplicable { get; set; }
            public string Description { get; set; }
            public string GSTTypeOfSupply { get; set; }
            public string CostingMethod { get; set; }
            public string ValuationMethod { get; set; }
            public string BaseUnitId { get; set; }
            public string BaseUnits { get; set; }
            public string AdditionalUnitId { get; set; }
            public string AdditionalUnits { get; set; }
            public bool? IsBatchWiseOn { get; set; }
            public bool? IsPerishableOn { get; set; }
            public bool? IgnoreNegativeStock { get; set; }
            public bool? TreatSalesAsManufactured { get; set; }
            public bool? TreatPurchasesAsConsumed { get; set; }
            public bool? TreatRejectsAsScrap { get; set; }
            public bool? HasMfgDate { get; set; }
            public bool? AllowUseOfExpiredItems { get; set; }
            public bool? InclusiveTax { get; set; }
            public decimal? Denominator { get; set; }
            public decimal? Conversion { get; set; }
            public decimal? OpeningBalance { get; set; }
            public decimal? OpeningValue { get; set; }
            public decimal? ReorderBase { get; set; }
            public decimal? MinimumOrderBase { get; set; }
            public decimal? OpeningRate { get; set; }
            public string FirstAlias { get; set; }
            public string HSNDescription { get; set; }
            public string HSNCode { get; set; }
            public decimal? IntegratedTax { get; set; }
            public decimal? CentralTax { get; set; }
            public decimal? StateTax { get; set; }
            public decimal? Cess { get; set; }
            public bool? ApplicableForReverseCharge { get; set; }
            public string LastUpdatedBy { get; set; }
        }


        public class StockGodown
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string ParentID { get; set; }
            public string ParentName { get; set; }
            public bool HasNoSpace { get; set; }
            public bool IsExternal { get; set; }
            public bool IsInternal { get; set; }
            public string LastUpdatedBy { get; set; }
            public string Alias { get; set; }
            public string GroupLevel { get; set; }
            public DateTime? tallysynctime { get; set; }
        }


        [HttpGet("GetPurchaseBillsSummerySync")]
        public IActionResult GetPurchaseBillsSummerySync([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            try
            {
                // If no fromDate is provided, use the current date
                DateTime dateParam = fromDate ?? DateTime.Now;

                List<PurchaseBill> purchaseBills = new List<PurchaseBill>();

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_Get_Purchase_Bill_Summary_Tally_Sync_Date]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add the 'from_date' parameter to the command
                        command.Parameters.AddWithValue("@from_date", (object)fromDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@to_date", (object)toDate ?? DBNull.Value);

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var purchaseBill = new PurchaseBill
                                {
                                    PurchaseBillTrnId = !reader.IsDBNull(reader.GetOrdinal("purchase_Bill_trn_id")) ? reader.GetGuid(reader.GetOrdinal("purchase_Bill_trn_id")) : Guid.Empty,
                                    PurchaseBillTallyTrnId = reader["purchase_bill_tally_trn_id"] != DBNull.Value ? reader["purchase_bill_tally_trn_id"].ToString() : string.Empty,
                                    VoucherTypeId = reader["voucher_type_id"] != DBNull.Value ? reader["voucher_type_id"].ToString() : string.Empty,
                                    VoucherType = reader["voucher_type"] != DBNull.Value ? reader["voucher_type"].ToString() : string.Empty,
                                    VoucherNo = reader["voucher_no"] != DBNull.Value ? reader["voucher_no"].ToString() : string.Empty,
                                    OrderNo = reader["order_no"] != DBNull.Value ? reader["order_no"].ToString() : string.Empty,
                                    TransactionDate = !reader.IsDBNull(reader.GetOrdinal("transaction_date")) ? reader.GetDateTime(reader.GetOrdinal("transaction_date")) : DateTime.MinValue,
                                    SupplierInvoice = reader["supplier_invoice"] != DBNull.Value ? reader["supplier_invoice"].ToString() : string.Empty,
                                    SupplierInvDate = !reader.IsDBNull(reader.GetOrdinal("supplier_inv_date")) ? reader.GetDateTime(reader.GetOrdinal("supplier_inv_date")) : DateTime.MinValue,
                                    SupplierId = reader["supplier_id"] != DBNull.Value ? reader["supplier_id"].ToString() : string.Empty,
                                    SupplierName = reader["supplier_name"] != DBNull.Value ? reader["supplier_name"].ToString() : string.Empty,
                                    FrmBillingAddress = reader["frm_billing_address"] != DBNull.Value ? reader["frm_billing_address"].ToString() : string.Empty,
                                    FrmStateId = !reader.IsDBNull(reader.GetOrdinal("frm_state_id")) ? reader.GetGuid(reader.GetOrdinal("frm_state_id")) : Guid.Empty,
                                    FrmState = reader["frm_state"] != DBNull.Value ? reader["frm_state"].ToString() : string.Empty,
                                    FrmCountryId = !reader.IsDBNull(reader.GetOrdinal("frm_country_id")) ? reader.GetGuid(reader.GetOrdinal("frm_country_id")) : Guid.Empty,
                                    FrmCountry = reader["frm_country"] != DBNull.Value ? reader["frm_country"].ToString() : string.Empty,
                                    FrmPin = reader["frm_pin"] != DBNull.Value ? reader["frm_pin"].ToString() : string.Empty,
                                    FrmGstRegType = reader["frm_gst_reg_type"] != DBNull.Value ? reader["frm_gst_reg_type"].ToString() : string.Empty,
                                    FrmGstUinNo = reader["frm_gst_uin_no"] != DBNull.Value ? reader["frm_gst_uin_no"].ToString() : string.Empty,
                                    ShipperName = reader["shipper_name"] != DBNull.Value ? reader["shipper_name"].ToString() : string.Empty,
                                    FrmShippingAddress = reader["frm_shipping_address"] != DBNull.Value ? reader["frm_shipping_address"].ToString() : string.Empty,
                                    ShpState = reader["shp_state"] != DBNull.Value ? reader["shp_state"].ToString() : string.Empty,
                                    ShpStateId = !reader.IsDBNull(reader.GetOrdinal("shp_state_id")) ? reader.GetGuid(reader.GetOrdinal("shp_state_id")) : Guid.Empty,
                                    ShpCountry = reader["shp_country"] != DBNull.Value ? reader["shp_country"].ToString() : string.Empty,
                                    ShpCountryId = !reader.IsDBNull(reader.GetOrdinal("shp_country_id")) ? reader.GetGuid(reader.GetOrdinal("shp_country_id")) : Guid.Empty,
                                    ShpPin = reader["shp_pin"] != DBNull.Value ? reader["shp_pin"].ToString() : string.Empty,
                                    ShpGstRegType = reader["shp_gst_reg_type"] != DBNull.Value ? reader["shp_gst_reg_type"].ToString() : string.Empty,
                                    ShpGstUinNo = reader["shp_gst_uin_no"] != DBNull.Value ? reader["shp_gst_uin_no"].ToString() : string.Empty,
                                    PlaceOfSupply = reader["place_of_supply"] != DBNull.Value ? reader["place_of_supply"].ToString() : string.Empty,
                                    CostCenter = reader["cost_center"] != DBNull.Value ? reader["cost_center"].ToString() : string.Empty,
                                    CostCenterId = !reader.IsDBNull(reader.GetOrdinal("cost_center_id")) ? reader.GetGuid(reader.GetOrdinal("cost_center_id")) : Guid.Empty,
                                    Narration = reader["narration"] != DBNull.Value ? reader["narration"].ToString() : string.Empty,
                                    TypeOfReference = reader["type_of_reference"] != DBNull.Value ? reader["type_of_reference"].ToString() : string.Empty,
                                    Name = reader["name"] != DBNull.Value ? reader["name"].ToString() : string.Empty,
                                    DueDate = !reader.IsDBNull(reader.GetOrdinal("due_date")) ? reader.GetDateTime(reader.GetOrdinal("due_date")) : DateTime.MinValue,
                                    TotalGrossAmt = !reader.IsDBNull(reader.GetOrdinal("total_gross_amt")) ? reader.GetDecimal(reader.GetOrdinal("total_gross_amt")) : 0,
                                    TaxableAmt = !reader.IsDBNull(reader.GetOrdinal("taxable_amt")) ? reader.GetDecimal(reader.GetOrdinal("taxable_amt")) : 0,
                                    TaxAmt = !reader.IsDBNull(reader.GetOrdinal("tax_amt")) ? reader.GetDecimal(reader.GetOrdinal("tax_amt")) : 0,
                                    TotalBillAmt = !reader.IsDBNull(reader.GetOrdinal("total_bill_amt")) ? reader.GetDecimal(reader.GetOrdinal("total_bill_amt")) : 0,
                                    RoundingOff = !reader.IsDBNull(reader.GetOrdinal("rounding_off")) ? reader.GetDecimal(reader.GetOrdinal("rounding_off")) : 0,
                                    NetBillAmt = !reader.IsDBNull(reader.GetOrdinal("net_bill_amt")) ? reader.GetDecimal(reader.GetOrdinal("net_bill_amt")) : 0,
                                    StatusId = !reader.IsDBNull(reader.GetOrdinal("status_id")) ? reader.GetGuid(reader.GetOrdinal("status_id")) : Guid.Empty,
                                    Status = reader["status"] != DBNull.Value ? reader["status"].ToString() : string.Empty,
                                    PlaceReceiptShipper = reader["place_receipt_shipper"] != DBNull.Value ? reader["place_receipt_shipper"].ToString() : string.Empty,
                                    VesselFlightNo = reader["vessel_flight_no"] != DBNull.Value ? reader["vessel_flight_no"].ToString() : string.Empty,
                                    PortOfLoading = reader["port_of_loading"] != DBNull.Value ? reader["port_of_loading"].ToString() : string.Empty,
                                    PortOfDischarge = reader["port_of_discharge"] != DBNull.Value ? reader["port_of_discharge"].ToString() : string.Empty,
                                    CountryTo = reader["country_to"] != DBNull.Value ? reader["country_to"].ToString() : string.Empty,
                                    PortCode = reader["port_code"] != DBNull.Value ? reader["port_code"].ToString() : string.Empty,
                                    CarrierName = reader["carrier_name"] != DBNull.Value ? reader["carrier_name"].ToString() : string.Empty,
                                    RrLrNo = reader["rr_lr_no"] != DBNull.Value ? reader["rr_lr_no"].ToString() : string.Empty,
                                    ModeTransPayment = reader["mode_trans_payment"] != DBNull.Value ? reader["mode_trans_payment"].ToString() : string.Empty,
                                    OtherReference = reader["other_reference"] != DBNull.Value ? reader["other_reference"].ToString() : string.Empty,
                                    TermsDelivery = reader["terms_delivery"] != DBNull.Value ? reader["terms_delivery"].ToString() : string.Empty,
                                    GrnDespatchThrough = reader["grn_despatch_through"] != DBNull.Value ? reader["grn_despatch_through"].ToString() : string.Empty,
                                    GrnDestination = reader["grn_destination"] != DBNull.Value ? reader["grn_destination"].ToString() : string.Empty,
                                    WarehouseId = reader["warehouse_id"] != DBNull.Value ? reader["warehouse_id"].ToString() : string.Empty,
                                    ChequeApplicable = !reader.IsDBNull(reader.GetOrdinal("cheque_applicable")) ? reader.GetBoolean(reader.GetOrdinal("cheque_applicable")) : false,
                                    ApprovalStatusAbbr = reader["approval_status_abbr"] != DBNull.Value ? reader["approval_status_abbr"].ToString() : string.Empty,
                                    CreatedBy = !reader.IsDBNull(reader.GetOrdinal("created_by")) ? reader.GetGuid(reader.GetOrdinal("created_by")) : Guid.Empty,
                                    CreatedDate = !reader.IsDBNull(reader.GetOrdinal("created_date")) ? reader.GetDateTime(reader.GetOrdinal("created_date")) : DateTime.MinValue,
                                    IsImported = !reader.IsDBNull(reader.GetOrdinal("is_imported")) ? reader.GetBoolean(reader.GetOrdinal("is_imported")) : false,
                                    IsActive = !reader.IsDBNull(reader.GetOrdinal("is_active")) ? reader.GetBoolean(reader.GetOrdinal("is_active")) : false,
                                    ItemTotal = !reader.IsDBNull(reader.GetOrdinal("item_total")) ? reader.GetDecimal(reader.GetOrdinal("item_total")) : 0,
                                    OtherChargesTotal = !reader.IsDBNull(reader.GetOrdinal("other_charges_total")) ? reader.GetDecimal(reader.GetOrdinal("other_charges_total")) : 0,
                                    BillAddressId = !reader.IsDBNull(reader.GetOrdinal("bill_address_id")) ? reader.GetGuid(reader.GetOrdinal("bill_address_id")) : Guid.Empty,
                                    TallySyncTime = !reader.IsDBNull(reader.GetOrdinal("tally_sync_time")) ? reader.GetDateTime(reader.GetOrdinal("tally_sync_time")) : DateTime.MinValue,
                                };

                                purchaseBills.Add(purchaseBill);
                            }
                        }
                    }
                }

                return Ok(purchaseBills);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the data.", Details = ex.Message });
            }
        }

        [HttpGet("GetCountPurchaseBillsSync")]
        public IActionResult GetCountPurchaseBillsSync([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            try
            {
                // Default to today's date if fromDate is not provided
                DateTime dateParam = fromDate ?? DateTime.Now;

                // Initialize the PurchaseBillCount object
                PurchaseBillCount purchaseBill = new PurchaseBillCount();

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_Get_Count_Purchase_Bill_Summary_Tally_Sync_Date]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add the 'from_date' parameter
                        //command.Parameters.Add(new SqlParameter("@from_date", SqlDbType.Date) { Value = dateParam });
                        command.Parameters.AddWithValue("@from_date", (object)fromDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@to_date", (object)toDate ?? DBNull.Value);

                        connection.Open();

                        // Execute the command and fetch the count
                        object result = command.ExecuteScalar();

                        // Set the countPurchaseBill property
                        purchaseBill.countPurchaseBill = result != null ? Convert.ToInt32(result) : 0;
                    }
                }

                return Ok(purchaseBill);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the count.", Details = ex.Message });
            }
        }

        [HttpGet("GetStockItems")]
        public IActionResult GetStockItems([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            try
            {
                DateTime dateParam = fromDate ?? DateTime.Now;

                List<StockItemData> stockItems = new List<StockItemData>();

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_GetStockItemData]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        //command.Parameters.Add(new SqlParameter("@from_date", SqlDbType.Date) { Value = dateParam });
                        command.Parameters.AddWithValue("@from_date", (object)fromDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@to_date", (object)toDate ?? DBNull.Value);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var stockItem = new StockItemData
                                {
                                    ItemId = reader["Item_ID"] != DBNull.Value ? reader["Item_ID"].ToString() : string.Empty,
                                    Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : string.Empty,
                                    ParentId = reader["ParentId"] != DBNull.Value ? reader["ParentId"].ToString() : string.Empty,
                                    Parent = reader["Parent"] != DBNull.Value ? reader["Parent"].ToString() : string.Empty,
                                    CategoryId = reader["CategoryId"] != DBNull.Value ? reader["CategoryId"].ToString() : string.Empty,
                                    Category = reader["Category"] != DBNull.Value ? reader["Category"].ToString() : string.Empty,
                                    MailingName = reader["MailingName"] != DBNull.Value ? reader["MailingName"].ToString() : string.Empty,
                                    Narration = reader["Narration"] != DBNull.Value ? reader["Narration"].ToString() : string.Empty,
                                    TCSApplicable = reader["TCSApplicable"] != DBNull.Value ? reader["TCSApplicable"].ToString() : string.Empty,
                                    GSTApplicable = reader["GSTApplicable"] != DBNull.Value ? reader["GSTApplicable"].ToString() : string.Empty,
                                    Description = reader["Description"] != DBNull.Value ? reader["Description"].ToString() : string.Empty,
                                    GSTTypeOfSupply = reader["GSTTypeOfSupply"] != DBNull.Value ? reader["GSTTypeOfSupply"].ToString() : string.Empty,
                                    CostingMethod = reader["CostingMethod"] != DBNull.Value ? reader["CostingMethod"].ToString() : string.Empty,
                                    ValuationMethod = reader["ValuationMethod"] != DBNull.Value ? reader["ValuationMethod"].ToString() : string.Empty,
                                    BaseUnitId = reader["Base_Unit_ID"] != DBNull.Value ? reader["Base_Unit_ID"].ToString() : string.Empty,
                                    BaseUnits = reader["BaseUnits"] != DBNull.Value ? reader["BaseUnits"].ToString() : string.Empty,
                                    AdditionalUnitId = reader["Additional_Unit_ID"] != DBNull.Value ? reader["Additional_Unit_ID"].ToString() : string.Empty,
                                    AdditionalUnits = reader["AdditionalUnits"] != DBNull.Value ? reader["AdditionalUnits"].ToString() : string.Empty,
                                    IsBatchWiseOn = !reader.IsDBNull(reader.GetOrdinal("IsBatchWiseOn")) ? reader.GetBoolean(reader.GetOrdinal("IsBatchWiseOn")) : (bool?)null,
                                    IsPerishableOn = !reader.IsDBNull(reader.GetOrdinal("IsPerishableOn")) ? reader.GetBoolean(reader.GetOrdinal("IsPerishableOn")) : (bool?)null,
                                    IgnoreNegativeStock = !reader.IsDBNull(reader.GetOrdinal("IgnoreNegativeStock")) ? reader.GetBoolean(reader.GetOrdinal("IgnoreNegativeStock")) : (bool?)null,
                                    TreatSalesAsManufactured = !reader.IsDBNull(reader.GetOrdinal("TreatSalesAsManufactured")) ? reader.GetBoolean(reader.GetOrdinal("TreatSalesAsManufactured")) : (bool?)null,
                                    TreatPurchasesAsConsumed = !reader.IsDBNull(reader.GetOrdinal("TreatPurchasesAsConsumed")) ? reader.GetBoolean(reader.GetOrdinal("TreatPurchasesAsConsumed")) : (bool?)null,
                                    TreatRejectsAsScrap = !reader.IsDBNull(reader.GetOrdinal("TreatRejectsAsScrap")) ? reader.GetBoolean(reader.GetOrdinal("TreatRejectsAsScrap")) : (bool?)null,
                                    HasMfgDate = !reader.IsDBNull(reader.GetOrdinal("HasMfgDate")) ? reader.GetBoolean(reader.GetOrdinal("HasMfgDate")) : (bool?)null,
                                    AllowUseOfExpiredItems = !reader.IsDBNull(reader.GetOrdinal("AllowUseOfExpiredItems")) ? reader.GetBoolean(reader.GetOrdinal("AllowUseOfExpiredItems")) : (bool?)null,
                                    InclusiveTax = !reader.IsDBNull(reader.GetOrdinal("InclusiveTax")) ? reader.GetBoolean(reader.GetOrdinal("InclusiveTax")) : (bool?)null,
                                    Denominator = !reader.IsDBNull(reader.GetOrdinal("Denominator")) ? reader.GetDecimal(reader.GetOrdinal("Denominator")) : (decimal?)null,
                                    Conversion = !reader.IsDBNull(reader.GetOrdinal("Conversion")) ? reader.GetDecimal(reader.GetOrdinal("Conversion")) : (decimal?)null,
                                    OpeningBalance = !reader.IsDBNull(reader.GetOrdinal("OpeningBalance")) ? reader.GetDecimal(reader.GetOrdinal("OpeningBalance")) : (decimal?)null,
                                    OpeningValue = !reader.IsDBNull(reader.GetOrdinal("OpeningValue")) ? reader.GetDecimal(reader.GetOrdinal("OpeningValue")) : (decimal?)null,
                                    ReorderBase = !reader.IsDBNull(reader.GetOrdinal("ReorderBase")) ? reader.GetDecimal(reader.GetOrdinal("ReorderBase")) : (decimal?)null,
                                    MinimumOrderBase = !reader.IsDBNull(reader.GetOrdinal("MinimumOrderBase")) ? reader.GetDecimal(reader.GetOrdinal("MinimumOrderBase")) : (decimal?)null,
                                    OpeningRate = !reader.IsDBNull(reader.GetOrdinal("OpeningRate")) ? reader.GetDecimal(reader.GetOrdinal("OpeningRate")) : (decimal?)null,
                                    FirstAlias = reader["FirstAlias"] != DBNull.Value ? reader["FirstAlias"].ToString() : string.Empty,
                                    HSNDescription = reader["HSNDescription"] != DBNull.Value ? reader["HSNDescription"].ToString() : string.Empty,
                                    HSNCode = reader["HSNCode"] != DBNull.Value ? reader["HSNCode"].ToString() : string.Empty,
                                    IntegratedTax = !reader.IsDBNull(reader.GetOrdinal("IntegratedTax")) ? reader.GetDecimal(reader.GetOrdinal("IntegratedTax")) : (decimal?)null,
                                    CentralTax = !reader.IsDBNull(reader.GetOrdinal("CentralTax")) ? reader.GetDecimal(reader.GetOrdinal("CentralTax")) : (decimal?)null,
                                    StateTax = !reader.IsDBNull(reader.GetOrdinal("StateTax")) ? reader.GetDecimal(reader.GetOrdinal("StateTax")) : (decimal?)null,
                                    Cess = !reader.IsDBNull(reader.GetOrdinal("Cess")) ? reader.GetDecimal(reader.GetOrdinal("Cess")) : (decimal?)null,
                                    ApplicableForReverseCharge = !reader.IsDBNull(reader.GetOrdinal("ApplicableForReverseCharge")) ? reader.GetBoolean(reader.GetOrdinal("ApplicableForReverseCharge")) : (bool?)null,
                                    LastUpdatedBy = reader["LastUpdatedBy"] != DBNull.Value ? reader["LastUpdatedBy"].ToString() : string.Empty
                                };

                                stockItems.Add(stockItem);
                            }
                        }
                    }
                }

                return Ok(stockItems);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching stock item data.", Details = ex.Message });
            }
        }


        [HttpGet("GetCountStockItems")]
        public IActionResult GetCountStockItems([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            try
            {
                // Default to today's date if fromDate is not provided
                DateTime dateParam = fromDate ?? DateTime.Now;

                // Initialize the PurchaseBillCount object
                PurchaseBillCount purchaseBill = new PurchaseBillCount();

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_Get_Count_Stock_Item_Data]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add the 'from_date' parameter
                        //command.Parameters.Add(new SqlParameter("@from_date", SqlDbType.Date) { Value = dateParam });
                        command.Parameters.AddWithValue("@from_date", (object)fromDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@to_date", (object)toDate ?? DBNull.Value);

                        connection.Open();

                        // Execute the command and fetch the count
                        object result = command.ExecuteScalar();

                        // Set the countPurchaseBill property
                        purchaseBill.countPurchaseBill = result != null ? Convert.ToInt32(result) : 0;
                    }
                }

                return Ok(purchaseBill);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the count.", Details = ex.Message });
            }
        }


        [HttpGet("GetCountStockGodown")]
        public IActionResult GetCountStockGodown([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            try
            {
                // Default to today's date if fromDate is not provided
                DateTime dateParam = fromDate ?? DateTime.Now;

                // Initialize the PurchaseBillCount object
                PurchaseBillCount purchaseBill = new PurchaseBillCount();

                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_Get_Count_Stock_Godown_Data]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add the 'from_date' parameter
                        //command.Parameters.Add(new SqlParameter("@from_date", SqlDbType.Date) { Value = dateParam });
                        command.Parameters.AddWithValue("@from_date", (object)fromDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@to_date", (object)toDate ?? DBNull.Value);

                        connection.Open();

                        // Execute the command and fetch the count
                        object result = command.ExecuteScalar();

                        // Set the countPurchaseBill property
                        purchaseBill.countPurchaseBill = result != null ? Convert.ToInt32(result) : 0;
                    }
                }

                return Ok(purchaseBill);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "An error occurred while fetching the count.", Details = ex.Message });
            }
        }

        [HttpGet("GetStockGodown")]
        public IActionResult GetStockGodown([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate)
        {
            try
            {
                // Default the date parameter to today's date if not supplied
                DateTime dateParam = fromDate ?? DateTime.Now;

                // Create a list to hold the stock godown data
                List<StockGodown> stockgo = new List<StockGodown>();

                // Establish a connection to the database
                using (SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    // Prepare the stored procedure command
                    using (SqlCommand command = new SqlCommand("[dbo].[sp_GetStockGodownData]", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add the date parameter
                        //command.Parameters.Add(new SqlParameter("@from_date", SqlDbType.Date) { Value = dateParam });
                        command.Parameters.AddWithValue("@from_date", (object)fromDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@to_date", (object)toDate ?? DBNull.Value);

                        // Open the database connection
                        connection.Open();

                        // Execute the command and read the results
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Map the database fields to the StockGodown object
                                var stockg = new StockGodown
                                {
                                    ID = reader["ID"] != DBNull.Value ? reader["ID"].ToString() : string.Empty,
                                    Name = reader["Name"] != DBNull.Value ? reader["Name"].ToString() : string.Empty,
                                    Address = reader["Address"] != DBNull.Value ? reader["Address"].ToString() : string.Empty,
                                    ParentID = reader["ParentID"] != DBNull.Value ? reader["ParentID"].ToString() : string.Empty,
                                    ParentName = reader["ParentName"] != DBNull.Value ? reader["ParentName"].ToString() : string.Empty,
                                    HasNoSpace = reader["HasNoSpace"] != DBNull.Value && Convert.ToBoolean(reader["HasNoSpace"]),
                                    IsExternal = reader["IsExternal"] != DBNull.Value && Convert.ToBoolean(reader["IsExternal"]),
                                    IsInternal = reader["IsInternal"] != DBNull.Value && Convert.ToBoolean(reader["IsInternal"]),
                                    LastUpdatedBy = reader["LastUpdatedBy"] != DBNull.Value ? reader["LastUpdatedBy"].ToString() : string.Empty,
                                    Alias = reader["Alias"] != DBNull.Value ? reader["Alias"].ToString() : string.Empty,
                                    GroupLevel = reader["GroupLevel"] != DBNull.Value ? reader["GroupLevel"].ToString() : string.Empty,
                                    tallysynctime = reader["tally_sync_time"] != DBNull.Value ? Convert.ToDateTime(reader["tally_sync_time"]) : (DateTime?)null
                                };

                                // Add the mapped object to the list
                                stockgo.Add(stockg);
                            }
                        }
                    }
                }

                // Return the list of stock godown data as the response
                return Ok(stockgo);
            }
            catch (Exception ex)
            {
                // Return a 500 status code with the error message
                return StatusCode(500, new { Message = "An error occurred while fetching stock godown data.", Details = ex.Message });
            }
        }


    }
}
