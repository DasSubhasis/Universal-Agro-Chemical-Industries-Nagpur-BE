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
    public class GenerateVoucherNumController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public GenerateVoucherNumController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class VoucherGenerationResponse
        {
            public string VoucherNo { get; set; }
        }
        [HttpGet("{voucherTypeId}")]
        public IActionResult GetVoucherNumber(string voucherTypeId)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection"); // Replace "YourConnectionString" with your actual connection string key

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("voucher_no_generation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@voucher_abbr", voucherTypeId);

                    command.Parameters.Add("@voucher_no", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@voucher_type", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

                    connection.Open();
                    command.ExecuteNonQuery();

                    string voucherNo = command.Parameters["@voucher_no"].Value.ToString();
                    string voucherType = command.Parameters["@voucher_type"].Value.ToString();

                    var response = new VoucherGenerationResponse
                    {
                        VoucherNo = voucherNo
                    };

                    return Ok(new { response });
                }
            }
        }

        [HttpGet("Generate/{subvoucherTypeId}")]
        public IActionResult GetSubVoucherNumber(string subvoucherTypeId)
        {
            string connectionString = _configuration.GetConnectionString("DefaultConnection"); // Replace "YourConnectionString" with your actual connection string key

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                using (SqlCommand command = new SqlCommand("subvoucher_no_generation", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@sub_voucher_id", subvoucherTypeId);

                    command.Parameters.Add("@voucher_no", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;
                    command.Parameters.Add("@sub_voucher_type", SqlDbType.NVarChar, 50).Direction = ParameterDirection.Output;

                    connection.Open();
                    command.ExecuteNonQuery();

                    string voucherNo = command.Parameters["@voucher_no"].Value.ToString();
                    string voucherType = command.Parameters["@sub_voucher_type"].Value.ToString();

                    var response = new VoucherGenerationResponse
                    {
                        VoucherNo = voucherNo
                    };

                    return Ok(new { response });
                }
            }
        }
    }
}
