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
    public class FollowUpController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FollowUpController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class GetFollow
        {
            public Guid id { get; set; }
            public Guid trn_id { get; set; }
            //public DateTime? exp_delivery_date { get; set; }
            public string exp_delivery_date { get; set; }
            //public DateTime? dispatch_date { get; set; }
            public string dispatch_date { get; set; }
            //public DateTime? created_date { get; set; }
            public string created_date { get; set; }
            public string notes { get; set; }
            public Guid user_id { get; set; }
            public string username { get; set; }
        }

        public class PostFollow
        {
            public Guid trn_id { get; set; }
            public DateTime? exp_delivery_date { get; set; } // Nullable DateTime
            public DateTime? dispatch_date { get; set; } // Nullable DateTime
            public string notes { get; set; } // Optional
            public Guid user_id { get; set; }
        }

        [HttpGet]
        public IActionResult GetItemCategories(Guid trn_id)
        {
            try
            {

                List<GetFollow> itemCategories = new List<GetFollow>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.FollowGet", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@trn_id", trn_id);

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
                                    GetFollow color = new GetFollow
                                    {
                                        id = reader["id"] != DBNull.Value ? (Guid)reader["id"] : Guid.Empty,
                                        trn_id = reader["trn_id"] != DBNull.Value ? (Guid)reader["trn_id"] : Guid.Empty,
                                        //exp_delivery_date = reader["exp_delivery_date"] != DBNull.Value ? (DateTime)reader["exp_delivery_date"] : (DateTime?)null,
                                        //dispatch_date = reader["dispatch_date"] != DBNull.Value ? (DateTime)reader["dispatch_date"] : (DateTime?)null,
                                        //created_date = reader["created_date"] != DBNull.Value ? (DateTime)reader["created_date"] : (DateTime?)null,
                                        exp_delivery_date = reader["exp_delivery_date"].ToString(),
                                        dispatch_date = reader["dispatch_date"].ToString(),
                                        created_date = reader["created_date"].ToString(),
                                        notes = reader["notes"] != DBNull.Value ? reader["notes"].ToString() : null,
                                        user_id = reader["user_id"] != DBNull.Value ? (Guid)reader["user_id"] : Guid.Empty,
                                        username = reader["username"] != DBNull.Value ? reader["username"].ToString() : null,
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
                    return NotFound("No follow ups found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("addFollow")]
        public IActionResult AddFollow(PostFollow postFollow)
        {
            try
            {
                string message;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.FollowInsert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@trn_id", postFollow.trn_id);

                        // Handle nullable parameters
                        command.Parameters.AddWithValue("@exp_delivery_date", (object)postFollow.exp_delivery_date ?? DBNull.Value);
                        command.Parameters.AddWithValue("@dispatch_date", (object)postFollow.dispatch_date ?? DBNull.Value);

                        // Handle optional parameter
                        command.Parameters.AddWithValue("@notes", (object)postFollow.notes ?? DBNull.Value);
                        command.Parameters.AddWithValue("@user_id", postFollow.user_id);

                        // Add OUTPUT parameter to capture the stored procedure message
                        var outputParam = new SqlParameter("@Message", SqlDbType.NVarChar, 1000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Get the message from the output parameter
                        message = command.Parameters["@Message"].Value.ToString();
                    }
                }

                // Check the message returned by the stored procedure
                if (message.StartsWith("Follow up inserted successfully."))
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

        [HttpPut("delete/{follow_id}")]
        public IActionResult DeleteItemCategory(Guid follow_id)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.FollowDelete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@id", follow_id);

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
