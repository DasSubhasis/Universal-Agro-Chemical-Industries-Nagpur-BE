using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Dugros_Api.Controllers.ItemCategoryController;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace Dugros_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseTypeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public WarehouseTypeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class GetWarehouseType
        {
            public Guid wh_type_id { get; set; }
            public string wh_type_name { get; set; }
            public int is_active { get; set; }
        }

        public class AddWarehouseType
        {
            public Guid user_id { get; set; }
            public string wh_type_name { get; set; }
        }
        public class EditWarehouseTypeModel
        {
            public Guid user_id { get; set; }
            public string wh_type_name { get; set; }
            public int is_active { get; set; }
        }

        public class DeleteWarehouseTypeModel
        {
            public Guid user_id { get; set; }
        }


        [HttpGet]
        public IActionResult GetWarehouseTypes(Guid userId)
        {
            try
            {
                List<GetWarehouseType> itemCategories = new List<GetWarehouseType>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.WarehouseTypeGet", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    GetWarehouseType warehouseType = new GetWarehouseType
                                    {
                                        wh_type_id = (Guid)(reader["wh_type_id"]),
                                        wh_type_name = reader["wh_type_name"].ToString(),
                                        is_active = Convert.ToInt32(reader["is_active"])
                                    };

                                    itemCategories.Add(warehouseType);
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
                    return NotFound("No warehouse type found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("add")]
        public IActionResult PostWarehouseType(AddWarehouseType addWarehouseType)
        {
            try
            {
                string message;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.WarehouseTypeInsert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", addWarehouseType.user_id);
                        command.Parameters.AddWithValue("@wh_type_name", addWarehouseType.wh_type_name);

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
                if (message.StartsWith("Warehouse Type inserted successfully"))
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

        [HttpPut("edit/{wh_type_id}")]
        public IActionResult EditItemCategory(Guid wh_type_id, [FromBody] EditWarehouseTypeModel editWarehouseType)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.WarehouseTypeEdit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", editWarehouseType.user_id);
                        command.Parameters.AddWithValue("@wh_type_id", wh_type_id);
                        command.Parameters.AddWithValue("@wh_type_name", editWarehouseType.wh_type_name);
                        command.Parameters.AddWithValue("@is_active", editWarehouseType.is_active);

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
                            return Ok(new { ExecuteMessage = successMessage });// Return success message
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

        [HttpPut("delete/{wh_type_id}")]
        public IActionResult DeleteItemCategory(Guid wh_type_id, [FromBody] DeleteWarehouseTypeModel deleteWarehouseType)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.WarehouseTypeDelete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", deleteWarehouseType.user_id);
                        command.Parameters.AddWithValue("@wh_type_id", wh_type_id);

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
