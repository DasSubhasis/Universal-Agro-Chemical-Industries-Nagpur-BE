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
    public class VoucherTypeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public VoucherTypeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class GetVoucher
        {
            public string document_id { get; set; }
            public string document_type { get; set; }
        }
        [HttpGet]
        public IActionResult GetVoucherType()
        {
            try
            {

                List<GetVoucher> itemCategories = new List<GetVoucher>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.VoucherType_Get", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        //command.Parameters.AddWithValue("@user_id", userId);

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
                                    GetVoucher color = new GetVoucher
                                    {
                                        document_id = reader["voucher_id"].ToString(),
                                        document_type = reader["voucher_type"].ToString()
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
                    return NotFound("No vouchers found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("SubVoucher")]
        public IActionResult GetSubVoucherType(string abbr)
        {
            try
            {

                List<GetVoucher> itemCategories = new List<GetVoucher>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.VoucherType_GetSubVoucher", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        //command.Parameters.AddWithValue("@user_id", userId);

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
                                    GetVoucher color = new GetVoucher
                                    {
                                        document_id = reader["voucher_id"].ToString(),
                                        document_type = reader["voucher_name"].ToString()
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
                    return NotFound("No sub vouchers found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("PB/SubVoucher")]
        public IActionResult GetPBSubVoucherType(string abbr)
        {
            try
            {

                List<GetVoucher> itemCategories = new List<GetVoucher>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PurchaseBill_GetSubVoucher", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        //command.Parameters.AddWithValue("@user_id", userId);

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
                                    GetVoucher color = new GetVoucher
                                    {
                                        document_id = reader["voucher_id"].ToString(),
                                        document_type = reader["voucher_name"].ToString()
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
                    return NotFound("No sub vouchers found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
