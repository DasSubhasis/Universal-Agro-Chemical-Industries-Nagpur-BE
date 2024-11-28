
using Dugros_Api.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using static Dugros_Api.Controllers.ColorController;
using static Dugros_Api.Controllers.PurchaseOrderController;

namespace Dugros_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public UserController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class GetUser
        {
            public Guid user_id { get; set; }
            public string login_id { get; set; }
            public string email_id { get; set; }
            public string full_name { get; set; }
            public int is_active { get; set; }
            public string warehouse_id { get; set; }
            public string warehouse_name { get; set; }
            public Guid role_id { get; set; }
            public string role_name { get; set; }
            public string mobile_no { get; set; }
        }
        public class AuthModel
        {
            public string login_id { get; set; }
            public string email_id { get; set; }
            public string mobile_no { get; set; }
            public string password { get; set; }
            public string full_name { get; set; }
            public Guid role_id { get; set; }

            public List<assign_warehouse> warehouses { get; set; }
        }

        public class EditUserModel
        {
            public Guid user_id { get; set; } // Assuming the user is identified by a unique ID
            //public string login_id { get; set; }
            public string email_id { get; set; }
            public string mobile_no { get; set; }
            //public string password { get; set; }
            public string full_name { get; set; }
            public Guid role_id { get; set; }

            public List<assign_warehouse> warehouses { get; set; }
        }
        public class assign_warehouse
        {
            public string warehouse_id { get; set; }
        }

        public class LoginModel
        {
            public string login_id { get; set; }
            public string Password { get; set; }
        }

        public class ChangePasswordModel
        {
            public string new_password { get; set; }
        }

        public class ResetPasswordRequest
        {
            public Guid UserId { get; set; }
            public string LoginId { get; set; }
        }

        public class ResetPasswordResponse
        {
            public int ResultCode { get; set; }
            public string ResultMessage { get; set; }
            public string NewPassword { get; set; }
        }


        [HttpGet]
        public IActionResult GetUsers(Guid userId)
        {
            try
            {

                List<GetUser> itemCategories = new List<GetUser>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.UserGet", connection))
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
                                    GetUser color = new GetUser
                                    {
                                        user_id = (Guid)reader["user_id"],
                                        login_id = reader["login_id"].ToString(),
                                        email_id = reader["email_id"].ToString(),
                                        full_name = reader["full_name"].ToString(),
                                        is_active = Convert.ToInt32(reader["isActive"]),
                                        warehouse_id = reader["warehouse_id"].ToString(),
                                        warehouse_name = reader["warehouse_name"].ToString(),
                                        role_id = (Guid)reader["role_id"],
                                        role_name = reader["role_name"].ToString(),
                                        mobile_no = reader["mobile_no"].ToString(),
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
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("register")]
        public IActionResult RegisterUser(AuthModel auth)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Initialize output parameters for success and error messages
                    SqlParameter successMessageParam = new SqlParameter("@SuccessMessage", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output };
                    SqlParameter errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, -1) { Direction = ParameterDirection.Output };

                    // Register the user by calling the stored procedure
                    using (var command = new SqlCommand("dbo.UserRegister", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@login_id", auth.login_id);
                        command.Parameters.AddWithValue("@email_id", auth.email_id);
                        command.Parameters.AddWithValue("@mobile_no", auth.mobile_no);
                        command.Parameters.AddWithValue("@password", auth.password);
                        command.Parameters.AddWithValue("@full_name", auth.full_name);
                        command.Parameters.AddWithValue("@role_id", auth.role_id);

                        string warehouse = Newtonsoft.Json.JsonConvert.SerializeObject(auth.warehouses);
                        command.Parameters.AddWithValue("@warehouse_id", warehouse);

                        // Add output parameters for success and error messages
                        command.Parameters.Add(successMessageParam);
                        command.Parameters.Add(errorMessageParam);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();
                    }

                    // Retrieve output messages
                    string successMessage = successMessageParam.Value.ToString();
                    string errorMessage = errorMessageParam.Value.ToString();

                    // Check if there is an error message
                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        return Conflict(new { executeMessage = errorMessage });
                    }

                    // If no error, return success message
                    return Ok(new { executeMessage = successMessage });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }



        [HttpPost("edit")]
        public IActionResult EditUser(EditUserModel user)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Check if the user exists
                    using (var checkCommand = new SqlCommand("SELECT COUNT(*) FROM tbl_user WHERE user_id = @UserId", connection))
                    {
                        checkCommand.Parameters.AddWithValue("@UserId", user.user_id);
                        int existingCount = (int)checkCommand.ExecuteScalar();

                        if (existingCount == 0)
                        {
                            return NotFound("User not found.");
                        }
                    }

                    // Update user details
                    using (var command = new SqlCommand("dbo.UserEdit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", user.user_id);
                        command.Parameters.AddWithValue("@email_id", user.email_id);
                        //command.Parameters.AddWithValue("@email_id", user.email_id);
                        command.Parameters.AddWithValue("@mobile_no", user.mobile_no);
                        //command.Parameters.AddWithValue("@password", user.password);
                        command.Parameters.AddWithValue("@full_name", user.full_name);
                        command.Parameters.AddWithValue("@role_id", user.role_id);
                        string warehouse = Newtonsoft.Json.JsonConvert.SerializeObject(user.warehouses);
                        command.Parameters.AddWithValue("@warehouse_id", warehouse);
                        command.ExecuteNonQuery();
                    }
                }

                return Ok(new { executeMessage = "User details updated successfully!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("delete/{user_id}")]
        public IActionResult DeleteUser(Guid user_id)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Execute the stored procedure to soft delete the user
                    using (var command = new SqlCommand("dbo.UserDelete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", user_id);

                        // Add output parameter for the message
                        var outputMessage = new SqlParameter("@OutputMessage", SqlDbType.NVarChar, 100)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputMessage);

                        command.ExecuteNonQuery();

                        // Retrieve the output message from the stored procedure
                        var executeMessage = outputMessage.Value.ToString();
                        return Ok(new { executeMessage });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { executeMessage = $"Error: {ex.Message}" });
            }
        }


        [HttpPost("login")]
        public IActionResult LoginUser(LoginModel login)
        {

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Execute the stored procedure for user login
                    using (var command = new SqlCommand("dbo.UserLogin", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@login_id", login.login_id);
                        command.Parameters.AddWithValue("@password", login.Password);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // User login successful
                                var userId = reader.GetGuid(reader.GetOrdinal("UserId"));
                                var name = reader["user_name"].ToString();
                                var warehouse = reader["warehouse"].ToString();
                                var org_name = reader["org_name"].ToString();
                                var state = reader["state"].ToString();
                                var token = GenerateJwtToken(userId);

                                return Ok(new { UserId = userId, name = name, Token = token, org_name = org_name, warehouse = warehouse, state = state });
                            }
                            else
                            {
                                // User login failed
                                return Unauthorized("Invalid email or password.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log detailed error
                Console.WriteLine(ex.ToString());
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        // Method to generate JWT token with auto-renewal
        private string GenerateJwtToken(Guid userId)
        {
            // var secretKey = _configuration["Jwt:Key"];
            // var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            // var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // var token = new JwtSecurityToken(
            //     issuer: _configuration["Jwt:Issuer"],
            //     audience: _configuration["Jwt:Issuer"],
            //     claims: new[] { new Claim(ClaimTypes.Name, userId.ToString()) },
            //     expires: DateTime.UtcNow.AddDays(7),
            //     signingCredentials: credentials);

            // var tokenHandler = new JwtSecurityTokenHandler();
            // var jwtToken = tokenHandler.WriteToken(token);

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // Check if the current token is near expiration
            var tokenExpirationTime = DateTime.Now.AddDays(7); // New expiration time
            var currentTime = DateTime.Now;

            // Assume the token was issued 30 days ago, check if it's near expiration
            if (currentTime >= tokenExpirationTime.AddMinutes(-5))
            {
                // Issue a new token with extended expiration
                tokenExpirationTime = DateTime.Now.AddDays(7);
            }

            var Sectoken = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Issuer"],
                claims: new[] { new Claim(ClaimTypes.Name, userId.ToString()) },
                expires: tokenExpirationTime,
                signingCredentials: credentials
            );

            var jwtToken = new JwtSecurityTokenHandler().WriteToken(Sectoken);
            Console.WriteLine(jwtToken);
            return jwtToken;
        }

        [HttpPut("change_password/{login_id}")]
        public IActionResult ChnagePassword(string login_id, [FromBody] ChangePasswordModel changePassword)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.UserPasswordChange", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@login_id", login_id);
                        command.Parameters.AddWithValue("@new_password", changePassword.new_password);
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

        [HttpPost("reset")]
        public IActionResult ResetPassword([FromBody] ResetPasswordRequest request)
        {
            // Define variable to store the new password
            string newPassword = "";

            // Your logic to call the stored procedure goes here
            // Use ADO.NET or Entity Framework Core to execute the stored procedure

            // Example using ADO.NET
            using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                connection.Open();

                using (var command = new SqlCommand("ResetPassword", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_UserId", request.UserId);
                    command.Parameters.AddWithValue("@p_LoginId", request.LoginId);

                    // Define and set output parameter
                    var newPasswordParam = command.Parameters.Add("@p_NewPassword", SqlDbType.NVarChar, 100);
                    newPasswordParam.Direction = ParameterDirection.Output;

                    // Execute the command
                    command.ExecuteNonQuery();

                    // Retrieve output parameter value
                    newPassword = newPasswordParam.Value.ToString();
                }
            }

            // Return only the new password in the response
            return Ok(new { ExecuteMessage = newPassword });
        }

    }

}
