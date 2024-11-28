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
    public class HistoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public HistoryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class GetHistory
        {
            public DateTime trn_date { get; set; }
            public string item_name { get; set; }
            public string supplier_name { get; set; }
            public decimal qty { get; set; }
            public decimal rate { get; set; }
            public decimal value { get; set; }
        }



        [HttpGet]
        public IActionResult GetItemCategories()
        {
            try
            {

                List<GetHistory> itemCategories = new List<GetHistory>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.HistoryGet", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

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
                                    GetHistory color = new GetHistory
                                    {
                                        trn_date = (DateTime)reader["trn_date"],
                                        item_name = reader["particulars"].ToString(),
                                        supplier_name = reader["supplier_name"].ToString(),
                                        qty = (decimal)reader["qty"],
                                        rate = (decimal)reader["rate"],
                                        value = (decimal)reader["value"],
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

        [HttpGet("ByItem")]
        public IActionResult GetItemCategoriesById(Guid trn_id)
        {
            try
            {

                List<GetHistory> itemCategories = new List<GetHistory>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.HistoryGetById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@trn_id", trn_id);

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
                                    GetHistory color = new GetHistory
                                    {
                                        trn_date = (DateTime)reader["trn_date"],
                                        item_name = reader["particulars"].ToString(),
                                        supplier_name = reader["supplier_name"].ToString(),
                                        qty = (decimal)reader["qty"],
                                        rate = (decimal)reader["rate"],
                                        value = (decimal)reader["value"],
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
