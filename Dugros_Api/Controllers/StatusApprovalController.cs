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
    public class StatusApprovalController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StatusApprovalController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class GetStatusApproval
        {
            public Guid status_id { get; set; }
            public string approval_status { get; set; }
        }
        [HttpGet]
        public IActionResult GetItemCategories(Guid userId)
        {
            try
            {

                List<GetStatusApproval> itemCategories = new List<GetStatusApproval>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.StatusApprovalGet", connection))
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
                                    GetStatusApproval color = new GetStatusApproval
                                    {
                                        status_id = (Guid)reader["id"],
                                        approval_status = reader["approval_status"].ToString()
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
    }
}
