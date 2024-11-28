using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;

namespace Dugros_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TaxController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TaxController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class GetTax
        {
            public Guid tax_id { get; set; }
            public string tax_name { get; set; }
            public decimal tax_rate { get; set; }
            public string tax_type { get; set; }
            public string hsn_code { get; set; }
            public string description { get; set; }
            public int is_active { get; set; }
        }

        public class PostTax
        {
            public Guid user_id { get; set; }
            public string tax_name { get; set; }
            public decimal tax_rate { get; set; }
            public string tax_type { get; set; }
            public string hsn_code { get; set; }
            public string description { get; set; }
        }
        public class EditTaxModel
        {
            public Guid user_id { get; set; }
            public string tax_name { get; set; }
            public decimal tax_rate { get; set; }
            public string tax_type { get; set; }
            public string hsn_code { get; set; }
            public string description { get; set; }
            public int is_active { get; set; }
        }
        public class DeleteTaxModel
        {
            public Guid user_id { get; set; }
        }

        [HttpGet]
        public IActionResult GetItemCategories(Guid userId)
        {
            try
            {

                List<GetTax> itemCategories = new List<GetTax>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.TaxGet", connection))
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
                                    GetTax color = new GetTax
                                    {
                                        tax_id = (Guid)reader["tax_id"],
                                        tax_name = reader["tax_name"].ToString(),
                                        tax_rate = (decimal)reader["tax_rate"],
                                        tax_type = reader["tax_type"].ToString(),
                                        hsn_code = reader["hsn_code"].ToString(),
                                        description = reader["description"].ToString(),
                                        is_active = Convert.ToInt32(reader["is_active"])
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
                    return NotFound("No tax found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("addTax")]
        public IActionResult AddCountry(PostTax postTax)
        {
            try
            {
                string message;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.TaxInsert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", postTax.user_id);
                        command.Parameters.AddWithValue("@tax_name", postTax.tax_name);
                        command.Parameters.AddWithValue("@tax_rate", postTax.tax_rate);
                        command.Parameters.AddWithValue("@tax_type", postTax.tax_type);
                        command.Parameters.AddWithValue("@hsn_code", postTax.hsn_code);
                        command.Parameters.AddWithValue("@description", postTax.description);



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
                if (message.StartsWith("Tax inserted successfully."))
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

        [HttpPut("edit/{tax_id}")]
        public IActionResult EditItemCategory(Guid tax_id, [FromBody] EditTaxModel editTax)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.TaxEdit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", editTax.user_id);
                        command.Parameters.AddWithValue("@tax_id", tax_id);
                        command.Parameters.AddWithValue("@tax_name", editTax.tax_name);
                        command.Parameters.AddWithValue("@tax_rate", editTax.tax_rate);
                        command.Parameters.AddWithValue("@tax_type", editTax.tax_type);
                        command.Parameters.AddWithValue("@hsn_code", editTax.hsn_code);
                        command.Parameters.AddWithValue("@description", editTax.description);
                        command.Parameters.AddWithValue("@is_active", editTax.is_active);

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

        [HttpPut("delete/{tax_id}")]
        public IActionResult DeleteItemCategory(Guid tax_id, [FromBody] DeleteTaxModel deleteTaxModel)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.TaxDelete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", deleteTaxModel.user_id);
                        command.Parameters.AddWithValue("@tax_id", tax_id);

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
