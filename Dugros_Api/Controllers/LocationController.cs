using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Dugros_Api.Controllers.WarehouseController;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace Dugros_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LocationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class GetLocation
        {
            public Guid location_id { get; set; }
            public string location_name { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public Guid state_id { get; set; }
            public string state_name { get; set; }
            public Guid country_id { get; set; }
            public string country_name { get; set; }
            public int is_active { get; set; }
        }

        public class AddLocationModel
        {
            public Guid user_id { get; set; }
            public string location_name { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public Guid state_id { get; set; }
            public Guid country_id { get; set; }

        }

        public class EditLocationModel
        {
            public Guid user_id { get; set; }
            public string location_name { get; set; }
            public string address { get; set; }
            public string city { get; set; }
            public Guid state_id { get; set; }
            public Guid country_id { get; set; }
            public int is_active { get; set; }
        }

        public class DeleteLocationModel
        {
            public Guid user_id { get; set; }
        }

        [HttpGet]
        public IActionResult GetItemCategories(Guid userId)
        {
            try
            {
                List<GetLocation> itemCategories = new List<GetLocation>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.LocationGet", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    GetLocation itemCategory = new GetLocation
                                    {
                                        location_id = (Guid)reader["location_id"],
                                        location_name = reader["location_name"].ToString(),
                                        address = reader["address"].ToString(),
                                        city = reader["city"].ToString(),
                                        state_id = (Guid)reader["state_id"],
                                        state_name =reader["state_name"].ToString(),
                                        country_id = (Guid)reader["country_id"],
                                        country_name = reader["country_name"].ToString(),
                                        is_active = Convert.ToInt32(reader["is_active"])
                                    };

                                    itemCategories.Add(itemCategory);
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
                    return NotFound("No location found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("addLocation")]
        public IActionResult AddLocation(AddLocationModel addLocation)
        {
            try
            {
                string message;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.LocationInsert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", addLocation.user_id);
                        command.Parameters.AddWithValue("@location_name", addLocation.location_name);
                        command.Parameters.AddWithValue("@address", addLocation.address);
                        command.Parameters.AddWithValue("@city", addLocation.city);
                        command.Parameters.AddWithValue("@state_id", addLocation.state_id);
                        command.Parameters.AddWithValue("@country_id", addLocation.country_id);


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
                if (message.StartsWith("Location inserted successfully."))
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

        [HttpPut("edit/{location_id}")]
        public IActionResult EditLocation(Guid location_id, [FromBody] EditLocationModel editLocation)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.LocationEdit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", editLocation.user_id);
                        command.Parameters.AddWithValue("@location_id", location_id);
                        command.Parameters.AddWithValue("@location_name", editLocation.location_name);
                        command.Parameters.AddWithValue("@address", editLocation.address);
                        command.Parameters.AddWithValue("@city", editLocation.city);
                        command.Parameters.AddWithValue("@state_id", editLocation.state_id);
                        command.Parameters.AddWithValue("@country_id", editLocation.country_id);
                        command.Parameters.AddWithValue("@is_active", editLocation.is_active);

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

        [HttpPut("delete/{location_id}")]
        public IActionResult DeleteItemCategory(Guid location_id, [FromBody] DeleteLocationModel deleteLocation)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.LocationDelete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", deleteLocation.user_id);
                        command.Parameters.AddWithValue("@location_id", location_id);

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
