using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Dugros_Api.Controllers.ColorController;
using System.Data.SqlClient;
using System.Data;

namespace Dugros_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ApprovalFlowController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ApprovalFlowController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class GetFlow
        {
            public Guid id { get; set; }
            public Guid creator { get; set; }
            public string creator_name { get; set; }
            public Guid checker { get; set; }
            public string checker_name { get; set; }
            public Guid approver { get; set; }
            public string approver_name { get; set; }
            public Guid spcl_approver { get; set; }
            public string spcl_approver_name { get; set; }
            public int is_active { get; set; }
            //public Guid document_id { get; set; }
            public string document_name { get; set; }
        }

        public class PostFlow
        {
            public Guid user_id { get; set; }
            public Guid creator { get; set; }
            public Guid checker { get; set; }
            public Guid approver { get; set; }
            public Guid spcl_approver { get; set; }
        }
        public class EditFlow
        {
            public Guid user_id { get; set; }
            public Guid creator { get; set; }
            public Guid checker { get; set; }
            public Guid approver { get; set; }
            public Guid spcl_approver { get; set; }
            public int is_active { get; set; }
        }
        public class DeleteFlow
        {
            public Guid user_id { get; set; }
        }

        [HttpGet]
        public IActionResult Get_Flows(Guid userId)
        {
            try
            {

                List<GetFlow> itemCategories = new List<GetFlow>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.ApprovalFlowGet", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);

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
                                    GetFlow color = new GetFlow
                                    {
                                        id = (Guid)reader["id"],
                                        creator = (Guid)reader["creator"],
                                        checker = (Guid)reader["checker"],
                                        approver = (Guid)reader["approver"],
                                        spcl_approver = (Guid)reader["special_approver"],
                                        creator_name = reader["creator_name"].ToString(),
                                        checker_name = reader["checker_name"].ToString(),
                                        approver_name = reader["approver_name"].ToString(),
                                        spcl_approver_name = reader["spcl_approver_name"].ToString(),
                                        is_active = Convert.ToInt32(reader["is_active"]),
                                        //document_id = (Guid)reader["document_id"],
                                        document_name = reader["document_name"].ToString()
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
                    return NotFound("No flows found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("addFlow")]
        public IActionResult AddCountry(PostFlow postFlow)
        {
            try
            {
                string message;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.ApprovalFlowInsert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", postFlow.user_id);
                        command.Parameters.AddWithValue("@creator", postFlow.creator);
                        command.Parameters.AddWithValue("@checker", postFlow.checker);
                        command.Parameters.AddWithValue("@approver", postFlow.approver);
                        command.Parameters.AddWithValue("@spcl_approver", postFlow.spcl_approver);



                        // Add OUTPUT parameter to capture the stored procedure message
                        var outputParam = new SqlParameter("@Message", SqlDbType.NVarChar, 1000);
                        outputParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputParam);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Get the message from the output parameter
                        message = command.Parameters["@Message"].Value.ToString();
                    }
                }

                // Check the message returned by the stored procedure
                if (message.StartsWith("Approval flow inserted successfully."))
                {
                    return Ok(new { ExecuteMessage = message });
                }
                else
                {
                    return BadRequest(message); // Return error message
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPut("edit/{flow_id}")]
        public IActionResult EditItemCategory(Guid flow_id, [FromBody] EditFlow editFlow)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.ApprovalFlowEdit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", editFlow.user_id);
                        command.Parameters.AddWithValue("@flow_id", flow_id);
                        command.Parameters.AddWithValue("@creator", editFlow.creator);
                        command.Parameters.AddWithValue("@checker", editFlow.checker);
                        command.Parameters.AddWithValue("@approver", editFlow.approver);
                        command.Parameters.AddWithValue("@spcl_approver", editFlow.spcl_approver);
                        command.Parameters.AddWithValue("@is_active", editFlow.is_active);

                        // Execute the stored procedure
                        var successMessageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 500)
                        {
                            Direction = ParameterDirection.Output
                        };
                        var errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 500)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(successMessageParam);
                        command.Parameters.Add(errorMessageParam);

                        command.ExecuteNonQuery();

                        string successMessage = successMessageParam.Value?.ToString();
                        string errorMessage = errorMessageParam.Value?.ToString();

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            return StatusCode(400, errorMessage); // Return bad request with error message
                        }
                        else if (!string.IsNullOrEmpty(successMessage))
                        {
                            return Ok(new { ExecuteMessage = successMessage }); // Return success message
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

        [HttpPut("delete/{flow_id}")]
        public IActionResult DeleteItemCategory(Guid flow_id, [FromBody] DeleteFlow deleteFlow)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.ApprovalFlowDelete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", deleteFlow.user_id);
                        command.Parameters.AddWithValue("@flow_id", flow_id);

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
