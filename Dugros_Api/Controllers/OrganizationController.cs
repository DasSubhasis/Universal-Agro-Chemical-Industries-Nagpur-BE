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
    public class OrganizationController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public OrganizationController(IConfiguration configuration)
        {
            _configuration = configuration;
        
        }

        public class Company
        {
            public string org_name { get; set; }
            public string org_email { get; set; }
            public string org_mobile { get; set; }
            public List<FileInsert> org_logo { get; set; }
        }

        public class CompanyEdit
        {
            public Guid org_id { get; set; }
            public string org_name { get; set; }
            public string org_email { get; set; }
            public string org_mobile { get; set; }
            public List<FileInsert> org_logo { get; set; }
        }

        public class CompanyGet
        {
            public Guid org_id { get; set; }
            public string org_name { get; set; }
            public string org_email { get; set; }
            public string org_mobile { get; set; }
            public List<FileInsert> org_logo { get; set; }
        }

        public class FileInsert
        {
            public string FileName { get; set; }
            public string Path { get; set; }
        }


        [HttpPost("InsertCompany")]
        public async Task<IActionResult> InsertCompany([FromBody] Company company, Guid user_id)
        {
            string executeMessage = string.Empty;

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("dbo.Company_Insert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the command
                        command.Parameters.AddWithValue("@user_id", user_id);
                        command.Parameters.AddWithValue("@org_name", company.org_name);
                        command.Parameters.AddWithValue("@org_email", company.org_email);
                        command.Parameters.AddWithValue("@org_mobile", company.org_mobile);
                        if (company.org_logo != null && company.org_logo.Count > 0)
                        {
                            var pdfDataJson = JsonConvert.SerializeObject(company.org_logo);
                            command.Parameters.AddWithValue("@org_logo", pdfDataJson);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@org_logo", DBNull.Value);
                        }


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

                return Ok(new { message = executeMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost("UpdateCompany")]
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyEdit company, Guid user_id)
        {
            string executeMessage = string.Empty;

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    await connection.OpenAsync();

                    using (var command = new SqlCommand("dbo.Company_Edit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters to the command
                        command.Parameters.AddWithValue("@user_id", user_id);
                        command.Parameters.AddWithValue("@org_id", company.org_id);
                        command.Parameters.AddWithValue("@org_name", company.org_name);
                        command.Parameters.AddWithValue("@org_email", company.org_email);
                        command.Parameters.AddWithValue("@org_mobile", company.org_mobile);
                        if (company.org_logo != null && company.org_logo.Count > 0)
                        {
                            var pdfDataJson = JsonConvert.SerializeObject(company.org_logo);
                            command.Parameters.AddWithValue("@org_logo", pdfDataJson);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@org_logo", DBNull.Value);
                        }


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

                return Ok(new { message = executeMessage });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpGet("GetAllCompanies")]
        public IActionResult GetAllCompanies(Guid user_id)
        {
            string executeMessage = string.Empty;
            List<CompanyGet> companies = new List<CompanyGet>();

            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.Company_Get", connection))
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
                                    CompanyGet company = new CompanyGet
                                    {
                                        org_id = (Guid)reader["org_id"],
                                        org_name = reader["org_name"].ToString(),
                                        org_email = reader["org_email"].ToString(),
                                        org_mobile = reader["org_mobile"].ToString(),
                                        //org_logo = reader["org_logo"] != DBNull.Value ? reader["org_logo"].ToString() : null,
                                    };
                                    if (!reader.IsDBNull(reader.GetOrdinal("org_logo")))
                                    {
                                        string pdfDataJson = reader["org_logo"].ToString();
                                        // Deserialize the JSON data to List<FileUpload2>
                                        List<FileInsert> pdfData = JsonConvert.DeserializeObject<List<FileInsert>>(pdfDataJson);
                                        company.org_logo = pdfData;
                                    }
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
    }

}
