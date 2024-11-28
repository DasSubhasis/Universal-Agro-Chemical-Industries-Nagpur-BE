using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Dugros_Api.Controllers.PurchaseBillController;
using System.Data.SqlClient;
using System.Data;

namespace Dugros_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShortFullCloseController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ShortFullCloseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class getshortclose
        {
            public Guid trn_id { get; set; }
            public DateTime trn_dt { get; set; }
            public string item_id { get; set; }
            public string item_name { get; set; }
            public string uom_id { get; set; }
            public string uom_name { get; set; }
            public decimal po_qty { get; set; }
            public decimal indent_qty { get; set; }
            public decimal pending_qty { get; set; }

            public string indent_no { get; set; }
        }

        public class getshortclose_PO
        {
            public Guid trn_id { get; set; }
            public DateTime trn_dt { get; set; }
            public string item_id { get; set; }
            public string item_name { get; set; }
            public string uom_id { get; set; }
            public string uom_name { get; set; }
            public decimal grn_qty { get; set; }
            public decimal po_qty { get; set; }
            public decimal pending_qty { get; set; }

            public string po_no { get; set; }
        }

        public class UpdateDetail
        {
            public Guid TrnId { get; set; }
            public string ItemId { get; set; }
            public string Remarks { get; set; }
            public decimal ClosingQty { get; set; }
        }

        public class ShortCloseUpdate
        {
            public List<UpdateDetail> details { get; set; }
        }
        [HttpPost("Get_LOI")]
        public IActionResult GetPurchaseOrders(Guid trn_id)
        {
            try
            {
                List<getshortclose> purchaseOrders = new List<getshortclose>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.Short_Full_Get", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@trn_id", trn_id);
                      

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a new GetPO object
                                var purchaseOrder = new getshortclose
                                {
                                    trn_id = (Guid)reader["purchase_indent_id"],
                                    trn_dt = (DateTime)reader["trn_dt"],
                                    item_name = reader.IsDBNull(reader.GetOrdinal("item_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("item_name")),
                                    item_id = reader.IsDBNull(reader.GetOrdinal("item_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("item_id")),
                                    uom_id = reader.IsDBNull(reader.GetOrdinal("uom_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("uom_id")),
                                    uom_name = reader.IsDBNull(reader.GetOrdinal("uom_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("uom_name")),
                                    po_qty = reader.IsDBNull(reader.GetOrdinal("po_qty")) ? 0 : reader.GetDecimal(reader.GetOrdinal("po_qty")),
                                    indent_qty = reader.IsDBNull(reader.GetOrdinal("indent_qty")) ? 0 : reader.GetDecimal(reader.GetOrdinal("indent_qty")),
                                    pending_qty = reader.IsDBNull(reader.GetOrdinal("pending_qty")) ? 0 : reader.GetDecimal(reader.GetOrdinal("pending_qty")),
                                    indent_no = reader.IsDBNull(reader.GetOrdinal("base_reference_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("base_reference_no")),
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

        [HttpPost("PostShortClose")]
        public IActionResult BulkUpdate([FromBody] ShortCloseUpdate model)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    foreach (var update in model.details)
                    {
                        using (var command = new SqlCommand("dbo.Short_Full_Insert", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@trn_id", update.TrnId);
                            command.Parameters.AddWithValue("@item_id", update.ItemId);
                            command.Parameters.AddWithValue("@close_qty", update.ClosingQty);
                            command.Parameters.AddWithValue("@remarks", update.Remarks);

                            // You can also add an output parameter to get the message from the SP
                            var outputParam = new SqlParameter("@executeMessage", SqlDbType.NVarChar, -1)
                            {
                                Direction = ParameterDirection.Output
                            };
                            command.Parameters.Add(outputParam);

                            command.ExecuteNonQuery();
                            var message = outputParam.Value.ToString();

                            // Optionally handle or log the message
                        }
                    }

                    return Ok(new { ExecuteMessage = "update successful." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpPost("PO/Get_LOI")]
        public IActionResult GetPOLOI(Guid trn_id)
        {
            try
            {
                List<getshortclose_PO> purchaseOrders = new List<getshortclose_PO>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.Short_Full_Get_PO", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@trn_id", trn_id);


                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a new GetPO object
                                var purchaseOrder = new getshortclose_PO
                                {
                                    trn_id = (Guid)reader["purchase_order_id"],
                                    trn_dt = (DateTime)reader["trn_dt"],
                                    item_name = reader.IsDBNull(reader.GetOrdinal("item_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("item_name")),
                                    item_id = reader.IsDBNull(reader.GetOrdinal("item_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("item_id")),
                                    uom_id = reader.IsDBNull(reader.GetOrdinal("uom_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("uom_id")),
                                    uom_name = reader.IsDBNull(reader.GetOrdinal("uom_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("uom_name")),
                                    grn_qty = reader.IsDBNull(reader.GetOrdinal("grn_qty")) ? 0 : reader.GetDecimal(reader.GetOrdinal("grn_qty")),
                                    po_qty = reader.IsDBNull(reader.GetOrdinal("po_qty")) ? 0 : reader.GetDecimal(reader.GetOrdinal("po_qty")),
                                    pending_qty = reader.IsDBNull(reader.GetOrdinal("pending_qty")) ? 0 : reader.GetDecimal(reader.GetOrdinal("pending_qty")),
                                    po_no = reader.IsDBNull(reader.GetOrdinal("base_reference_no")) ? string.Empty : reader.GetString(reader.GetOrdinal("base_reference_no")),
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

        [HttpPost("PO/PostShortClose")]
        public IActionResult updatepo([FromBody] ShortCloseUpdate model)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    foreach (var update in model.details)
                    {
                        using (var command = new SqlCommand("dbo.Short_Full_Insert_PO", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@trn_id", update.TrnId);
                            command.Parameters.AddWithValue("@item_id", update.ItemId);
                            command.Parameters.AddWithValue("@close_qty", update.ClosingQty);
                            command.Parameters.AddWithValue("@remarks", update.Remarks);

                            // You can also add an output parameter to get the message from the SP
                            var outputParam = new SqlParameter("@executeMessage", SqlDbType.NVarChar, -1)
                            {
                                Direction = ParameterDirection.Output
                            };
                            command.Parameters.Add(outputParam);

                            command.ExecuteNonQuery();
                            var message = outputParam.Value.ToString();

                            // Optionally handle or log the message
                        }
                    }

                    return Ok(new { ExecuteMessage = "update successful." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
