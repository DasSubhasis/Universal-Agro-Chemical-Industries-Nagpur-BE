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
    public class WarehouseController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public WarehouseController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class GetWarehouse
        {
            public  Guid warehouse_id { get; set; }
            public string wh_name { get; set; }
            //public string address { get; set; }
            //public string city { get; set; }
            //public int state_id { get; set; }
            //public int country_id { get; set; }
            public Guid location_id { get; set; }
            public string location_name { get; set; }
            public Guid wh_type_id { get; set; }
            public string wh_type_name { get; set; }
            public int is_active { get; set; }
        }
        public class AddWarehouseModel
        {
            public Guid user_id { get; set; }
            public string wh_name { get; set; }
            //public string address { get; set; }
            //public string city { get; set; }
            //public int state_id { get; set; }
            //public int country_id { get; set; }
            public Guid location_id { get; set; }
            public Guid wh_type_id { get; set; }
        
        }

        public class EditWarehouseModel
        {
            public Guid user_id { get; set; }
            public string wh_name { get; set; }
            //public string address { get; set; }
            //public string city { get; set; }
            //public int state_id { get; set; }
            //public int country_id { get; set; }
            public Guid location_id { get; set; }
            public Guid wh_type_id { get; set; }
            public int is_active { get; set; }
        }

        public class DeleteWarehouseModel
        {
            public Guid user_id { get; set; }
        }

        [HttpGet]
        public IActionResult GetItemCategories(Guid userId)
        {
            try
            {
                List<GetWarehouse> itemCategories = new List<GetWarehouse>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.WarehouseGet", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    GetWarehouse itemCategory = new GetWarehouse
                                    {
                                        warehouse_id = (Guid)(reader["warehouse_id"]),
                                        wh_name = reader["warehouse_name"].ToString(),
                                        //address = reader["address"].ToString(),
                                        //city = reader["city"].ToString(),
                                        //state_id = Convert.ToInt32(reader["state_id"]),
                                        //country_id = Convert.ToInt32(reader["country_id"]),
                                        location_id = (Guid)reader["location_id"],
                                        location_name = reader["location_name"].ToString(),
                                        wh_type_id = (Guid)(reader["wh_type_id"]),
                                        wh_type_name = reader["wh_type_name"].ToString(),
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
                    return NotFound("No item categories found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpPost("addWarehouse")]
        public IActionResult AddWarehouse(AddWarehouseModel addWarehouse)
        {
            try
            {
                string message;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.WarehouseInsert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", addWarehouse.user_id);
                        command.Parameters.AddWithValue("@wh_name", addWarehouse.wh_name);
                        //command.Parameters.AddWithValue("@address", addWarehouse.address);
                        //command.Parameters.AddWithValue("@city", addWarehouse.city);
                        //command.Parameters.AddWithValue("@state_id", addWarehouse.state_id);
                        //command.Parameters.AddWithValue("@country_id", addWarehouse.country_id);
                        command.Parameters.AddWithValue("@location_id", addWarehouse.location_id);
                        command.Parameters.AddWithValue("@wh_type_id", addWarehouse.wh_type_id);
                    
                      
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
                if (message.StartsWith("Warehouse  inserted successfully."))
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

        [HttpPut("edit/{warehouse_id}")]
        public IActionResult EditItemCategory(Guid warehouse_id, [FromBody] EditWarehouseModel editItemCategory)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.WarehouseEdit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", editItemCategory.user_id);
                        command.Parameters.AddWithValue("@warehouse_id", warehouse_id);
                        command.Parameters.AddWithValue("@warehouse_name", editItemCategory.wh_name);
                        //command.Parameters.AddWithValue("@address", editItemCategory.address);
                        //command.Parameters.AddWithValue("@city", editItemCategory.city);
                        //command.Parameters.AddWithValue("@state_id", editItemCategory.state_id);
                        //command.Parameters.AddWithValue("@country_id", editItemCategory.country_id);
                        command.Parameters.AddWithValue("@location_id", editItemCategory.location_id);
                        command.Parameters.AddWithValue("@wh_type_id", editItemCategory.wh_type_id);
                        command.Parameters.AddWithValue("@is_active", editItemCategory.is_active);

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

        [HttpPut("delete/{warehouse_id}")]
        public IActionResult DeleteItemCategory(Guid warehouse_id, [FromBody] DeleteWarehouseModel deleteItemCategory)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.WarehouseDelete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", deleteItemCategory.user_id);
                        command.Parameters.AddWithValue("@warehouse_id", warehouse_id);

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
