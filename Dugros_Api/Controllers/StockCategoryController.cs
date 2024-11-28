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
    public class StockCategoryController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StockCategoryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class StockCategoryModel
        {
            public string id { get; set; }
            public string name { get; set; }
        }
        [HttpGet("GetCategories")]
        public IActionResult GetStockCategories(Guid userId)
        {
            try
            {
                List<StockCategoryModel> stockCategories = new List<StockCategoryModel>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.sp_mst_Stock_Category_GET", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    StockCategoryModel category = new StockCategoryModel
                                    {
                                        id = reader["id"].ToString(),
                                        name = reader["category"].ToString(),
                                       
                                    };

                                    stockCategories.Add(category);
                                }
                            }
                        }
                    }
                }

                if (stockCategories.Any())
                {
                    return Ok(stockCategories);
                }
                else
                {
                    return NotFound("No stock categories found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("GetGroup")]
        public IActionResult GetGroup(Guid userId, string? category_id)
        {
            try
            {
                List<StockCategoryModel> stockCategories = new List<StockCategoryModel>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.sp_mst_Stock_Group_GET", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@category_id", category_id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    StockCategoryModel category = new StockCategoryModel
                                    {
                                        id = reader["id"].ToString(),
                                        name = reader["group_name"].ToString(),

                                    };

                                    stockCategories.Add(category);
                                }
                            }
                        }
                    }
                }

                if (stockCategories.Any())
                {
                    return Ok(stockCategories);
                }
                else
                {
                    return NotFound("No stock categories found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("GetSubGroup")]
        public IActionResult GetSubGroup(Guid userId, string? group_id)
        {
            try
            {
                List<StockCategoryModel> stockCategories = new List<StockCategoryModel>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.sp_mst_Stock_SubGroup_GET", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@group_id", group_id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    StockCategoryModel category = new StockCategoryModel
                                    {
                                        id = reader["id"].ToString(),
                                        name = reader["sub_group_name"].ToString(),

                                    };

                                    stockCategories.Add(category);
                                }
                            }
                        }
                    }
                }

                if (stockCategories.Any())
                {
                    return Ok(stockCategories);
                }
                else
                {
                    return NotFound("No stock categories found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

    }
}
