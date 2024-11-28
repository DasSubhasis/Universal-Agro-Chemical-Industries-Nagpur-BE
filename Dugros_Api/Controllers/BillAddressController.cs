using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using static Dugros_Api.Controllers.ChequeController;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;

namespace Dugros_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BillAddressController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public BillAddressController(IConfiguration configuration)
        {
            _configuration = configuration;

        }

        public class BillAddress
        {
            public Guid org_id { get; set; }
            public string bill_address { get; set; }
            public Guid bill_state { get; set; }
            public string bill_pin { get; set; }
            public string gst_no { get; set; }
            public string pan_no { get; set; }
        }

        public class BillAddressEdit
        {
            public Guid id { get; set; }
            public Guid org_id { get; set; }
            public string bill_address { get; set; }
            public Guid bill_state { get; set; }
            public string bill_pin { get; set; }
            public string gst_no { get; set; }
            public string pan_no { get; set; }
        }

        public class BillAddressGet
        {
            public Guid id { get; set; }
            public Guid org_id { get; set; }
            public string org_name { get; set; }
            public string bill_address { get; set; }
            public Guid bill_state { get; set; }
            public string state_name { get; set; }
            public string bill_pin { get; set; }
            public string gst_no { get; set; }
            public string pan_no { get; set; }
        }

        public class FileInsert
        {
            public string FileName { get; set; }
            public string Path { get; set; }
        }


        [HttpPost("InsertBillAddress")]
        public async Task<IActionResult> InsertBillAddress([FromBody] BillAddress company, Guid user_id)
        {
            string executeMessage = string.Empty;

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("dbo.BillAddress_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the command
                        command.Parameters.AddWithValue("@user_id", user_id);
                        command.Parameters.AddWithValue("@org_id", company.org_id);
                        command.Parameters.AddWithValue("@bill_address", company.bill_address);
                        command.Parameters.AddWithValue("@bill_state", company.bill_state);
                        command.Parameters.AddWithValue("@bill_pin", company.bill_pin);
                        command.Parameters.AddWithValue("@gst_no", company.gst_no);
                        command.Parameters.AddWithValue("@pan_no", company.pan_no);
                       


                        // Output parameter for the execution message
                        SqlParameter outputParam = new SqlParameter("@executeMessage", SqlDbType.NVarChar, 1000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        // Execute the stored procedure
                        await command.ExecuteNonQueryAsync();

                        // Get the output message
                        executeMessage = outputParam.Value.ToString();
                    }
                }
                if (executeMessage.Contains("inserted"))
                {
                    return Ok(new { message = executeMessage });
                }
                return StatusCode(400,new { message = executeMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("UpdateBillAddress")]
        public async Task<IActionResult> UpdateBillAddress([FromBody] BillAddressEdit company, Guid user_id)
        {
            string executeMessage = string.Empty;

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("dbo.BillAddress_Edit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the command

                        command.Parameters.AddWithValue("@user_id", user_id);
                        command.Parameters.AddWithValue("@id", company.id);
                        command.Parameters.AddWithValue("@org_id", company.org_id);
                        command.Parameters.AddWithValue("@bill_address", company.bill_address);
                        command.Parameters.AddWithValue("@bill_state", company.bill_state);
                        command.Parameters.AddWithValue("@bill_pin", company.bill_pin);
                        command.Parameters.AddWithValue("@gst_no", company.gst_no);
                        command.Parameters.AddWithValue("@pan_no", company.pan_no);
                       


                        // Output parameter for the execution message
                        SqlParameter outputParam = new SqlParameter("@executeMessage", SqlDbType.NVarChar, 1000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        // Execute the stored procedure
                        await command.ExecuteNonQueryAsync();

                        // Get the output message
                        executeMessage = outputParam.Value.ToString();
                    }
                }
                if (executeMessage.Contains("updated"))
                {
                    return Ok(new { message = executeMessage });
                }
                return StatusCode(400,new { message = executeMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("GetAllBillingAddress")]
        public IActionResult GetAllCompanies(Guid user_id)
        {
            string executeMessage = string.Empty;
            List<BillAddressGet> companies = new List<BillAddressGet>();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.BillingAddress_Get", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", user_id);
                        // Add OUTPUT parameter to capture the stored procedure's execution message
                        SqlParameter outputParam = new SqlParameter("@executeMessage", SqlDbType.NVarChar, -1);
                        outputParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputParam);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    BillAddressGet company = new BillAddressGet
                                    {
                                        id = (Guid)reader["id"],
                                        org_id = (Guid)reader["org_id"],
                                        org_name = reader["org_name"].ToString(),
                                        bill_address = reader["bill_address"].ToString(),
                                        bill_state  = (Guid)reader["bill_state"],
                                        state_name  = reader["state_name"].ToString(),
                                        bill_pin  = reader["bill_pin"].ToString(),
                                        gst_no   = reader["gst_no"].ToString(),
                                        pan_no   = reader["pan_no"].ToString(),
                                        //org_logo = reader["org_logo"] != DBNull.Value ? reader["org_logo"].ToString() : null,
                                    };
                                    
                                    companies.Add(company);
                                }
                            }
                        }

                        // Get the execution message from the stored procedure
                        executeMessage = outputParam.Value.ToString();
                    }
                }

                // Return the list of companies and the execution message
                if (companies.Count > 0)
                {
                    return Ok(new { Message = executeMessage, Data = companies });
                }
                else
                {
                    return NotFound(new { Message = "No companies found." });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Error: {ex.Message}" });
            }
        }

        [HttpPost("DeleteBillAddress")]
        public async Task<IActionResult> DeleteCompany( Guid id, Guid user_id)
        {
            string executeMessage = string.Empty;

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("dbo.BillAddress_Delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the command

                        command.Parameters.AddWithValue("@user_id", user_id);
                        command.Parameters.AddWithValue("@id", id);
                       



                        // Output parameter for the execution message
                        SqlParameter outputParam = new SqlParameter("@executeMessage", SqlDbType.NVarChar, 1000)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputParam);

                        // Execute the stored procedure
                        await command.ExecuteNonQueryAsync();

                        // Get the output message
                        executeMessage = outputParam.Value.ToString();
                    }
                }

                if(executeMessage.StartsWith("Billing Address deactivated successfully."))
                {
                    return Ok(new { message = executeMessage });
                }
                return StatusCode(400, new { message = executeMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

    }

}
