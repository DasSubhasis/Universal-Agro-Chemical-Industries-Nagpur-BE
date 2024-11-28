using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Dugros_Api.Controllers.ColorController;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;

namespace Dugros_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseOrderAllGet : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PurchaseOrderAllGet(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class GetLOI
        {
            public Guid purchase_indent_id { get; set; }
            public string base_reference_no { get; set; }
            public string base_reference_date { get; set; }
            public string item_id { get; set; }
            public string? item_desc { get; set; }
            public string item_name { get; set; }
            public decimal indent_qty { get; set; }
            public decimal pending_qty { get; set; }
            public decimal amt_sgst { get; set; }
            public decimal amt_cgst { get; set; }
            public decimal amt_igst { get; set; }
            public string uom_primary {get; set;}
            public string uom_name {get; set;}
            public decimal rate {get;set;}
            public string tax_type { get; set; }
            public string bill_gst_reg_type { get; set; }
            public string shp_gst_reg_type { get; set; }
            public decimal mrp { get; set; }
            public List<fileUpload1> files1 { get; set; }
        }

        public class fileUpload1
        {
            public string url_path { get; set; }
            public string filename { get; set; }
        }
        public class GetLedger
        {
            public string ledger_id { get; set; }
            public string ledger_name { get; set; }
            
            public decimal amt_sgst { get; set; }
            public decimal amt_cgst { get; set; }
            public decimal amt_igst { get; set; }
            public string tax_type { get; set; }
            public string bill_gst_reg_type { get; set; }
            public string shp_gst_reg_type { get; set; }

        }

        public class loi_filters
        {
            public Guid user_id { get; set; }
            public string indent_no { get; set; }
            public string item_name { get; set; }
        }
        [HttpPost]
        public IActionResult Get_LOI(loi_filters filter)
        {
            try
            {

                List<GetLOI> itemCategories = new List<GetLOI>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PurchaseOrder_LineOfItem", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id",filter.user_id);
                        command.Parameters.AddWithValue("@indent_no", filter.indent_no);
                        command.Parameters.AddWithValue("@item_name", filter.item_name);

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
                                    GetLOI color = new GetLOI
                                    {
                                        purchase_indent_id = (Guid)reader["purchase_indent_id"],
                                        base_reference_no = reader["voucher_no"].ToString(),
                                        base_reference_date = ((DateTime)reader["voucher_date"]).ToString("dd-MM-yyyy"),
                                        item_id = reader["item_id"].ToString(),
                                        item_name = reader["item_name"].ToString(),
                                        item_desc = reader["item_desc"].ToString(),
                                        amt_igst=(decimal)reader["amt_igst"],
                                        amt_cgst=(decimal)reader["amt_cgst"],
                                        amt_sgst=(decimal)reader["amt_sgst"],
                                        uom_primary=reader["uom_id"].ToString(),
                                        uom_name=reader["uom_name"].ToString(),
                                        rate=(decimal)reader["rate"],
                                        indent_qty = (decimal)reader["indent_qty"],
                                        pending_qty = (decimal)reader["pending_qty"],
                                        tax_type = reader["tax_type"].ToString(),
                                        bill_gst_reg_type = reader["bill_gst_reg_type"].ToString(),
                                        shp_gst_reg_type = reader["shp_gst_reg_type"].ToString(),
                                        mrp = (decimal)reader["mrp"],

                                    };
                                    string files1 = reader.GetString(reader.GetOrdinal("file_details"));
                                    color.files1 = JsonConvert.DeserializeObject<List<fileUpload1>>(files1);


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
                    return NotFound("No items found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("GetLedger")]
        public IActionResult Get_Ledger()
        {
            try
            {

                List<GetLedger> itemCategories = new List<GetLedger>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PurchaseOrder_GetLedger", connection))
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
                                    GetLedger color = new GetLedger
                                    {
                                        ledger_id = reader["ledger_id"].ToString(),
                                        ledger_name = reader["ledger_name"] == DBNull.Value ? null : reader["ledger_name"].ToString(),
                                      
                                        amt_sgst = reader["amt_sgst"] == DBNull.Value ? 0 : (decimal)reader["amt_sgst"],
                                        amt_cgst = reader["amt_cgst"] == DBNull.Value ? 0 : (decimal)reader["amt_cgst"],
                                        amt_igst = reader["amt_igst"] == DBNull.Value ? 0 : (decimal)reader["amt_igst"],
                                        tax_type = reader["tax_type"].ToString(),
                                        bill_gst_reg_type = reader["bill_gst_reg_type"].ToString(),
                                        shp_gst_reg_type = reader["shp_gst_reg_type"].ToString(),
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
                    return NotFound("No ledger found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
