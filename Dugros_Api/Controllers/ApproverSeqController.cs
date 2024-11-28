using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Dugros_Api.Controllers.PurchaseIndentController;
using System.Data.SqlClient;
using System.Data;
using static Dugros_Api.Controllers.PurchaseOrderController;
using Newtonsoft.Json;
using System.Globalization;

namespace Dugros_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApproverSeqController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ApproverSeqController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class template
        {
            public string temp_name { get; set; }
            public bool is_any { get; set; }
            public List<approver_seq> approver { get; set; }
        }
        public class approver_seq
        {
            public Guid user_id { get; set; }
            public string activity { get; set; }
            public int seq { get; set; }
        }

        public class GetTemplate
        {
            public Guid temp_id { get; set; }
            public string temp_name { get; set; }
            public bool is_any { get; set; }
            public List<getApprover> approver { get; set; }
           
        }
        public class GetTemplatebyId
        {
            public Guid temp_id { get; set; }
            public string temp_name { get; set; }
            public bool is_any { get; set; }
            public List<getCreator> creator { get; set; }
            public List<getSpApprover> special_approver { get; set; }
            public List<getApprover> approver { get; set; }

        }

        public class getCreator
        {
            public Guid trn_id { get; set; }
            public Guid user_id { get; set; }
            public string user_name { get; set; }
            public string activity { get; set; }
            public int seq_no { get; set; }
        }
        public class getSpApprover
        {
            public Guid trn_id { get; set; }
            public Guid user_id { get; set; }
            public string user_name { get; set; }
            public string activity { get; set; }
            public int seq_no { get; set; }
        }
        public class getApprover
        {
            public Guid trn_id { get; set; }
            public Guid user_id { get; set; }
            public string user_name { get; set; }
            public string  activity { get; set; }
            public int seq_no { get; set; }
        }
        public class junc_post
        {
            public Guid temp_id { get; set; }
            public Guid voucher_id { get; set; }
        }
        public class junc_get
        {
            public Guid temp_id { get; set; }
            public string temp_name { get; set; }
            public string voucher_id { get; set; }
            public string voucher_name { get; set; }
        }

        [HttpGet("GetApproverSeq")]
        public IActionResult GetSeq(Guid userId)
        {
            try
            {
                List<GetTemplate> purchaseOrders = new List<GetTemplate>();

                

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.ApproverSeq_Get", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                       

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a new GetPO object
                                var purchaseOrder = new GetTemplate
                                {
                                    temp_id = reader.IsDBNull(reader.GetOrdinal("id")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("id")),
                                    temp_name = reader.IsDBNull(reader.GetOrdinal("temp_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("temp_name")),
                                    is_any = reader.IsDBNull(reader.GetOrdinal("is_any")) ? false : reader.GetBoolean(reader.GetOrdinal("is_any")),
                                };

                                // Deserialize JSON properties
                                string itemDetailsJson = reader.GetString(reader.GetOrdinal("approver"));
                                purchaseOrder.approver = JsonConvert.DeserializeObject<List<getApprover>>(itemDetailsJson);

                        

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


        [HttpGet("GetApproverSeqById")]
        public IActionResult GetSeqById(Guid userId,Guid temp_id)
        {
            try
            {
                List<GetTemplatebyId> purchaseOrders = new List<GetTemplatebyId>();



                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("ApproverSeq_GetById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@temp_id", temp_id);


                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a new GetPO object
                                var purchaseOrder = new GetTemplatebyId
                                {
                                    temp_id = reader.IsDBNull(reader.GetOrdinal("id")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("id")),
                                    temp_name = reader.IsDBNull(reader.GetOrdinal("temp_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("temp_name")),
                                    is_any = reader.IsDBNull(reader.GetOrdinal("is_any")) ? false : reader.GetBoolean(reader.GetOrdinal("is_any")),
                                };

                                // Deserialize JSON properties

                                string itemDetailsJson1 = reader.GetString(reader.GetOrdinal("creator"));
                                purchaseOrder.creator = JsonConvert.DeserializeObject<List<getCreator>>(itemDetailsJson1);

                                string itemDetailsJson2 = reader.GetString(reader.GetOrdinal("special_approver"));
                                purchaseOrder.special_approver = JsonConvert.DeserializeObject<List<getSpApprover>>(itemDetailsJson2);

                                string itemDetailsJson = reader.GetString(reader.GetOrdinal("approver"));
                                purchaseOrder.approver = JsonConvert.DeserializeObject<List<getApprover>>(itemDetailsJson);



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


        [HttpPost("AddApproverSeq")]
        public IActionResult AddSeq(template post_temp, Guid userId)
        {
            try
            {
                string message;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.ApproverSeq_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@temp_name", post_temp.temp_name);
                        command.Parameters.AddWithValue("@is_any", post_temp.is_any);
                        

                        string approver_details = Newtonsoft.Json.JsonConvert.SerializeObject(post_temp.approver);

                        command.Parameters.AddWithValue("@approver_seq", approver_details);

                       

                        // Add output parameter for the execute message
                        command.Parameters.Add("@executeMessage", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the output parameter value
                        message = command.Parameters["@executeMessage"].Value.ToString();
                    }

                    // Return success message
                    return Ok(new { ExecuteMessage = message });
                }
            }
            catch (Exception ex)
            {
                // Return error message
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("AddJunction")]
        public IActionResult AddJunc(junc_post junc, Guid userId)
        {
            try
            {
                string message;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.Junction_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@temp_id", junc.temp_id);
                        command.Parameters.AddWithValue("@voucher_id", junc.voucher_id);


                        


                        // Add output parameter for the execute message
                        command.Parameters.Add("@executeMessage", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Retrieve the output parameter value
                        message = command.Parameters["@executeMessage"].Value.ToString();
                    }

                    // Return success message
                    return Ok(new { ExecuteMessage = message });
                }
            }
            catch (Exception ex)
            {
                // Return error message
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("GetJunction")]
        public IActionResult GetJunc(Guid userId)
        {
            try
            {
                List<junc_get> purchaseOrders = new List<junc_get>();



                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();
                    using (var command = new SqlCommand("dbo.Junction_Get", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);


                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                // Create a new GetPO object
                                var purchaseOrder = new junc_get
                                {
                                    temp_id = reader.IsDBNull(reader.GetOrdinal("id")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("temp_id")),
                                    temp_name = reader.IsDBNull(reader.GetOrdinal("temp_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("temp_name")),
                                    voucher_id =reader.IsDBNull(reader.GetOrdinal("voucher_id")) ? string.Empty : reader.GetString(reader.GetOrdinal("voucher_id")),
                                    voucher_name = reader.IsDBNull(reader.GetOrdinal("voucher_name")) ? string.Empty : reader.GetString(reader.GetOrdinal("voucher_name")),
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
    }
}
