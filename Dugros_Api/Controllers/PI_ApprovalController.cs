using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Data;
using System.Data.SqlClient;

namespace Dugros_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PI_ApprovalController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PI_ApprovalController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class GetHeader
        {
            public Guid temp_id { get; set; }
            public Guid voucher_id { get; set; }
            public string voucher_name { get; set; }
            public string temp_name { get; set; }
            public bool is_any { get; set; }
            public List<details> approver_details { get; set; }
        }

        public class details
        {
            public Guid user_id { get; set; }
            public string full_name { get; set; }
            public string activity { get; set; }
            public Guid  status { get; set; }
            public string status_name { get; set; }
            public string remarks { get; set; }
            public bool is_enable { get; set; }
        }

        [HttpGet("GetApproveSeqById")]
        public IActionResult GetPurchaseOrders(Guid userId, Guid trn_id)
        {
            try
            {
                List<GetHeader> purchaseOrders = new List<GetHeader>();

                

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.PI_ApproverSeq_GET", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@trn_id", trn_id);
                        

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a new GetPO object
                                var purchaseOrder = new GetHeader
                                {
                                    temp_id = reader.IsDBNull(reader.GetOrdinal("temp_id")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("temp_id")),
                                    voucher_id = reader.IsDBNull(reader.GetOrdinal("voucher_id")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("voucher_id")),
                                    temp_name = reader.IsDBNull(reader.GetOrdinal("temp_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("temp_name")),
                                    voucher_name = reader.IsDBNull(reader.GetOrdinal("voucher_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("voucher_name")),
                                    is_any = !reader.IsDBNull(reader.GetOrdinal("is_any")) && reader.GetBoolean(reader.GetOrdinal("is_any")),

                                };

                                // Deserialize JSON properties
                                string details = reader.GetString(reader.GetOrdinal("approver_details"));
                                purchaseOrder.approver_details = JsonConvert.DeserializeObject<List<details>>(details);

                               
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
