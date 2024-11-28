using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;

namespace Dugros_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TallyIntegrationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TallyIntegrationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class GetPO
        {
            public Guid purchase_Bill_trn_id { get; set; }
            public Guid voucher_id { get; set; }
            public string voucher_type { get; set; }
            public string doc_no { get; set; }
            public string order_no { get; set; }
            public DateTime doc_date { get; set; }
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
            public DateTime po_due_date { get; set; }
            public List<getitems> item_details { get; set; }
            public List<getterms> term_details { get; set; }
            //public string    bill_address { get; set; }
            //public string    ship_address { get; set; }
            public string remarks { get; set; }
            public string warehouse_id { get; set; }
            public string warehouse_name { get; set; }
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
        public class getterms
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
        [HttpGet("GetPurchaseBill")]
        public IActionResult GetPurchaseOrders()
        {
            try
            {
                List<GetPO> purchaseOrders = new List<GetPO>();
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.Tally_PurchaseBill_Get", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        //command.Parameters.AddWithValue("@user_id", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a new GetPO object
                                var purchaseOrder = new GetPO
                                {
                                    purchase_Bill_trn_id = reader.GetGuid(reader.GetOrdinal("purchase_Bill_trn_id")),
                                    voucher_id = reader.GetGuid(reader.GetOrdinal("voucher_id")),
                                    voucher_type = reader.GetString(reader.GetOrdinal("voucher_type")),
                                    doc_no = reader.GetString(reader.GetOrdinal("doc_no")),
                                    order_no = reader.GetString(reader.GetOrdinal("order_no")),
                                    doc_date = reader.GetDateTime(reader.GetOrdinal("doc_date")),
                                    vendor_id = reader.GetString(reader.GetOrdinal("vendor_id")),
                                    vendor_name = reader.GetString(reader.GetOrdinal("vendor_name")),

                                    vendor_ref_no = reader.GetString(reader.GetOrdinal("vendor_ref_no")),
                                    billing_location = reader.GetString(reader.GetOrdinal("billing_location")),
                                    bill_country = reader.GetString(reader.GetOrdinal("bill_country")),
                                    bill_state = reader.GetString(reader.GetOrdinal("bill_state")),
                                    bill_pin = reader.GetString(reader.GetOrdinal("bill_pin")),
                                    bill_gst_reg_type = reader["bill_gst_reg_type"].ToString(),
                                    bill_gst_uin_no = reader.GetString(reader.GetOrdinal("bill_gst_uin_no")),
                                    shipper_name = reader.GetString(reader.GetOrdinal("shipper_name")),
                                    shp_state = reader.GetString(reader.GetOrdinal("shp_state")),
                                    shp_country = reader.GetString(reader.GetOrdinal("shp_country")),
                                    shp_pin = reader.GetString(reader.GetOrdinal("shp_pin")),
                                    shp_gst_reg_type = reader["shp_gst_reg_type"].ToString(),
                                    shp_gst_uin_no = reader.GetString(reader.GetOrdinal("shp_gst_uin_no")),
                                    total_gross_amt = reader.GetDecimal(reader.GetOrdinal("total_gross_amt")),
                                    taxable_amt = reader.GetDecimal(reader.GetOrdinal("taxable_amt")),
                                    tax_amt = reader.GetDecimal(reader.GetOrdinal("tax_amt")),
                                    total_bill_amt = reader.GetDecimal(reader.GetOrdinal("total_bill_amt")),
                                    rounding_off = reader.GetDecimal(reader.GetOrdinal("rounding_off")),
                                    net_bill_amt = reader.GetDecimal(reader.GetOrdinal("net_bill_amt")),
                                    po_due_date = reader.GetDateTime(reader.GetOrdinal("po_due_date")),
                                    remarks = reader.GetString(reader.GetOrdinal("remarks")),
                                    warehouse_id = reader.GetString(reader.GetOrdinal("warehouse_id")),
                                    warehouse_name = reader.GetString(reader.GetOrdinal("warehouse_name")),
                                };

                                // Deserialize JSON properties
                                string itemDetailsJson = reader.GetString(reader.GetOrdinal("item_details"));
                                purchaseOrder.item_details = JsonConvert.DeserializeObject<List<getitems>>(itemDetailsJson);

                                string termsDetailsJson = reader.GetString(reader.GetOrdinal("term_details"));
                                purchaseOrder.term_details = JsonConvert.DeserializeObject<List<getterms>>(termsDetailsJson);


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
    }
}
