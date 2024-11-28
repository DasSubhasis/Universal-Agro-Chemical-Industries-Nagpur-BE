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
    public class ApprovalMappingController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ApprovalMappingController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class GetMapping
        {
            public Guid map_id { get; set; }
            public Guid document_id { get; set; }
            public string document_name { get; set; }
            public Guid approval_id { get; set; }
            public Guid creator { get; set; }
            public string creator_name { get; set; }
            public Guid checker { get; set; }
            public string checker_name { get; set; }
            public Guid approver { get; set; }
            public string approver_name { get; set; }
            public Guid spcl_approver { get; set; }
            public string spcl_approver_name { get; set; }
            public int is_active { get; set; }
        }

        public class PostMapping
        {
            public Guid user_id { get; set; }
            public Guid document_id { get; set; }
            public Guid approval_id { get; set; }
        }
        public class EditMapping
        {
            public Guid user_id { get; set; }
            public Guid document_id { get; set; }
            public Guid approval_id { get; set; }
            public int is_active { get; set; }
        }
        public class DeleteMapping
        {
            public Guid user_id { get; set; }
        }

        [HttpGet]
        public IActionResult Get_Mapping(Guid userId)
        {
            try
            {

                List<GetMapping> itemCategories = new List<GetMapping>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.ApprovalMappingGet", connection))
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
                                    GetMapping color = new GetMapping
                                    {
                                        map_id = (Guid)reader["id"],
                                        document_id = (Guid)reader["document_id"],
                                        document_name = reader["document_name"].ToString(),
                                        approval_id = (Guid)reader["approval_id"],
                                        creator = (Guid)reader["creator"],
                                        creator_name = reader["creator_name"].ToString(),
                                        checker = (Guid)reader["checker"],
                                        checker_name = reader["checker_name"].ToString(),
                                        approver = (Guid)reader["approver"],
                                        approver_name = reader["approver_name"].ToString(),
                                        spcl_approver = (Guid)reader["special_approver"],
                                        spcl_approver_name = reader["spcl_approver_name"].ToString(),
                                        is_active = Convert.ToInt32(reader["is_active"]),

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
                    return NotFound("No mappings found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("addMapping")]
        public IActionResult AddCountry(PostMapping postMapping)
        {
            try
            {
                string message;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.ApprovalMappingInsert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", postMapping.user_id);
                        command.Parameters.AddWithValue("@document_id", postMapping.document_id);
                        command.Parameters.AddWithValue("@approval_id", postMapping.approval_id);



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
                if (message.StartsWith("Mapping inserted successfully."))
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

        [HttpPut("edit/{map_id}")]
        public IActionResult EditItemCategory(Guid map_id, [FromBody] EditMapping editMapping)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.ApprovalMappingEdit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", editMapping.user_id);
                        command.Parameters.AddWithValue("@map_id", map_id);
                        command.Parameters.AddWithValue("@document_id", editMapping.document_id);
                        command.Parameters.AddWithValue("@approval_id", editMapping.approval_id);
                        command.Parameters.AddWithValue("@is_active", editMapping.is_active);

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

        [HttpPut("delete/{map_id}")]
        public IActionResult DeleteItemCategory(Guid map_id, [FromBody] DeleteMapping deleteMapping)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.ApprovalMappingDelete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", deleteMapping.user_id);
                        command.Parameters.AddWithValue("@map_id", map_id);

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
