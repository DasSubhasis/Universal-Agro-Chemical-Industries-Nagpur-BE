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
    public class StockItemController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public StockItemController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class StockItemModel
        {
            public string Item_ID { get; set; }
            public string Name { get; set; }
            public string ParentID { get; set; }
            public string Parent { get; set; }
            public string CategoryID { get; set; }
            public string Category { get; set; }
            public string MailingName { get; set; }
            public string Narration { get; set; }
            public string TCSApplicable { get; set; }
            public string GSTApplicable { get; set; }
            public string Description { get; set; }
            public string GSTTypeofSupply { get; set; }
            public string CostingMethod { get; set; }
            public string ValuationMethod { get; set; }
            public string Base_Unit_ID { get; set; }
            public string BaseUnits { get; set; }
            public string Additional_Unit_ID { get; set; }
            public string AdditionalUnits { get; set; }
            public string IsBatchWiseOn { get; set; }
            public string IsPerishableOn { get; set; }
            public string IgnoreNegativeStock { get; set; }
            public string TreatPurchasesasConsumed { get; set; }
            public string TreatRejectsasScrap { get; set; }
            public string HasMfgDate { get; set; }
            public string AllowUseofExpiredItems { get; set; }
            public string InclusiveTax { get; set; }
            public decimal Denominator { get; set; }
            public decimal Conversion { get; set; }
            public decimal OpeningBalance { get; set; }
            public decimal OpeningValue { get; set; }
            public decimal ReorderBase { get; set; }
            public decimal MinimumOrderBase { get; set; }
            public decimal OpeningRate { get; set; }
            public string FirstAlias { get; set; }
            public string HSNDescription { get; set; }
            public string HSNCode { get; set; }
            public decimal IntegratedTax { get; set; }
            public decimal CentralTax { get; set; }
            public decimal StateTax { get; set; }
            public decimal Cess { get; set; }
            public string ApplicableForReverseCharge { get; set; }
            public string LastUpdatedBy { get; set; }

            public string group_id { get; set; }
            public string group_name { get; set; }
            public string sub_group_id { get; set; }
            public string sub_group_name { get; set; }
        }
        [HttpGet]
        public IActionResult GetStockItems(Guid userId,string? category_id, string? group_id, string? sub_group_id )
        {
            try
            {
                List<StockItemModel> stockItems = new List<StockItemModel>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.sp_mst_Stock_Item_GET", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@category_id", category_id);
                        command.Parameters.AddWithValue("@group_id", group_id);
                        command.Parameters.AddWithValue("@sub_group_id", sub_group_id);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    StockItemModel stockItem = new StockItemModel
                                    {
                                        Item_ID = reader["Item_ID"].ToString(),
                                        Name = reader["Name"].ToString(),
                                        ParentID = reader["ParentID"].ToString(),
                                        Parent = reader["Parent"].ToString(),
                                        CategoryID = reader["CategoryID"].ToString(),
                                        Category = reader["Category"].ToString(),
                                        MailingName = reader["MailingName"].ToString(),
                                        Narration = reader["Narration"].ToString(),
                                        TCSApplicable = reader["TCSApplicable"].ToString(),
                                        GSTApplicable = reader["GSTApplicable"].ToString(),
                                        Description = reader["Description"].ToString(),
                                        GSTTypeofSupply = reader["GSTTypeofSupply"].ToString(),
                                        CostingMethod = reader["CostingMethod"].ToString(),
                                        ValuationMethod = reader["ValuationMethod"].ToString(),
                                        Base_Unit_ID =reader["Base_Unit_ID"].ToString(),
                                        BaseUnits = reader["BaseUnits"].ToString(),
                                        Additional_Unit_ID = reader["Additional_Unit_ID"].ToString(),
                                        AdditionalUnits = reader["AdditionalUnits"].ToString(),
                                        IsBatchWiseOn = reader["IsBatchWiseOn"].ToString(),
                                        IsPerishableOn = reader["IsPerishableOn"].ToString(),
                                        IgnoreNegativeStock = reader["IgnoreNegativeStock"].ToString(),
                                        TreatPurchasesasConsumed = reader["TreatPurchasesasConsumed"].ToString(),
                                        TreatRejectsasScrap = reader["TreatRejectsasScrap"].ToString(),
                                        HasMfgDate = reader["HasMfgDate"].ToString(),
                                        AllowUseofExpiredItems = reader["AllowUseofExpiredItems"].ToString(),
                                        InclusiveTax = reader["InclusiveTax"].ToString(),
                                        Denominator = (decimal)reader["Denominator"],
                                        Conversion = (decimal)reader["Conversion"],
                                        OpeningBalance = (decimal)reader["OpeningBalance"],
                                        OpeningValue = (decimal)reader["OpeningValue"],
                                        ReorderBase = (decimal)reader["ReorderBase"],
                                        MinimumOrderBase = (decimal)reader["MinimumOrderBase"],
                                        OpeningRate = (decimal)reader["OpeningRate"],
                                        FirstAlias = reader["FirstAlias"].ToString(),
                                        HSNDescription = reader["HSNDescription"].ToString(),
                                        HSNCode = reader["HSNCode"].ToString(),
                                        IntegratedTax = (decimal)reader["IntegratedTax"],
                                        CentralTax = (decimal)reader["CentralTax"],
                                        StateTax = (decimal)reader["StateTax"],
                                        Cess = (decimal)reader["Cess"],
                                        ApplicableForReverseCharge = reader["ApplicableForReverseCharge"].ToString(),
                                        LastUpdatedBy = reader["LastUpdatedBy"].ToString(),
                                        group_id = reader["group_id"].ToString(),
                                        group_name = reader["group_name"].ToString(),
                                        sub_group_id = reader["sub_group_id"].ToString(),
                                        sub_group_name = reader["sub_group_name"].ToString(),
                                    };

                                    stockItems.Add(stockItem);
                                }
                            }
                        }
                    }
                }

                if (stockItems.Count > 0)
                {
                    return Ok(stockItems);
                }
                else
                {
                    return NotFound("No stock items found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
