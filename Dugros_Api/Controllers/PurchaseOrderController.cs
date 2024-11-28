using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data.SqlClient;
using System.Data;
using System.Reflection;
using System.Globalization;
using static Dugros_Api.Controllers.PurchaseIndentController;

namespace Dugros_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PurchaseOrderController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class ApproveFlow
        {
            //public Guid trn_id { get; set; }
            //public Guid user_id { get; set; }
            public string user_name { get; set; }
            public string activity { get; set; }
            // public Guid status { get; set; }
            public string approval_status { get; set; }
            public string remarks { get; set; }
            //public bool is_enable { get; set; }
        }

        public class GetPO
        {
            public Guid purchase_Order_trn_id { get; set; }
            public string voucher_id { get; set; }
            public string voucher_type { get; set; }
            public string doc_no { get; set; }
            public string order_no { get; set; }
            public DateOnly doc_date { get; set; }
            public string vendor_id { get; set; }
            public string vendor_name { get; set; }
            public string vendor_address { get; set; }
            public string vendor_state { get; set; }
            public string vendor_pin { get; set; }
            public string vendor_gst { get; set; }
            public string vendor_ref_no { get; set; }
            public string billing_location { get; set; }
            //public Guid bill_state_id { get; set; }
            public string bill_state { get; set; }
            public string bill_country { get; set; }
            public string bill_pin { get; set; }
            public string bill_gst_reg_type { get; set; }
            public string bill_gst_uin_no { get; set; }
            public string con_person { get; set; }
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
            public DateTime po_due_date { get; set; }
            public List<getitems> item_details { get; set; }
            public List<getterms> term_details { get; set; }
            public List<quotations> quotation_details { get; set; }
            //public string    bill_address { get; set; }
            //public string    ship_address { get; set; }
            public string remarks { get; set; }
            public string warehouse_id { get; set; }
            public string warehouse_name { get; set; }
            public string approval_status { get; set; }
            public Guid status_id { get; set; }
            public string status_name { get; set; }
            public int print_enable { get; set; }
            public int cheque_applicable { get; set; }
            public string t_c { get; set; }
            public DateTime? last_followup_date { get; set; }
            public DateTime? exp_delivery_date { get; set; }
            public string contact_person { get; set; }
            public string unit_name { get; set; }
            public string dev_inv { get; set; }
            public string office { get; set; }
            public string gst_no { get; set; }
            public string tag { get; set; }
            public decimal other_charges_total { get; set; }
            public decimal item_total { get; set; }
            public List<ApproveFlow> approving_flow { get; set; }
            public int cheque_issued { get; set; }
            public string last_approved_by { get; set; }
        }


        public class PostPO
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
            //public Guid shp_state_id { get; set; }
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
            // public DateTime po_due_date { get; set; }
            public List<items> item_details { get; set; }
            public List<terms> term_details { get; set; }
            public List<quotationspost>? quotation_details { get; set; }
            //public string    bill_address { get; set; }
            public string    shipper_address { get; set; }
            public string    remarks { get; set; }
            public bool cheque_applicable { get; set; }
            public string t_c { get; set; }
            public string? contact_person { get; set; }
            public decimal item_total { get; set; }
            public decimal other_charges_total { get; set; }
            public Guid? bill_address_id { get; set; }

        }

        public class items
        {
            public Guid purchase_indent_id { get; set; }
            public string item_name { get; set; }
            public string item_id { get; set; }
            public string? item_desc { get; set; }
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

            public List<delivery> delivery_schedule { get; set; }
            //public string voucher_no { get; set; }
            //public DateTime voucher_date { get; set; }
            //public decimal indent_qty { get; set; }
            //public decimal pending_qty { get; set; }
            //public string godown_id { get; set; }
            //public string godown_name { get; set; }


        }

        public class delivery
        {
            public string item_id { get; set; }
            public decimal qty { get; set; }
            public DateTime trn_dt { get; set; }
            public string base_reference_no { get; set; }
        }
        public class quotations
        {
            public string supplier_name { get; set; }
            public string product { get; set; }
            public string item_name { get; set; }
            public decimal quantity { get; set; }
            public string unit { get; set; }
            public string uom_name { get; set; }
            public decimal rate { get; set; }
            public decimal gst_rate { get; set; }
            public string remarks { get; set; }
        }
        public class quotationspost
        {
            public string supplier_name { get; set; }
            public string product { get; set; }
            public decimal quantity { get; set; }
            public string unit { get; set; }
            public decimal rate { get; set; }
            public decimal gst_rate { get; set; }
            public string remarks { get; set; }
        }
        public class terms
        {
            public string   gl_ledger_name { get; set; }
            public decimal  amt { get; set; }
            public string  tax_type { get; set; }
            public decimal gst_ratio { get; set; }
            public decimal  amt_cgst { get; set; }
            public decimal  amt_sgst { get; set; }
            public decimal  amt_igst { get; set; }
            public decimal  amt_total { get; set; }
            
        }

        public class getitems
        {
            public string item_name { get; set; }
            public string item_id { get; set; }
            public string item_desc { get; set; }
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
            public List<response_delivery_PI>? delivery_schedule { get; set; }
            //public string voucher_no { get; set; }
            //public DateTime voucher_date { get; set; }
            //public decimal indent_qty { get; set; }
            //public decimal pending_qty { get; set; }
            //public string warehouse_id { get; set; }
            //public string warehouse_name { get; set; }


        }
        public class response_delivery_PI
        {
            public Guid trn_id { get; set; }
            public string item_id { get; set; }
            public string item_name { get; set; }
            public decimal qty { get; set; }
            public DateOnly trn_dt { get; set; }
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
        public class pi_delivery
        {
            public decimal quantity { get; set; }
            public string  trn_dt { get; set; }
        }
        public class DeletePurchaseOrder
        {
            public Guid user_id { get; set; }
        }
        public class AttachmentDetail
        {
            public Guid purchase_indent_id { get; set; }
            public string pi_no { get; set; }
            public string url_path { get; set; }
            public string filename { get; set; }
        }

        public class PurchaseIndentRequest
        {
            public List<Guid> PurchaseIndentIds { get; set; }
        }

        [HttpPost("AddPurchaseOrder")]
        public IActionResult AddPurchaseOrder(PostPO postPO, Guid userId)
        {
            try
            {
                string message,message1,message2;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.PurchaseOrder_Insert", connection))
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
                        //command.Parameters.AddWithValue("@bill_state_id", postPO.bill_state_id);
                        command.Parameters.AddWithValue("@bill_state", postPO.bill_state);
                        command.Parameters.AddWithValue("@bill_pin", postPO.bill_pin);
                        command.Parameters.AddWithValue("@bill_gst_reg_type", postPO.bill_gst_reg_type);
                        command.Parameters.AddWithValue("@bill_gst_uin_no", postPO.bill_gst_uin_no);
                        command.Parameters.AddWithValue("@shipper_name", postPO.shipper_name);
                        command.Parameters.AddWithValue("@shipper_address", postPO.shipper_address);

                        //command.Parameters.AddWithValue("@shp_state", postPO.shp_state);
                        //command.Parameters.AddWithValue("@shp_state_id", postPO.shp_state_id);
                        //command.Parameters.AddWithValue("@shp_pin", postPO.shp_pin);
                        //command.Parameters.AddWithValue("@shp_gst_reg_type", postPO.shp_gst_reg_type);
                        //command.Parameters.AddWithValue("@shp_gst_uin_no", postPO.shp_gst_uin_no);
                        command.Parameters.AddWithValue("@total_gross_amt", postPO.total_gross_amt);
                        command.Parameters.AddWithValue("@taxable_amt", postPO.taxable_amt);
                        command.Parameters.AddWithValue("@tax_amt", postPO.tax_amt);
                        command.Parameters.AddWithValue("@total_bill_amt", postPO.total_bill_amt);
                        command.Parameters.AddWithValue("@rounding_off", postPO.rounding_off);
                        command.Parameters.AddWithValue("@net_bill_amt", postPO.net_bill_amt);
                        command.Parameters.AddWithValue("@contact_person", postPO.contact_person);



                        //command.Parameters.AddWithValue("@exp_bill_date", postPO.exp_bill_date);
                        // command.Parameters.AddWithValue("@po_due_date", postPO.po_due_date);
                        command.Parameters.AddWithValue("@cheque_applicable", postPO.cheque_applicable);

                        string itemDetails = Newtonsoft.Json.JsonConvert.SerializeObject(postPO.item_details);
                        string termDetails = Newtonsoft.Json.JsonConvert.SerializeObject(postPO.term_details);
                        string quotationDetails = Newtonsoft.Json.JsonConvert.SerializeObject(postPO.quotation_details);

                        command.Parameters.AddWithValue("@item_details", itemDetails);
                        command.Parameters.AddWithValue("@term_details", termDetails);
                        command.Parameters.AddWithValue("@quotation_details", quotationDetails);
                        command.Parameters.AddWithValue("@other_charges_total", postPO.other_charges_total);
                        command.Parameters.AddWithValue("@bill_address_id", postPO.bill_address_id);
                        command.Parameters.AddWithValue("@item_total", postPO.item_total);

                        //command.Parameters.AddWithValue("@bill_address", postPO.bill_address);
                        //command.Parameters.AddWithValue("@ship_address", postPO.ship_address);
                        command.Parameters.AddWithValue("@remarks", postPO.remarks);
                        command.Parameters.AddWithValue("@t_c", postPO.t_c);

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


        [HttpPost("UpdatePurchaseOrder")]
        public IActionResult UpdatePurchaseOrder(PostPO postPO, Guid userId,Guid id)
        {
            try
            {
                string message;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.PurchaseOrder_Update", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@purchase_Order_trn_id", id); // Add the purchase order transaction ID for updating
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
                        //command.Parameters.AddWithValue("@bill_state_id", postPO.bill_state_id);
                        command.Parameters.AddWithValue("@bill_state", postPO.bill_state);
                        command.Parameters.AddWithValue("@bill_pin", postPO.bill_pin);
                        command.Parameters.AddWithValue("@bill_gst_reg_type", postPO.bill_gst_reg_type);
                        command.Parameters.AddWithValue("@bill_gst_uin_no", postPO.bill_gst_uin_no);
                        command.Parameters.AddWithValue("@shipper_name", postPO.shipper_name);
                                command.Parameters.AddWithValue("@shipper_address", postPO.shipper_address);
                        //command.Parameters.AddWithValue("@shp_state", postPO.shp_state);
                        //command.Parameters.AddWithValue("@shp_state_id", postPO.shp_state_id);
                        //command.Parameters.AddWithValue("@shp_pin", postPO.shp_pin);
                        //command.Parameters.AddWithValue("@shp_gst_reg_type", postPO.shp_gst_reg_type);
                        //command.Parameters.AddWithValue("@shp_gst_uin_no", postPO.shp_gst_uin_no);
                        command.Parameters.AddWithValue("@total_gross_amt", postPO.total_gross_amt);
                        command.Parameters.AddWithValue("@taxable_amt", postPO.taxable_amt);
                        command.Parameters.AddWithValue("@tax_amt", postPO.tax_amt);
                        command.Parameters.AddWithValue("@total_bill_amt", postPO.total_bill_amt);
                        command.Parameters.AddWithValue("@rounding_off", postPO.rounding_off);
                        command.Parameters.AddWithValue("@net_bill_amt", postPO.net_bill_amt);
                        command.Parameters.AddWithValue("@contact_person", postPO.contact_person);
                        // command.Parameters.AddWithValue("@po_due_date", postPO.po_due_date);
                        string itemDetails = Newtonsoft.Json.JsonConvert.SerializeObject(postPO.item_details);
                        string termDetails = Newtonsoft.Json.JsonConvert.SerializeObject(postPO.term_details);
                        command.Parameters.AddWithValue("@item_details", itemDetails);
                        command.Parameters.AddWithValue("@term_details", termDetails);
                        command.Parameters.AddWithValue("@remarks", postPO.remarks);
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

        [HttpPost("attachments-by-purchase-indent")]
        public IActionResult GetExcelAttachments([FromBody] PurchaseIndentRequest request)
        {
            try
            {
                if (request.PurchaseIndentIds == null || !request.PurchaseIndentIds.Any())
                {
                    return BadRequest("Purchase Indent IDs are required.");
                }

                List<AttachmentDetail> attachmentDetails = new List<AttachmentDetail>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("ExcelAttachment_GetByPI", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Convert the list of GUIDs to a comma-separated string
                        string purchaseIndentIdsString = string.Join(",", request.PurchaseIndentIds);
                        command.Parameters.AddWithValue("@PurchaseIndentIds", purchaseIndentIdsString);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                attachmentDetails.Add(new AttachmentDetail
                                {
                                    purchase_indent_id = reader["purchase_indent_id"] != DBNull.Value ? (Guid)reader["purchase_indent_id"] : Guid.Empty,
                                    pi_no = reader["voucher_no"].ToString(),
                                    url_path = reader["url_path"] != DBNull.Value ? reader["url_path"].ToString() : null,
                                    filename = reader["filename"] != DBNull.Value ? reader["filename"].ToString() : null
                                });
                            }
                        }
                    }
                }

                if (attachmentDetails.Any())
                {
                    return Ok(attachmentDetails);
                }
                else
                {
                    return NotFound("No attachments found for the provided Purchase Indent IDs.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        [HttpGet("GetPurchaseOrders")]
        public IActionResult GetPurchaseOrders(Guid userId, string? warehouse_id, string? vendor_id, string? from_date, string? to_date, Guid? status_id, string? approval_status,string param,string? po_no)
        {
            try
            {
                List<GetPO> purchaseOrders = new List<GetPO>();

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
                    using (var command = new SqlCommand("dbo.PurchaseOrder_Get", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@status_id", status_id ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@warehouse_id", warehouse_id ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@approval_status", approval_status ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@vendor_id", vendor_id ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@from_date", fromDate ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@to_date", toDate ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@param", param);
                        command.Parameters.AddWithValue("@po_no", po_no ?? (object)DBNull.Value);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a new GetPO object
                                var purchaseOrder = new GetPO
                                {
                                    purchase_Order_trn_id = reader.IsDBNull(reader.GetOrdinal("purchase_Order_trn_id")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("purchase_Order_trn_id")),
                                    status_id = reader.IsDBNull(reader.GetOrdinal("status_id")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("status_id")),
                                    voucher_id = reader.IsDBNull(reader.GetOrdinal("voucher_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("voucher_id")),
                                    voucher_type = reader.IsDBNull(reader.GetOrdinal("voucher_type")) ? string.Empty : reader.GetString(reader.GetOrdinal("voucher_type")),
                                    status_name = reader.IsDBNull(reader.GetOrdinal("status_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("status_name")),
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
                                    po_due_date = reader.IsDBNull(reader.GetOrdinal("po_due_date")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("po_due_date")),
                                    remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? string.Empty : reader.GetString(reader.GetOrdinal("remarks")),
                                    contact_person = reader.IsDBNull(reader.GetOrdinal("contact_person")) ? string.Empty : reader.GetString(reader.GetOrdinal("contact_person")),
                                    t_c = reader.IsDBNull(reader.GetOrdinal("t_c")) ? string.Empty : reader.GetString(reader.GetOrdinal("t_c")),
                                    warehouse_id = reader.IsDBNull(reader.GetOrdinal("warehouse_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("warehouse_id")),
                                    warehouse_name = reader.IsDBNull(reader.GetOrdinal("warehouse_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("warehouse_name")),
                                    approval_status = reader.IsDBNull(reader.GetOrdinal("app_status_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("app_status_name")),
                                    print_enable = Convert.ToInt32(reader["print_enable"]),
                                    cheque_applicable = Convert.ToInt32(reader["cheque_applicable"]),
                                    last_followup_date = reader["last_followup_date"] != DBNull.Value ? (DateTime)reader["last_followup_date"] : (DateTime?)null,
                                    exp_delivery_date = reader["exp_delivery_date"] != DBNull.Value ? (DateTime)reader["exp_delivery_date"] : (DateTime?)null,
                                    tag = reader["tag"].ToString(),
                                    cheque_issued = Convert.ToInt32(reader["cheque_issued"]),
                                    last_approved_by = reader["last_approved_by"].ToString()
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

        [HttpGet("GetPurchaseOrders/{id}")]
        public IActionResult GetPurchaseOrdersId(Guid userId,Guid id)
        {
            try
            {
                List<GetPO> purchaseOrders = new List<GetPO>();
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.PurchaseOrder_GetById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@trn_id", id);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a new GetPO object
                                var purchaseOrder = new GetPO
                                {
                                    purchase_Order_trn_id = reader.IsDBNull(reader.GetOrdinal("purchase_Order_trn_id")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("purchase_Order_trn_id")),
                                    voucher_id = reader.IsDBNull(reader.GetOrdinal("voucher_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("voucher_id")),
                                    voucher_type = reader.IsDBNull(reader.GetOrdinal("voucher_type")) ? string.Empty : reader.GetString(reader.GetOrdinal("voucher_type")),
                                    doc_no = reader.IsDBNull(reader.GetOrdinal("doc_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("doc_no")),
                                    order_no = reader.IsDBNull(reader.GetOrdinal("order_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("order_no")),
                                    doc_date = reader.IsDBNull(reader.GetOrdinal("doc_date")) ? DateOnly.MinValue : DateOnly.FromDateTime(reader.GetDateTime(reader.GetOrdinal("doc_date"))),
                                    vendor_id = reader.IsDBNull(reader.GetOrdinal("vendor_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("vendor_id")),
                                    vendor_name = reader.IsDBNull(reader.GetOrdinal("vendor_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("vendor_name")),
                                    vendor_address = reader.IsDBNull(reader.GetOrdinal("vendor_address")) ? string.Empty : reader.GetString(reader.GetOrdinal("vendor_address")),
                                    vendor_state = reader.IsDBNull(reader.GetOrdinal("vendor_state")) ? string.Empty : reader.GetString(reader.GetOrdinal("vendor_state")),
                                    vendor_pin = reader.IsDBNull(reader.GetOrdinal("vendor_pin")) ? string.Empty : reader.GetString(reader.GetOrdinal("vendor_pin")),
                                    vendor_gst = reader.IsDBNull(reader.GetOrdinal("vendor_gst")) ? string.Empty : reader.GetString(reader.GetOrdinal("vendor_gst")),
                                    con_person = reader.IsDBNull(reader.GetOrdinal("con_person")) ? string.Empty : reader.GetString(reader.GetOrdinal("con_person")),
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
                                    po_due_date = reader.IsDBNull(reader.GetOrdinal("po_due_date")) ? DateTime.MinValue : reader.GetDateTime(reader.GetOrdinal("po_due_date")),
                                    remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? string.Empty : reader.GetString(reader.GetOrdinal("remarks")),
                                    t_c = reader.IsDBNull(reader.GetOrdinal("t_c")) ? string.Empty : reader.GetString(reader.GetOrdinal("t_c")),
                                    warehouse_id = reader.IsDBNull(reader.GetOrdinal("warehouse_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("warehouse_id")),
                                    warehouse_name = reader.IsDBNull(reader.GetOrdinal("warehouse_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("warehouse_name")),
                                    contact_person = reader.IsDBNull(reader.GetOrdinal("contact_person")) ? string.Empty : reader.GetString(reader.GetOrdinal("contact_person")),
                                    unit_name = reader.IsDBNull(reader.GetOrdinal("unit_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("unit_name")),
                                    dev_inv = reader.IsDBNull(reader.GetOrdinal("dev_inv")) ? string.Empty : reader.GetString(reader.GetOrdinal("dev_inv")),
                                    office = reader.IsDBNull(reader.GetOrdinal("office")) ? string.Empty : reader.GetString(reader.GetOrdinal("office")),
                                    gst_no = reader.IsDBNull(reader.GetOrdinal("gst_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("gst_no")),
                                    tag = reader["tag"].ToString(),

                                };

                                // Deserialize JSON properties
                                string itemDetailsJson = reader.GetString(reader.GetOrdinal("item_details"));
                                purchaseOrder.item_details = JsonConvert.DeserializeObject<List<getitems>>(itemDetailsJson);

                                string termsDetailsJson = reader.GetString(reader.GetOrdinal("term_details"));
                                purchaseOrder.term_details = JsonConvert.DeserializeObject<List<getterms>>(termsDetailsJson);

                                string quotationDetailsJson = reader.GetString(reader.GetOrdinal("quotation_details"));
                                purchaseOrder.quotation_details = JsonConvert.DeserializeObject<List<quotations>>(quotationDetailsJson);

                                string statusJson = reader.GetString(reader.GetOrdinal("approving_flow"));
                                purchaseOrder.approving_flow = JsonConvert.DeserializeObject<List<ApproveFlow>>(statusJson);

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


        [HttpPut("delete/{purchase_order_id}")]
        public IActionResult Delete_PurchaseIndent(Guid purchase_order_id, [FromBody] DeletePurchaseIndent deletePurchaseIndent)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PurchaseOrder_Delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", deletePurchaseIndent.user_id);
                        command.Parameters.AddWithValue("@purchase_order_id", purchase_order_id);

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


        [HttpGet("PI_DeliverySchedule")]
        public IActionResult GetItemCategories(string item_id,Guid trn_id)
        {
            try
            {
                List<pi_delivery> vendortypes = new List<pi_delivery>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.DeliverySchedule_Get_For_PO", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@item_id", item_id );
                        command.Parameters.AddWithValue("@trn_id", trn_id);

                        // Add OUTPUT parameter to capture the stored procedure message
                        var executeMessageParam = new SqlParameter("@executeMessage", SqlDbType.NVarChar, -1)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(executeMessageParam);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    pi_delivery vendortype = new pi_delivery
                                    {
                                        quantity = (decimal)reader["quantity"],
                                        trn_dt = (string)reader["trn_dt"]
                                    };

                                    vendortypes.Add(vendortype);
                                }
                            }
                        }

                        // Get the executeMessage after the stored procedure execution
                        string executeMessage = executeMessageParam.Value.ToString();

                        if (vendortypes.Any())
                        {
                            return Ok(new { message = executeMessage, data = vendortypes });
                        }
                        else
                        {
                            return NotFound(new { message = executeMessage, data = vendortypes });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

    }
}
