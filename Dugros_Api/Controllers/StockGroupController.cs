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
    public class StockGroupController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StockGroupController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        [HttpGet]
        public IActionResult GetStockGroups(Guid userId)
        {
            try
            {
                List<StockGroupModel> stockGroups = new List<StockGroupModel>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.sp_mst_Stock_Group_GET", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    StockGroupModel stockGroup = new StockGroupModel
                                    {
                                        ID = reader["ID"].ToString(),
                                        Name = reader["Name"].ToString(),
                                        ParentID = reader["ParentID"].ToString(),
                                        Parent = reader["Parent"].ToString(),
                                        IsAddable = (bool)reader["IsAddable"],
                                        FirstAlias = reader["FirstAlias"].ToString(),
                                        HSNDescription = reader["HSNDescription"].ToString(),
                                        HSNCode = reader["HSNCode"].ToString(),
                                        IntegratedTax = reader["IntegratedTax"].ToString(),
                                        CentralTax = reader["CentralTax"].ToString(),
                                        StateTax = reader["StateTax"].ToString(),
                                        Cess = reader["Cess"].ToString(),
                                        LastModifiedBy = reader["LastModifiedBy"].ToString(),
                                        Group_Level = reader["Group_Level"].ToString()
                                    };

                                    stockGroups.Add(stockGroup);
                                }
                            }
                        }
                    }
                }

                if (stockGroups.Any())
                {
                    return Ok(stockGroups);
                }
                else
                {
                    return NotFound("No stock groups found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
        public class StockGroupModel
        {
            public string ID { get; set; }
            public string Name { get; set; }
            public string ParentID { get; set; }
            public string Parent { get; set; }
            public bool IsAddable { get; set; }
            public string FirstAlias { get; set; }
            public string HSNDescription { get; set; }
            public string HSNCode { get; set; }
            public string IntegratedTax { get; set; }
            public string CentralTax { get; set; }
            public string StateTax { get; set; }
            public string Cess { get; set; }
            public string LastModifiedBy { get; set; }
            public string Group_Level { get; set; }
        }
    }
}
