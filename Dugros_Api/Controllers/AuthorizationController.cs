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
    public class AuthorizationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public AuthorizationController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class GetAuth
        {
            
            public Guid module_id { get; set; }
            public Guid assn_user_id { get; set; }
            public int access_view { get; set; }
            public int access_add { get; set; }
            public int access_edit { get; set; }
            public int access_delete { get; set; }
            public string module_name { get; set; }
            public Guid menu_id { get; set; }
            public string url { get; set; }
            public string icon_svg { get; set; }
        }

        public class PostAuth
        {
            public Guid user_id { get; set; }
            
            public Guid assn_user_id { get; set; }
           
            public List<permissions> access { get; set; }
        }

        public class permissions
        {
            public Guid module_id { get; set; }
            public int access_view { get; set; }
            public int access_add { get; set; }
            public int access_edit { get; set; }
            public int access_delete { get; set; }
        }
        public class EditAuth
        {
            public Guid user_id { get; set; }
            public Guid assn_user_id { get; set; }
             public List<permissions> access { get; set; }
        }

        [HttpGet]
        public IActionResult Get_Auth(Guid userId, string param)
        {
            try
            {

                List<GetAuth> itemCategories = new List<GetAuth>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.AuthGet", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@param", param);

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
                                    GetAuth color = new GetAuth
                                    {
                                        module_id = (Guid)reader["module_id"],
                                        assn_user_id = (Guid)reader["user_id"],
                                        access_view = Convert.ToInt32(reader["access_view"]),
                                        access_add = Convert.ToInt32(reader["access_add"]),
                                        access_edit = Convert.ToInt32(reader["access_edit"]),
                                        access_delete = Convert.ToInt32(reader["access_delete"]),
                                        module_name = reader["module_name"].ToString(),
                                        menu_id = (Guid)reader["menu_id"],
                                        url = reader["url"].ToString(),
                                        icon_svg = reader["icon"].ToString()

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
                    return NotFound("No colors found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.StackTrace}");
            }
        }

        [HttpPost("addAuth")]
        public IActionResult AddAuth(PostAuth postAuth)
        {
            try
            {
                string message = string.Empty;

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    foreach (var permission in postAuth.access)
                    {
                        using (var command = new SqlCommand("dbo.AuthInsert", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@user_id", postAuth.user_id);
                            command.Parameters.AddWithValue("@module_id", permission.module_id);
                            command.Parameters.AddWithValue("@assn_user_id", postAuth.assn_user_id);
                            command.Parameters.AddWithValue("@access_view", permission.access_view);
                            command.Parameters.AddWithValue("@access_add", permission.access_add);
                            command.Parameters.AddWithValue("@access_edit", permission.access_edit);
                            command.Parameters.AddWithValue("@access_delete", permission.access_delete);

                            // Add OUTPUT parameter to capture the stored procedure message
                            var outputParam = new SqlParameter("@Message", SqlDbType.NVarChar, 1000);
                            outputParam.Direction = ParameterDirection.Output;
                            command.Parameters.Add(outputParam);

                            // Execute the stored procedure
                            command.ExecuteNonQuery();

                            // Get the message from the output parameter
                            message = command.Parameters["@Message"].Value.ToString();

                            // If any iteration returns a "Not authorized" message, return immediately
                            if (message == "Not authorized.")
                            {
                                return BadRequest(message); // Return error message
                            }
                        }
                    }
                }

                // If all insertions are successful
                return Ok(new { ExecuteMessage = "Authorization inserted successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPut("editAuth")]
        public IActionResult Edit_Auth([FromBody] EditAuth editAuth)
        {
            try
            {
                string message = string.Empty;

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    foreach (var permission in editAuth.access)
                    {
                        using (var command = new SqlCommand("dbo.AuthEdit", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@user_id", editAuth.user_id);
                            command.Parameters.AddWithValue("@module_id", permission.module_id);
                            command.Parameters.AddWithValue("@assn_user_id", editAuth.assn_user_id);
                            command.Parameters.AddWithValue("@access_view", permission.access_view);
                            command.Parameters.AddWithValue("@access_add", permission.access_add);
                            command.Parameters.AddWithValue("@access_edit", permission.access_edit);
                            command.Parameters.AddWithValue("@access_delete", permission.access_delete);

                            // Add OUTPUT parameter to capture the stored procedure message
                            var outputParam = new SqlParameter("@Message", SqlDbType.NVarChar, 1000);
                            outputParam.Direction = ParameterDirection.Output;
                            command.Parameters.Add(outputParam);

                            // Execute the stored procedure
                            command.ExecuteNonQuery();

                            // Get the message from the output parameter
                            message = command.Parameters["@Message"].Value.ToString();

                            // If any iteration returns a "Not authorized" message, return immediately
                            if (message == "Not authorized.")
                            {
                                return BadRequest(message); // Return error message
                            }
                        }
                    }
                }

                // If all updates are successful
                return Ok(new { ExecuteMessage = "Authorization updated successfully." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}"); // Internal server error
            }
        }

    }
}
