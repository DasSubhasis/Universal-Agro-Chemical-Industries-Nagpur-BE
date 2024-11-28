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
    public class ModuleController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ModuleController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class GetModule
        {
            public Guid module_id { get; set; }
            public string module_name { get; set; }
            public Guid menu_id { get; set; }
            public string url { get; set; }
        }

        public class PostModule
        {
            public Guid user_id { get; set; }
            public string module_name { get; set; }
            public string desc { get; set; }
            public Guid menu_id { get; set; }
            public string  url { get; set; }
        }
        public class EditModule
        {
            public Guid user_id { get; set; }
            public string module_name { get; set; }
            public string desc { get; set; }
            public int is_active { get; set; }
        }
        public class DeleteModule
        {
            public Guid user_id { get; set; }
        }

        [HttpGet]
        public IActionResult GetModules(Guid userId)
        {
            try
            {

                List<GetModule> itemCategories = new List<GetModule>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.ModuleGet", connection))
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
                                    GetModule color = new GetModule
                                    {
                                        module_id = reader.IsDBNull(reader.GetOrdinal("module_id")) ? Guid.Empty : (Guid)reader["module_id"],
                                        module_name = reader.IsDBNull(reader.GetOrdinal("module_name")) ? string.Empty : reader["module_name"].ToString(),
                                        menu_id = reader.IsDBNull(reader.GetOrdinal("menu_id")) ? Guid.Empty : (Guid)reader["menu_id"],
                                        url = reader.IsDBNull(reader.GetOrdinal("url")) ? string.Empty : reader["url"].ToString()
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
                    return NotFound("No modules found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("addModule")]
        public IActionResult AddModule(PostModule postModule)
        {
            try
            {
                string message;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.ModuleInsert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", postModule.user_id);
                        command.Parameters.AddWithValue("@module_name", postModule.module_name);
                        command.Parameters.AddWithValue("@desc", postModule.desc);
                        command.Parameters.AddWithValue("@menu_id", postModule.menu_id);
                        command.Parameters.AddWithValue("@url", postModule.url);



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
                if (message.StartsWith("Module inserted successfully."))
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

        [HttpPut("edit/{module_id}")]
        public IActionResult Edit_Module(Guid module_id, [FromBody] EditModule editModule)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.ModuleEdit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", editModule.user_id);
                        command.Parameters.AddWithValue("@module_id", module_id);
                        command.Parameters.AddWithValue("@module_name", editModule.module_name);
                        command.Parameters.AddWithValue("@desc", editModule.desc);
                        command.Parameters.AddWithValue("@is_active", editModule.is_active);

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

        [HttpPut("delete/{module_id}")]
        public IActionResult Delete_Module(Guid module_id, [FromBody] DeleteModule deleteModule)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.ModuleDelete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", deleteModule.user_id);
                        command.Parameters.AddWithValue("@module_id", module_id);

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
