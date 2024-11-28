using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using static Dugros_Api.Controllers.PurchaseOrderController;
using System.Globalization;
using static Dugros_Api.Controllers.PurchaseOrderAllGet;
using Microsoft.Extensions.Logging;
using static Dugros_Api.Controllers.PurchaseIndentController;

namespace Dugros_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseBillController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PurchaseBillController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
      
        public class GetPB
        {
            public Guid purchase_Bill_trn_id { get; set; }
            public string voucher_id { get; set; }
            public string voucher_type { get; set; }
            public string doc_no { get; set; }
            public string order_no { get; set; }
            public DateOnly doc_date { get; set; }
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
            //public string shp_state { get; set; }
            //public string shp_country { get; set; }
            //public string shp_pin { get; set; }
            //public string shp_gst_reg_type { get; set; }
            //public string shp_gst_uin_no { get; set; }
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

            public List<fileUpload> files { get; set; }
            public decimal item_total { get; set; }
            public decimal other_charges_total { get; set; }
        }
        public class PostPB
        {
            public string voucher_id { get; set; }
            public string voucher_type { get; set; }
            //public string doc_no { get; set; }
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
            //public string shp_state { get; set; }
            ////public Guid shp_state_id { get; set; }
            //public string shp_pin { get; set; }
            //public string shp_gst_reg_type { get; set; }
            //public string shp_gst_uin_no { get; set; }
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
            public List<fileUpload> files { get; set; }
            public decimal item_total { get; set; }
            public decimal other_charges_total { get; set; }
            public Guid? bill_address_id { get; set; }
        }

        public class fileUpload
        {
            public string url_path { get; set; }
            public string filename { get; set; }
        }

        public class itemsPB
        {
            public Guid purchase_indent_id { get; set; }
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
            public DateTime? expected_arrival_date { get; set; }
            public string vehicle_no { get; set; }
            public string contact_no { get; set; }
            public string transportername { get; set; }
            public string receiptnotes { get; set; }

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
            //public string shp_state { get; set; }
            //public string shp_country { get; set; }
            //public string shp_pin { get; set; }
            //public string shp_gst_reg_type { get; set; }
            //public string shp_gst_uin_no { get; set; }
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
            //public string shp_state { get; set; }
            //public string shp_country { get; set; }
            //public string shp_pin { get; set; }
            //public string shp_gst_reg_type { get; set; }
            //public string shp_gst_uin_no { get; set; }
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
            public string po_no { get; set; }
        }
        public class Ledger
        {
            public string ledger_name { get; set; }
            public decimal amount { get; set; }
        }

        public class LOI
        {
            public Guid purchase_indent_id { get; set; }
            public string pi_no { get; set; }
            public Guid purchase_Order_trn_id { get; set; }
            public string doc_no { get; set; }
            public string warehouse_id { get; set; }
            public string warehouse_name { get; set; }
            public string vendor_id { get; set; }
            public string vendor_name { get; set; }
            public string vendor_state { get; set; }
            public string item_id { get; set; }
            public string item_name { get; set; }
            public string item_desc { get; set; }
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
            //public string shp_state { get; set; }
            //public string shp_country { get; set; }
            //public string shp_pin { get; set; }
            //public string shp_gst_reg_type { get; set; }
            //public string shp_gst_uin_no { get; set; }
            public string shipper_address { get; set; }
            public decimal net_bill_amt { get; set; }
            public decimal pi_qty { get; set; }
            public string vendor_gst { get; set; }
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

      

        
        
        [HttpGet("GetPO_Selection")]
        public IActionResult GetPO_Select(string? vendor_id,string? warehouse_id)
        {
            try
            {

                List<GetPO_Selection> itemCategories = new List<GetPO_Selection>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PurchaseBill_PO_Selection", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@vendor_id", vendor_id);
                        command.Parameters.AddWithValue("@warehouse_id", warehouse_id);

                        // Add OUTPUT parameter to capture the stored procedure message
                        // var outputParam = new SqlParameter("@Message", SqlDbType.NVarChar, 1000);
                        // outputParam.Direction = ParameterDirection.Output;
                        // command.Parameters.Add(outputParam);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    GetPO_Selection color = new GetPO_Selection
                                    {
                                        purchase_order_id = reader["purchase_Order_trn_id"] != DBNull.Value ? (Guid)reader["purchase_Order_trn_id"] : Guid.Empty,
                                        voucher_no = reader["voucher_no"] != DBNull.Value ? reader["voucher_no"].ToString() : string.Empty,
                                        voucher_date = reader["voucher_date"] != DBNull.Value ? ((DateTime)reader["voucher_date"]).ToString("dd-MM-yyyy") : string.Empty,
                                        vendor_name = reader["partyname"] != DBNull.Value ? reader["partyname"].ToString() : string.Empty,
                                        vendor_id = reader["vendor_id"] != DBNull.Value ? reader["vendor_id"].ToString() : string.Empty,
                                        net_bill_amt = reader["net_bill_amt"] != DBNull.Value ? (decimal)reader["net_bill_amt"] : 0.0m,
                                        warehouse_id = reader["warehouse_id"] != DBNull.Value ? reader["warehouse_id"].ToString() : string.Empty,
                                        warehouse_name = reader["warehouse_name"] != DBNull.Value ? reader["warehouse_name"].ToString() : string.Empty,
                                        billing_location = reader["billing_location"] != DBNull.Value ? reader["billing_location"].ToString() : string.Empty,
                                        bill_state = reader["bill_state"] != DBNull.Value ? reader["bill_state"].ToString() : string.Empty,
                                        bill_country = reader["bill_country"] != DBNull.Value ? reader["bill_country"].ToString() : string.Empty,
                                        bill_pin = reader["bill_pin"] != DBNull.Value ? reader["bill_pin"].ToString() : string.Empty,
                                        bill_gst_reg_type = reader["bill_gst_reg_type"] != DBNull.Value ? reader["bill_gst_reg_type"].ToString() : string.Empty,
                                        bill_gst_uin_no = reader["bill_gst_uin_no"] != DBNull.Value ? reader["bill_gst_uin_no"].ToString() : string.Empty,
                                        shipper_name = reader["shipper_name"] != DBNull.Value ? reader["shipper_name"].ToString() : string.Empty,
                                        //shp_state = reader["shp_state"] != DBNull.Value ? reader["shp_state"].ToString() : string.Empty,
                                        //shp_country = reader["shp_country"] != DBNull.Value ? reader["shp_country"].ToString() : string.Empty,
                                        //shp_gst_reg_type = reader["shp_gst_reg_type"] != DBNull.Value ? reader["shp_gst_reg_type"].ToString() : string.Empty,
                                        //shp_gst_uin_no = reader["shp_gst_uin_no"] != DBNull.Value ? reader["shp_gst_uin_no"].ToString() : string.Empty,
                                        shipper_address = reader["shipper_address"] != DBNull.Value ? reader["shipper_address"].ToString() : string.Empty,
                                        //shp_pin = reader["shp_pin"] != DBNull.Value ? reader["shp_pin"].ToString() : string.Empty,
                                        item_id = reader["item_id"] != DBNull.Value ? reader["item_id"].ToString() : string.Empty,
                                        item_name = reader["item_name"] != DBNull.Value ? reader["item_name"].ToString() : string.Empty,
                                        pending_qty = reader["pending_qty"] != DBNull.Value ? (decimal)reader["pending_qty"] : 0.0m,
                                    };

                                    itemCategories.Add(color);
                                }
                            }

                        }
                    }
                }


                if (itemCategories.Any())
                {
                    return Ok(itemCategories);
                }
                else
                {
                    return NotFound("No items found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        //[HttpPost("GetPO_LOI")]
        //public IActionResult GetPurchaseOrders(multipletrn trn_ids)
        //{
        //    try
        //    {
        //        List<LOI> purchaseOrders = new List<LOI>();

        //        using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
        //        {
        //            connection.Open();
        //            using (var command = new SqlCommand("dbo.PurchaseBill_LOI", connection))
        //            {
        //                command.CommandType = CommandType.StoredProcedure;
        //                command.Parameters.AddWithValue("@trn_ids", trn_ids.trnids);

        //                using (var reader = command.ExecuteReader())
        //                {
        //                    while (reader.Read())
        //                    {
        //                        // Create a new GetPO object
        //                        var purchaseOrder = new LOI
        //                        {

        //                            doc_no = reader.IsDBNull(reader.GetOrdinal("doc_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("doc_no")),
        //                            item_name = reader.IsDBNull(reader.GetOrdinal("item_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("item_name")),
        //                            item_id = reader.IsDBNull(reader.GetOrdinal("item_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("item_id")),
        //                            po_qty = reader.IsDBNull(reader.GetOrdinal("po_qty")) ? 0 : reader.GetDecimal(reader.GetOrdinal("po_qty")),
        //                            uom_primary = reader.IsDBNull(reader.GetOrdinal("uom_primary")) ? string.Empty : reader.GetString(reader.GetOrdinal("uom_primary")),
        //                            uom_primary_name = reader.IsDBNull(reader.GetOrdinal("uom_primary_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("uom_primary_name")),
        //                            rate = reader.IsDBNull(reader.GetOrdinal("rate")) ? 0 : reader.GetDecimal(reader.GetOrdinal("rate")),
        //                            discount_ratio = reader.IsDBNull(reader.GetOrdinal("discount_ratio")) ? 0 : reader.GetDecimal(reader.GetOrdinal("discount_ratio")),
        //                            discount_amt = reader.IsDBNull(reader.GetOrdinal("discount_amt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("discount_amt")),
        //                            amt_before_dis = reader.IsDBNull(reader.GetOrdinal("amt_before_dis")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amt_before_dis")),
        //                            amt_after_dis = reader.IsDBNull(reader.GetOrdinal("amt_after_dis")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amt_after_dis")),
        //                            mrp = reader.IsDBNull(reader.GetOrdinal("mrp")) ? 0 : reader.GetDecimal(reader.GetOrdinal("mrp")),
        //                            tax_type = reader.IsDBNull(reader.GetOrdinal("tax_type")) ? string.Empty : reader.GetString(reader.GetOrdinal("tax_type")),
        //                            amt_sgst = reader.IsDBNull(reader.GetOrdinal("amt_sgst")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amt_sgst")),
        //                            amt_cgst = reader.IsDBNull(reader.GetOrdinal("amt_cgst")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amt_cgst")),
        //                            amt_igst = reader.IsDBNull(reader.GetOrdinal("amt_igst")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amt_igst")),
        //                            amt_total = reader.IsDBNull(reader.GetOrdinal("amt_total")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amt_total")),
        //                            sgst_rate = reader.IsDBNull(reader.GetOrdinal("sgst_rate")) ? 0 : reader.GetDecimal(reader.GetOrdinal("sgst_rate")),
        //                            cgst_rate = reader.IsDBNull(reader.GetOrdinal("cgst_rate")) ? 0 : reader.GetDecimal(reader.GetOrdinal("cgst_rate")),
        //                            igst_rate = reader.IsDBNull(reader.GetOrdinal("igst_rate")) ? 0 : reader.GetDecimal(reader.GetOrdinal("igst_rate")),
        //                            base_reference_no = reader.IsDBNull(reader.GetOrdinal("base_reference_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("base_reference_no")),
        //                            base_reference_date = reader["base_reference_date"] != DBNull.Value ? ((DateTime)reader["base_reference_date"]).ToString("yyyy-MM-dd") : string.Empty,
        //                            base_reference_type = reader.IsDBNull(reader.GetOrdinal("base_reference_type")) ? string.Empty : reader.GetString(reader.GetOrdinal("base_reference_type")),
        //                            pending_qty = reader.IsDBNull(reader.GetOrdinal("pending_qty")) ? 0 : reader.GetDecimal(reader.GetOrdinal("pending_qty"))
        //                        };


        //                        // Add the purchase order to the list
        //                        purchaseOrders.Add(purchaseOrder);
        //                    }
        //                }
        //            }
        //        }

        //        // Return the list of purchase orders
        //        return Ok(purchaseOrders);
        //    }
        //    catch (Exception ex)
        //    {
        //        // Return error message
        //        return StatusCode(500, $"Error: {ex.Message}");
        //    }
        //}

        [HttpPost("GetPO_LOI")]
        public IActionResult GetPurchaseOrders(filter_selection filter)
        {
            try
            {
                List<LOI> purchaseOrders = new List<LOI>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.PurchaseBill_LineOfItem", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", filter.user_id);
                        command.Parameters.AddWithValue("@warehouse_id", string.IsNullOrEmpty(filter.warehouse_id) ? (object)DBNull.Value : filter.warehouse_id);
                        command.Parameters.AddWithValue("@vendor_id", string.IsNullOrEmpty(filter.vendor_id) ? (object)DBNull.Value : filter.vendor_id);
                        command.Parameters.AddWithValue("@item_name", string.IsNullOrEmpty(filter.item_name) ? (object)DBNull.Value : filter.item_name);
                        command.Parameters.AddWithValue("@po_no", string.IsNullOrEmpty(filter.po_no) ? (object)DBNull.Value : filter.po_no);


                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a new GetPO object
                                var purchaseOrder = new LOI
                                {
                                    purchase_indent_id = (Guid)reader["purchase_indent_id"],
                                    pi_no = reader["pi_no"].ToString(),
                                    purchase_Order_trn_id = (Guid)reader["purchase_Order_trn_id"],
                                    doc_no = reader.IsDBNull(reader.GetOrdinal("doc_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("doc_no")),
                                    item_name = reader.IsDBNull(reader.GetOrdinal("item_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("item_name")),
                                    item_id = reader.IsDBNull(reader.GetOrdinal("item_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("item_id")),
                                    item_desc = reader["item_desc"].ToString(),
                                    warehouse_name = reader.IsDBNull(reader.GetOrdinal("warehouse_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("warehouse_name")),
                                    warehouse_id = reader.IsDBNull(reader.GetOrdinal("warehouse_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("warehouse_id")),
                                    vendor_name = reader.IsDBNull(reader.GetOrdinal("vendor_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("vendor_name")),
                                    vendor_id = reader.IsDBNull(reader.GetOrdinal("vendor_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("vendor_id")),
                                    //po_qty = reader.IsDBNull(reader.GetOrdinal("po_qty")) ? 0 : reader.GetDecimal(reader.GetOrdinal("po_qty")),
                                    uom_primary = reader.IsDBNull(reader.GetOrdinal("uom_primary")) ? string.Empty : reader.GetString(reader.GetOrdinal("uom_primary")),
                                    uom_primary_name = reader.IsDBNull(reader.GetOrdinal("uom_primary_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("uom_primary_name")),
                                    rate = reader.IsDBNull(reader.GetOrdinal("rate")) ? 0 : reader.GetDecimal(reader.GetOrdinal("rate")),
                                    discount_ratio = reader.IsDBNull(reader.GetOrdinal("discount_ratio")) ? 0 : reader.GetDecimal(reader.GetOrdinal("discount_ratio")),
                                    discount_amt = reader.IsDBNull(reader.GetOrdinal("discount_amt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("discount_amt")),
                                    amt_before_dis = reader.IsDBNull(reader.GetOrdinal("amt_before_dis")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amt_before_dis")),
                                    amt_after_dis = reader.IsDBNull(reader.GetOrdinal("amt_after_dis")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amt_after_dis")),
                                    mrp = reader.IsDBNull(reader.GetOrdinal("mrp")) ? 0 : reader.GetDecimal(reader.GetOrdinal("mrp")),
                                    tax_type = reader.IsDBNull(reader.GetOrdinal("tax_type")) ? string.Empty : reader.GetString(reader.GetOrdinal("tax_type")),
                                    amt_sgst = reader.IsDBNull(reader.GetOrdinal("amt_sgst")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amt_sgst")),
                                    amt_cgst = reader.IsDBNull(reader.GetOrdinal("amt_cgst")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amt_cgst")),
                                    amt_igst = reader.IsDBNull(reader.GetOrdinal("amt_igst")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amt_igst")),
                                    amt_total = reader.IsDBNull(reader.GetOrdinal("amt_total")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amt_total")),
                                    sgst_rate = reader.IsDBNull(reader.GetOrdinal("sgst_rate")) ? 0 : reader.GetDecimal(reader.GetOrdinal("sgst_rate")),
                                    cgst_rate = reader.IsDBNull(reader.GetOrdinal("cgst_rate")) ? 0 : reader.GetDecimal(reader.GetOrdinal("cgst_rate")),
                                    igst_rate = reader.IsDBNull(reader.GetOrdinal("igst_rate")) ? 0 : reader.GetDecimal(reader.GetOrdinal("igst_rate")),
                                    base_reference_no = reader.IsDBNull(reader.GetOrdinal("base_reference_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("base_reference_no")),
                                    base_reference_date = reader["base_reference_date"] != DBNull.Value ? ((DateTime)reader["base_reference_date"]).ToString("yyyy-MM-dd") : string.Empty,
                                    base_reference_type = reader.IsDBNull(reader.GetOrdinal("base_reference_type")) ? string.Empty : reader.GetString(reader.GetOrdinal("base_reference_type")),
                                    pending_qty = reader.IsDBNull(reader.GetOrdinal("pending_qty")) ? 0 : reader.GetDecimal(reader.GetOrdinal("pending_qty")),
                                    net_bill_amt = reader.IsDBNull(reader.GetOrdinal("net_bill_amt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("net_bill_amt")),


                                     billing_location = reader["billing_location"] != DBNull.Value ? reader["billing_location"].ToString() : string.Empty,
                                    bill_state = reader["bill_state"] != DBNull.Value ? reader["bill_state"].ToString() : string.Empty,
                                    vendor_state = reader["state"] != DBNull.Value ? reader["state"].ToString() : string.Empty,
                                    bill_country = reader["bill_country"] != DBNull.Value ? reader["bill_country"].ToString() : string.Empty,
                                    bill_pin = reader["bill_pin"] != DBNull.Value ? reader["bill_pin"].ToString() : string.Empty,
                                    bill_gst_reg_type = reader["bill_gst_reg_type"] != DBNull.Value ? reader["bill_gst_reg_type"].ToString() : string.Empty,
                                    bill_gst_uin_no = reader["bill_gst_uin_no"] != DBNull.Value ? reader["bill_gst_uin_no"].ToString() : string.Empty,
                                    shipper_name = reader["shipper_name"] != DBNull.Value ? reader["shipper_name"].ToString() : string.Empty,
                                    //shp_state = reader["shp_state"] != DBNull.Value ? reader["shp_state"].ToString() : string.Empty,
                                    //shp_country = reader["shp_country"] != DBNull.Value ? reader["shp_country"].ToString() : string.Empty,
                                    //shp_gst_reg_type = reader["shp_gst_reg_type"] != DBNull.Value ? reader["shp_gst_reg_type"].ToString() : string.Empty,
                                    //shp_gst_uin_no = reader["shp_gst_uin_no"] != DBNull.Value ? reader["shp_gst_uin_no"].ToString() : string.Empty,
                                    shipper_address = reader["shipper_address"] != DBNull.Value ? reader["shipper_address"].ToString() : string.Empty,
                                    //shp_pin = reader["shp_pin"] != DBNull.Value ? reader["shp_pin"].ToString() : string.Empty,
                                    pi_qty = reader.IsDBNull(reader.GetOrdinal("pi_qty")) ? 0 : reader.GetDecimal(reader.GetOrdinal("pi_qty")),
                                    vendor_gst = reader["gst_no"].ToString()
                                };


                                // Add the purchase order to the list
                                purchaseOrders.Add(purchaseOrder);
                            }
                        }
                    }
                }

                // Return the list of purchase orders
                return Ok(purchaseOrders);
            }
            catch (Exception ex)
            {
                // Return error message
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("GetPO_Terms")]
        public IActionResult GetPurchaseOrders_terms(multipletrn trn_ids)
        {
            try
            {
                List<terms_bill> purchaseOrders = new List<terms_bill>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.PurchaseBill_Terms", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@trn_ids", trn_ids.trn_ids);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a new GetPO object
                                var purchaseOrder = new terms_bill
                                {

                                    
                                    gl_ledger_name = reader.IsDBNull(reader.GetOrdinal("gl_ledger_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("gl_ledger_name")),

                                    amt = reader.IsDBNull(reader.GetOrdinal("amt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amt")),
                                    amt_sgst = reader.IsDBNull(reader.GetOrdinal("amt_sgst")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amt_sgst")),
                                    amt_cgst = reader.IsDBNull(reader.GetOrdinal("amt_cgst")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amt_cgst")),
                                    amt_igst = reader.IsDBNull(reader.GetOrdinal("amt_igst")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amt_igst")),
                                    amt_total = reader.IsDBNull(reader.GetOrdinal("amt_total")) ? 0 : reader.GetDecimal(reader.GetOrdinal("amt_total")),
                                    sgst_rate = reader.IsDBNull(reader.GetOrdinal("sgst_rate")) ? 0 : reader.GetDecimal(reader.GetOrdinal("sgst_rate")),
                                    cgst_rate = reader.IsDBNull(reader.GetOrdinal("cgst_rate")) ? 0 : reader.GetDecimal(reader.GetOrdinal("cgst_rate")),
                                    igst_rate = reader.IsDBNull(reader.GetOrdinal("igst_rate")) ? 0 : reader.GetDecimal(reader.GetOrdinal("igst_rate")),
                                    tax_type = reader.IsDBNull(reader.GetOrdinal("tax_type")) ? string.Empty : reader.GetString(reader.GetOrdinal("tax_type")),
                                    
                                };

                                
                                // Add the purchase order to the list
                                purchaseOrders.Add(purchaseOrder);
                            }
                        }
                    }
                }

                // Return the list of purchase orders
                return Ok(purchaseOrders);
            }
            catch (Exception ex)
            {
                // Return error message
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpPost("PO/AddPurchaseBill")]
        public IActionResult AddPurchaseBill(PostPB postPO, Guid userId)
        {
            try
            {
                string message,message1,message2;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.PurchaseBill_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@voucher_id", postPO.voucher_id);
                        command.Parameters.AddWithValue("@voucher_type", postPO.voucher_type);
                        //command.Parameters.AddWithValue("@doc_no", postPO.doc_no);
                        command.Parameters.AddWithValue("@order_no", postPO.order_no);
                        command.Parameters.AddWithValue("@doc_date", postPO.doc_date);
                        command.Parameters.AddWithValue("@vendor_id", postPO.vendor_id);
                        command.Parameters.AddWithValue("@vendor_name", postPO.vendor_name);
                        command.Parameters.AddWithValue("@warehouse_id", postPO.warehouse_id);
                        command.Parameters.AddWithValue("@vendor_ref_no", postPO.vendor_ref_no);
                        command.Parameters.AddWithValue("@billing_location", postPO.billing_address);
                        command.Parameters.AddWithValue("@bill_state", postPO.bill_state);
                        command.Parameters.AddWithValue("@bill_pin", postPO.bill_pin);
                        command.Parameters.AddWithValue("@bill_gst_reg_type", postPO.bill_gst_reg_type);
                        command.Parameters.AddWithValue("@bill_gst_uin_no", postPO.bill_gst_uin_no);
                        command.Parameters.AddWithValue("@shipper_name", postPO.shipper_name);
                        command.Parameters.AddWithValue("@shipper_address", postPO.shipper_address);
                        //command.Parameters.AddWithValue("@shp_state", postPO.shp_state);
                        //command.Parameters.AddWithValue("@shp_pin", postPO.shp_pin);
                        //command.Parameters.AddWithValue("@shp_gst_reg_type", postPO.shp_gst_reg_type);
                        //command.Parameters.AddWithValue("@shp_gst_uin_no", postPO.shp_gst_uin_no);
                        command.Parameters.AddWithValue("@total_gross_amt", postPO.total_gross_amt);
                        command.Parameters.AddWithValue("@taxable_amt", postPO.taxable_amt);
                        command.Parameters.AddWithValue("@tax_amt", postPO.tax_amt);
                        command.Parameters.AddWithValue("@total_bill_amt", postPO.total_bill_amt);
                        command.Parameters.AddWithValue("@rounding_off", postPO.rounding_off);
                        command.Parameters.AddWithValue("@net_bill_amt", postPO.net_bill_amt);
                        command.Parameters.AddWithValue("@cheque_applicable", postPO.cheque_applicable);
                        command.Parameters.AddWithValue("@status", postPO.status);
                        command.Parameters.AddWithValue("@item_total", postPO.item_total);
                        command.Parameters.AddWithValue("@other_charges_total", postPO.other_charges_total);
                        command.Parameters.AddWithValue("@bill_address_id", postPO.bill_address_id);

                        string itemDetails = Newtonsoft.Json.JsonConvert.SerializeObject(postPO.item_details);
                        string termDetails = Newtonsoft.Json.JsonConvert.SerializeObject(postPO.term_details);
                        string fileDetails = Newtonsoft.Json.JsonConvert.SerializeObject(postPO.files);

                        string dispatchDetails = postPO.dispatch_details != null ?
                            Newtonsoft.Json.JsonConvert.SerializeObject(postPO.dispatch_details) :
                            null;

                        command.Parameters.AddWithValue("@item_details", itemDetails);
                        command.Parameters.AddWithValue("@term_details", termDetails);
                        command.Parameters.AddWithValue("@file_details", fileDetails);

                        if (string.IsNullOrEmpty(dispatchDetails))
                        {
                            command.Parameters.AddWithValue("@dispatch_details", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@dispatch_details", dispatchDetails);
                        }



                        command.Parameters.AddWithValue("@remarks", postPO.remarks);

                        // Add output parameter for the execute message
                        command.Parameters.Add("@executeMessage", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@voucherMessage", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@approvalMessage", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the output parameter value
                        message = command.Parameters["@executeMessage"].Value.ToString();
                        message1 = command.Parameters["@voucherMessage"].Value.ToString();
                        message2 = command.Parameters["@approvalMessage"].Value.ToString();
                    }

                    // Return success message
                    return Ok(new { executeMessage = message,voucherMessage=message1,approvalMessage=message2 });
                }
            }
            catch (Exception ex)
            {
                // Return error message
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpPost("PO/UpdatePurchaseBill")]
        public IActionResult UpdatePurchaseBill(Guid userId, Guid id, string status)
        {
            try
            {
                string message;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.PurchaseBill_Update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@purchase_Bill_trn_id", id); // Add the purchase order transaction ID for updating
                        command.Parameters.AddWithValue("@status", status);
                        command.Parameters.Add("@executeMessage", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                        command.ExecuteNonQuery();
                        message = command.Parameters["@executeMessage"].Value.ToString();
                    }
                    return Ok(new { ExecuteMessage = message });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpGet("PO/GetPurchaseBills")]
        public IActionResult GetPurchaseBills(Guid userId, string? warehouse_id, string? vendor_id, string? from_date, string? to_date, string? status)
        {
            try
            {
                List<GetPB> purchaseOrders = new List<GetPB>();

                // Parse date strings to DateTime objects, ensuring time part is set to midnight
                DateTime? fromDate = null;
                DateTime? toDate = null;

                if (!string.IsNullOrEmpty(from_date))
                {
                    fromDate = DateTime.ParseExact(from_date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }

                if (!string.IsNullOrEmpty(to_date))
                {
                    toDate = DateTime.ParseExact(to_date, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                }

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.PurchaseBill_Get", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@warehouse_id", warehouse_id ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@vendor_id", vendor_id ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@from_date", fromDate ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@to_date", toDate ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@status", status ?? (object)DBNull.Value);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a new GetPO object
                                var purchaseOrder = new GetPB
                                {
                                    purchase_Bill_trn_id = reader.IsDBNull(reader.GetOrdinal("purchase_Bill_trn_id")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("purchase_Bill_trn_id")),
                                    voucher_id = reader.IsDBNull(reader.GetOrdinal("voucher_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("voucher_id")),
                                    voucher_type = reader.IsDBNull(reader.GetOrdinal("voucher_type")) ? string.Empty : reader.GetString(reader.GetOrdinal("voucher_type")),
                                    doc_no = reader.IsDBNull(reader.GetOrdinal("doc_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("doc_no")),
                                    order_no = reader.IsDBNull(reader.GetOrdinal("order_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("order_no")),
                                    doc_date = reader.IsDBNull(reader.GetOrdinal("doc_date")) ? DateOnly.MinValue : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("doc_date"))),
                                    vendor_id = reader.IsDBNull(reader.GetOrdinal("vendor_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("vendor_id")),
                                    vendor_name = reader.IsDBNull(reader.GetOrdinal("vendor_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("vendor_name")),
                                    vendor_ref_no = reader.IsDBNull(reader.GetOrdinal("vendor_ref_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("vendor_ref_no")),
                                    billing_location = reader.IsDBNull(reader.GetOrdinal("billing_location")) ? string.Empty : reader.GetString(reader.GetOrdinal("billing_location")),
                                    bill_country = reader.IsDBNull(reader.GetOrdinal("bill_country")) ? string.Empty : reader.GetString(reader.GetOrdinal("bill_country")),
                                    bill_state = reader.IsDBNull(reader.GetOrdinal("bill_state")) ? string.Empty : reader.GetString(reader.GetOrdinal("bill_state")),
                                    bill_pin = reader.IsDBNull(reader.GetOrdinal("bill_pin")) ? string.Empty : reader.GetString(reader.GetOrdinal("bill_pin")),
                                    bill_gst_reg_type = reader["bill_gst_reg_type"] == DBNull.Value ? string.Empty : reader["bill_gst_reg_type"].ToString(),
                                    bill_gst_uin_no = reader.IsDBNull(reader.GetOrdinal("bill_gst_uin_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("bill_gst_uin_no")),
                                    shipper_name = reader.IsDBNull(reader.GetOrdinal("shipper_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("shipper_name")),
                                    shipper_address = reader.IsDBNull(reader.GetOrdinal("shipper_address")) ? string.Empty : reader.GetString(reader.GetOrdinal("shipper_address")),
                                    //shp_state = reader.IsDBNull(reader.GetOrdinal("shp_state")) ? string.Empty : reader.GetString(reader.GetOrdinal("shp_state")),
                                    //shp_country = reader.IsDBNull(reader.GetOrdinal("shp_country")) ? string.Empty : reader.GetString(reader.GetOrdinal("shp_country")),
                                    //shp_pin = reader.IsDBNull(reader.GetOrdinal("shp_pin")) ? string.Empty : reader.GetString(reader.GetOrdinal("shp_pin")),
                                    //shp_gst_reg_type = reader["shp_gst_reg_type"] == DBNull.Value ? string.Empty : reader["shp_gst_reg_type"].ToString(),
                                    //shp_gst_uin_no = reader.IsDBNull(reader.GetOrdinal("shp_gst_uin_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("shp_gst_uin_no")),
                                    total_gross_amt = reader.IsDBNull(reader.GetOrdinal("total_gross_amt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("total_gross_amt")),
                                    taxable_amt = reader.IsDBNull(reader.GetOrdinal("taxable_amt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("taxable_amt")),
                                    tax_amt = reader.IsDBNull(reader.GetOrdinal("tax_amt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("tax_amt")),
                                    total_bill_amt = reader.IsDBNull(reader.GetOrdinal("total_bill_amt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("total_bill_amt")),
                                    rounding_off = reader.IsDBNull(reader.GetOrdinal("rounding_off")) ? 0 : reader.GetDecimal(reader.GetOrdinal("rounding_off")),
                                    net_bill_amt = reader.IsDBNull(reader.GetOrdinal("net_bill_amt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("net_bill_amt")),
                                    //po_due_date = reader.IsDBNull(reader.GetOrdinal("po_due_date")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("po_due_date")),
                                    remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? string.Empty : reader.GetString(reader.GetOrdinal("remarks")),
                                    warehouse_id = reader.IsDBNull(reader.GetOrdinal("warehouse_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("warehouse_id")),
                                    warehouse_name = reader.IsDBNull(reader.GetOrdinal("warehouse_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("warehouse_name")),
                                    approval_status = reader.IsDBNull(reader.GetOrdinal("app_status_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("app_status_name")),
                                    status = reader.IsDBNull(reader.GetOrdinal("status")) ? string.Empty : reader.GetString(reader.GetOrdinal("status"))
                                };

                                // Deserialize JSON properties
                                string itemDetailsJson = reader.GetString(reader.GetOrdinal("item_details"));
                                purchaseOrder.item_details = JsonConvert.DeserializeObject<List<getitemsPB>>(itemDetailsJson);

                                string termsDetailsJson = reader.GetString(reader.GetOrdinal("term_details"));
                                purchaseOrder.term_details = JsonConvert.DeserializeObject<List<gettermsPB>>(termsDetailsJson);

                                // Add the purchase order to the list
                                purchaseOrders.Add(purchaseOrder);
                            }
                        }
                    }
                }

                // Return the list of purchase orders
                return Ok(purchaseOrders);
            }
            catch (Exception ex)
            {
                // Return error message
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("PO/GetPurchaseBills/{id}")]
        public IActionResult GetPurchaseBillsId(Guid userId, Guid id)
        {
            try
            {
                List<GetPB> purchaseOrders = new List<GetPB>();
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.PurchaseBill_GetById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@trn_id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a new GetPO object
                                var purchaseOrder = new GetPB
                                {
                                    purchase_Bill_trn_id = reader.IsDBNull(reader.GetOrdinal("purchase_Bill_trn_id")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("purchase_Bill_trn_id")),
                                    voucher_id = reader.IsDBNull(reader.GetOrdinal("voucher_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("voucher_id")),
                                    voucher_type = reader.IsDBNull(reader.GetOrdinal("voucher_type")) ? string.Empty : reader.GetString(reader.GetOrdinal("voucher_type")),
                                    doc_no = reader.IsDBNull(reader.GetOrdinal("doc_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("doc_no")),
                                    order_no = reader.IsDBNull(reader.GetOrdinal("order_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("order_no")),
                                    doc_date = reader.IsDBNull(reader.GetOrdinal("doc_date")) ? DateOnly.MinValue : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("doc_date"))),
                                    vendor_id = reader.IsDBNull(reader.GetOrdinal("vendor_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("vendor_id")),
                                    vendor_name = reader.IsDBNull(reader.GetOrdinal("vendor_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("vendor_name")),
                                    vendor_ref_no = reader.IsDBNull(reader.GetOrdinal("vendor_ref_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("vendor_ref_no")),
                                    billing_location = reader.IsDBNull(reader.GetOrdinal("billing_location")) ? string.Empty : reader.GetString(reader.GetOrdinal("billing_location")),
                                    bill_country = reader.IsDBNull(reader.GetOrdinal("bill_country")) ? string.Empty : reader.GetString(reader.GetOrdinal("bill_country")),
                                    bill_state = reader.IsDBNull(reader.GetOrdinal("bill_state")) ? string.Empty : reader.GetString(reader.GetOrdinal("bill_state")),
                                    bill_pin = reader.IsDBNull(reader.GetOrdinal("bill_pin")) ? string.Empty : reader.GetString(reader.GetOrdinal("bill_pin")),
                                    bill_gst_reg_type = reader["bill_gst_reg_type"] == DBNull.Value ? string.Empty : reader["bill_gst_reg_type"].ToString(),
                                    bill_gst_uin_no = reader.IsDBNull(reader.GetOrdinal("bill_gst_uin_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("bill_gst_uin_no")),
                                    shipper_name = reader.IsDBNull(reader.GetOrdinal("shipper_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("shipper_name")),
                                    shipper_address = reader.IsDBNull(reader.GetOrdinal("shipper_address")) ? string.Empty : reader.GetString(reader.GetOrdinal("shipper_address")),
                                    //shp_state = reader.IsDBNull(reader.GetOrdinal("shp_state")) ? string.Empty : reader.GetString(reader.GetOrdinal("shp_state")),
                                    //shp_country = reader.IsDBNull(reader.GetOrdinal("shp_country")) ? string.Empty : reader.GetString(reader.GetOrdinal("shp_country")),
                                    //shp_pin = reader.IsDBNull(reader.GetOrdinal("shp_pin")) ? string.Empty : reader.GetString(reader.GetOrdinal("shp_pin")),
                                    //shp_gst_reg_type = reader["shp_gst_reg_type"] == DBNull.Value ? string.Empty : reader["shp_gst_reg_type"].ToString(),
                                    //shp_gst_uin_no = reader.IsDBNull(reader.GetOrdinal("shp_gst_uin_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("shp_gst_uin_no")),
                                    total_gross_amt = reader.IsDBNull(reader.GetOrdinal("total_gross_amt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("total_gross_amt")),
                                    taxable_amt = reader.IsDBNull(reader.GetOrdinal("taxable_amt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("taxable_amt")),
                                    tax_amt = reader.IsDBNull(reader.GetOrdinal("tax_amt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("tax_amt")),
                                    total_bill_amt = reader.IsDBNull(reader.GetOrdinal("total_bill_amt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("total_bill_amt")),
                                    item_total = reader.IsDBNull(reader.GetOrdinal("item_total")) ? 0 : reader.GetDecimal(reader.GetOrdinal("item_total")),
                                    other_charges_total = reader.IsDBNull(reader.GetOrdinal("other_charges_total")) ? 0 : reader.GetDecimal(reader.GetOrdinal("other_charges_total")),
                                    rounding_off = reader.IsDBNull(reader.GetOrdinal("rounding_off")) ? 0 : reader.GetDecimal(reader.GetOrdinal("rounding_off")),
                                    net_bill_amt = reader.IsDBNull(reader.GetOrdinal("net_bill_amt")) ? 0 : reader.GetDecimal(reader.GetOrdinal("net_bill_amt")),
                                    //po_due_date = reader.IsDBNull(reader.GetOrdinal("po_due_date")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("po_due_date")),
                                    remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? string.Empty : reader.GetString(reader.GetOrdinal("remarks")),
                                    warehouse_id = reader.IsDBNull(reader.GetOrdinal("warehouse_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("warehouse_id")),
                                    warehouse_name = reader.IsDBNull(reader.GetOrdinal("warehouse_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("warehouse_name")),
                                    status = reader.IsDBNull(reader.GetOrdinal("status")) ? string.Empty : reader.GetString(reader.GetOrdinal("status")),
                                };

                                // Deserialize JSON properties
                                string itemDetailsJson = reader.GetString(reader.GetOrdinal("item_details"));
                                purchaseOrder.item_details = JsonConvert.DeserializeObject<List<getitemsPB>>(itemDetailsJson);

                                string termsDetailsJson = reader.GetString(reader.GetOrdinal("term_details"));
                                purchaseOrder.term_details = JsonConvert.DeserializeObject<List<gettermsPB>>(termsDetailsJson);
                                
                                string dispatch = reader.GetString(reader.GetOrdinal("dispatch_details"));
                                purchaseOrder.dispatch_Details = JsonConvert.DeserializeObject<List<dispatchPB>>(dispatch);


                                string files = reader.GetString(reader.GetOrdinal("file_details"));
                                purchaseOrder.files = JsonConvert.DeserializeObject<List<fileUpload>>(files);

                                // Add the purchase order to the list
                                purchaseOrders.Add(purchaseOrder);
                            }
                        }
                    }
                }

                // Return the list of purchase orders
                return Ok(purchaseOrders);
            }
            catch (Exception ex)
            {
                // Return error message
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpPut("delete/{purchase_bill_id}")]
        public IActionResult Delete_PurchaseIndent(Guid purchase_bill_id, [FromBody] DeletePurchaseIndent deletePurchaseIndent)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PurchaseBill_Delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", deletePurchaseIndent.user_id);
                        command.Parameters.AddWithValue("@purchase_bill_id", purchase_bill_id);

                        // Define output parameters for messages
                        var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 500)
                        {
                            Direction = ParameterDirection.Output
                        };
                        var errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 500)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(messageParam);
                        command.Parameters.Add(errorMessageParam);

                        command.ExecuteNonQuery();

                        string message = messageParam.Value?.ToString();
                        string errorMessage = errorMessageParam.Value?.ToString();

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            return StatusCode(400, errorMessage); // Return bad request with error message
                        }
                        else if (!string.IsNullOrEmpty(message))
                        {
                            return Ok(new { ExecuteMessage = message }); // Return success message
                        }
                        else
                        {
                            return StatusCode(500, "Error: No response from the database."); // No response from database
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}"); // Internal server error
            }
        }

    }
}
