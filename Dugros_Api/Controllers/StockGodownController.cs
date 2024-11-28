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
    public class StockGodownController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StockGodownController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class GetStockGodown
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string ParentID { get; set; }
            public string ParentName { get; set; }
            public bool HasNoSpace { get; set; }
            public bool IsExternal { get; set; }
            public bool IsInternal { get; set; }
            public string LastUpdatedBy { get; set; }
            public string Alias { get; set; }
            public string GroupLevel { get; set; }
        }

        [HttpGet]
        public IActionResult GetStockGodowns(Guid userId)
        {
            try
            {
                List<GetStockGodown> stockGodowns = new List<GetStockGodown>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.sp_mst_Stock_Godown_GET", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    GetStockGodown godown = new GetStockGodown
                                    {
                                        ID = reader["ID"].ToString(),
                                        Name = reader["Name"].ToString(),
                                        Address = reader["Address"].ToString(),
                                        ParentID = reader["ParentID"].ToString(),
                                        ParentName = reader["ParentName"].ToString(),
                                        HasNoSpace = (bool)reader["HasNoSpace"],
                                        IsExternal = (bool)reader["IsExternal"],
                                        IsInternal = (bool)reader["IsInternal"],
                                        LastUpdatedBy = reader["LastUpdatedBy"].ToString(),
                                        Alias = reader["Alias"].ToString(),
                                        GroupLevel = reader["GroupLevel"].ToString()
                                    };

                                    stockGodowns.Add(godown);
                                }
                            }
                        }
                    }
                }

                if (stockGodowns.Any())
                {
                    return Ok(stockGodowns);
                }
                else
                {
                    return NotFound("No stock godowns found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
