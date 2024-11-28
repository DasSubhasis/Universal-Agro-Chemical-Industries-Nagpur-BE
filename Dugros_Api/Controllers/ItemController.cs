using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Dugros_Api.Controllers.ColorController;
using System.Data.SqlClient;
using System.Data;
using static Dugros_Api.Controllers.ItemController;

namespace Dugros_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ItemController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class GetItem
        {
            public string item_id { get; set; }
            public string item_code { get; set; }
            public string item_name { get; set; }
            public string item_description { get; set; }
            public string item_type_id { get; set; }
            public string type_name { get; set; }
            public string item_category_id { get; set; }
            public string category_name { get; set; }
            public string color_id { get; set; }
            public string color_name { get; set; }
            public string uom_id { get; set; }
            public string uom_name { get; set; }
            public int purchase_price { get; set; }
            public int selling_price { get; set; }
            public int stock_level_min { get; set; }
            public int stock_level_reorder { get; set; }
            public int lead_time { get; set; }
            public string img_url { get; set; }
            public string tax_id { get; set; }
            public string tax_name { get; set; }

            public int is_active { get; set; }
        }
        public class GetItemById
        {
            public string warehouse { get; set; }
            public string item_id { get; set; }
            public string item_code { get; set; }
            public string item_name { get; set; }
            public string item_description { get; set; }
            public string item_type_id { get; set; }
            public string type_name { get; set; }
            public string item_category_id { get; set; }
            public string category_name { get; set; }
            public string color_id { get; set; }
            public string color_name { get; set; }
            public string uom_id { get; set; }
            public string uom_name { get; set; }
            public int purchase_price { get; set; }
            public int selling_price { get; set; }
            public int stock_level_min { get; set; }
            public int stock_level_reorder { get; set; }
            public int lead_time { get; set; }
            public string img_url { get; set; }
            public string tax_id { get; set; }
            public string tax_name { get; set; }
            public int is_active { get; set; }
            public int avl_stock { get; set; }

            public decimal in_stock_qty { get; set; }
            public decimal pending_receipt { get; set; }
            public decimal total_qty { get; set; }
        }
        public class PostItem
        {
            public Guid user_id { get; set; }
            public string item_code { get; set; }
            public string item_name { get; set; }
            public string item_description { get; set; }
            public Guid item_type_id { get; set; }
            public Guid item_category_id { get; set; }
            public Guid color_id { get; set; }
            public Guid uom_id { get; set; }
            public decimal purchase_price { get; set; }
            public decimal selling_price { get; set; }
            public int stock_level_min { get; set; }
            public int stock_level_reorder { get; set; }
            public int lead_time { get; set; }
            public string img_url { get; set; }
            public Guid tax_id { get; set; }
        }
        public class EditItem
        {
            public Guid user_id { get; set; }
            public string item_code { get; set; }
            public string item_name { get; set; }
            public string item_description { get; set; }
            public Guid item_type_id { get; set; }
            public Guid item_category_id { get; set; }
            public Guid color_id { get; set; }
            public Guid uom_id { get; set; }
            public decimal purchase_price { get; set; }
            public decimal selling_price { get; set; }
            public int stock_level_min { get; set; }
            public int stock_level_reorder { get; set; }
            public int lead_time { get; set; }
            public string img_url { get; set; }
            public Guid tax_id { get; set; }
            public int is_active { get; set; }
        }
        public class DeleteItem
        {
            public Guid user_id { get; set; }
        }

        [HttpGet]
        public IActionResult Get_Items(Guid userId)
        {
            try
            {

                List<GetItem> itemCategories = new List<GetItem>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.ItemGet", connection))
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
                                    GetItem color = new GetItem
                                    {
                                        item_id = reader["item_id"].ToString(),
                                        item_code = reader["item_code"].ToString(),
                                        item_name = reader["item_name"].ToString(),
                                        item_description = reader["item_description"].ToString(),
                                        item_type_id = reader["item_type_id"].ToString(),
                                        type_name = reader["type_name"].ToString(),
                                        item_category_id = reader["item_category_id"].ToString(),
                                        category_name = reader["category_name"].ToString(),
                                        color_id = reader["color_id"].ToString(),
                                        color_name = reader["color_name"].ToString(),
                                        uom_id = reader["uom_id"].ToString(),
                                        uom_name = reader["uom_name"].ToString(),
                                        purchase_price = Convert.ToInt32(reader["purchase_price"]),
                                        selling_price = Convert.ToInt32(reader["selling_price"]),
                                        stock_level_min = Convert.ToInt32(reader["stock_level_min"]),
                                        stock_level_reorder = Convert.ToInt32(reader["stock_level_reorder"]),
                                        lead_time = Convert.ToInt32(reader["lead_time"]),
                                        img_url = reader["img_url"].ToString(),
                                        tax_id = reader["tax_id"].ToString(),
                                        tax_name = reader["tax_name"].ToString(),
                                        is_active = Convert.ToInt32(reader["is_active"])
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
                    return NotFound("No items found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("id")]
        public IActionResult Get_Items_By_Id(Guid userId,string item_id,string godown_id)
        {
            try
            {

                List<GetItemById> itemCategories = new List<GetItemById>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.ItemGetById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@item_id", item_id);
                        command.Parameters.AddWithValue("@godown_id", godown_id);

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
                                    GetItemById color = new GetItemById
                                    {
                                        warehouse = reader["warehouse"].ToString(),
                                        item_id = reader["item_id"].ToString(),
                                        item_code = reader["item_code"].ToString(),
                                        item_name = reader["item_name"].ToString(),
                                        item_description = reader["item_description"].ToString(),
                                        item_type_id = reader["item_type_id"].ToString(),
                                        type_name = reader["type_name"].ToString(),
                                        item_category_id = reader["item_category_id"].ToString(),
                                        category_name = reader["category_name"].ToString(),
                                        color_id = reader["color_id"].ToString(),
                                        color_name = reader["color_name"].ToString(),
                                        uom_id = reader["uom_id"].ToString(),
                                        uom_name = reader["uom_name"].ToString(),
                                        purchase_price = Convert.ToInt32(reader["purchase_price"]),
                                        selling_price = Convert.ToInt32(reader["selling_price"]),
                                        stock_level_min = Convert.ToInt32(reader["stock_level_min"]),
                                        stock_level_reorder = Convert.ToInt32(reader["stock_level_reorder"]),
                                        lead_time = Convert.ToInt32(reader["lead_time"]),
                                        img_url = reader["img_url"].ToString(),
                                        tax_id = reader["tax_id"].ToString(),
                                        tax_name = reader["tax_name"].ToString(),
                                        is_active = Convert.ToInt32(reader["is_active"]),
                                        avl_stock = Convert.ToInt32(reader["avl_stock"]),
                                        in_stock_qty = Convert.ToInt32(reader["in_stock_qty"]),
                                        pending_receipt = Convert.ToInt32(reader["pending_receipt"]),
                                        total_qty = Convert.ToInt32(reader["total_qty"]),
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
                    return NotFound("No items found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("addItem")]
        public IActionResult Add_Items(PostItem postItem)
        {
            try
            {
                string message;
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.ItemInsert", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", postItem.user_id);
                        command.Parameters.AddWithValue("@item_code", postItem.item_code);
                        command.Parameters.AddWithValue("@item_name", postItem.item_name);
                        command.Parameters.AddWithValue("@item_description", postItem.item_description);
                        command.Parameters.AddWithValue("@item_type_id", postItem.item_type_id);
                        command.Parameters.AddWithValue("@item_category_id", postItem.item_category_id);
                        command.Parameters.AddWithValue("@color_id", postItem.color_id);
                        command.Parameters.AddWithValue("@uom_id", postItem.uom_id);
                        command.Parameters.AddWithValue("@purchase_price", postItem.purchase_price);
                        command.Parameters.AddWithValue("@selling_price", postItem.selling_price);
                        command.Parameters.AddWithValue("@stock_level_min", postItem.stock_level_min);
                        command.Parameters.AddWithValue("@stock_level_reorder", postItem.stock_level_reorder);
                        command.Parameters.AddWithValue("@lead_time", postItem.lead_time);
                        command.Parameters.AddWithValue("@img_url", postItem.img_url);
                        command.Parameters.AddWithValue("@tax_id", postItem.tax_id);



                        // Add OUTPUT parameter to capture the stored procedure message
                        var outputParam = new SqlParameter("@Message", SqlDbType.NVarChar, 1000);
                        outputParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputParam);

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Get the message from the output parameter
                        message = command.Parameters["@Message"].Value.ToString();
                    }
                }

                // Check the message returned by the stored procedure
                if (message.StartsWith("Item inserted successfully."))
                {
                    return Ok(new { ExecuteMessage = message });
                }
                else
                {
                    return BadRequest(message); // Return error message
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPut("edit/{item_id}")]
        public IActionResult Edit_Item(Guid item_id, [FromBody] EditItem editItem)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.ItemEdit", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", editItem.user_id);
                        command.Parameters.AddWithValue("@item_id", item_id);
                        command.Parameters.AddWithValue("@item_code", editItem.item_code);
                        command.Parameters.AddWithValue("@item_name", editItem.item_name);
                        command.Parameters.AddWithValue("@item_description", editItem.item_description);
                        command.Parameters.AddWithValue("@item_type_id", editItem.item_type_id);
                        command.Parameters.AddWithValue("@item_category_id", editItem.item_category_id);
                        command.Parameters.AddWithValue("@color_id", editItem.color_id);
                        command.Parameters.AddWithValue("@uom_id", editItem.uom_id);
                        command.Parameters.AddWithValue("@purchase_price", editItem.purchase_price);
                        command.Parameters.AddWithValue("@selling_price", editItem.selling_price);
                        command.Parameters.AddWithValue("@stock_level_min", editItem.stock_level_min);
                        command.Parameters.AddWithValue("@stock_level_reorder", editItem.stock_level_reorder);
                        command.Parameters.AddWithValue("@lead_time", editItem.lead_time);
                        command.Parameters.AddWithValue("@img_url", editItem.img_url);
                        command.Parameters.AddWithValue("@tax_id", editItem.tax_id);
                        command.Parameters.AddWithValue("@is_active", editItem.is_active);

                        // Execute the stored procedure
                        var successMessageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 500)
                        {
                            Direction = ParameterDirection.Output
                        };
                        var errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 500)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(successMessageParam);
                        command.Parameters.Add(errorMessageParam);

                        command.ExecuteNonQuery();

                        string successMessage = successMessageParam.Value?.ToString();
                        string errorMessage = errorMessageParam.Value?.ToString();

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            return StatusCode(400, errorMessage); // Return bad request with error message
                        }
                        else if (!string.IsNullOrEmpty(successMessage))
                        {
                            return Ok(new { ExecuteMessage = successMessage }); // Return success message
                        }
                        else
                        {
                            return StatusCode(500, "Error: No response from the database."); // No response from database
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}"); // Internal server error
            }
        }

        [HttpPut("delete/{item_id}")]
        public IActionResult DeleteItemCategory(Guid item_id, [FromBody] DeleteColorModel deleteColorModel)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.ItemDelete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", deleteColorModel.user_id);
                        command.Parameters.AddWithValue("@item_id", item_id);

                        // Define output parameters for messages
                        var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 500)
                        {
                            Direction = ParameterDirection.Output
                        };
                        var errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 500)
                        {
                            Direction = ParameterDirection.Output
                        };

                        command.Parameters.Add(messageParam);
                        command.Parameters.Add(errorMessageParam);

                        command.ExecuteNonQuery();

                        string message = messageParam.Value?.ToString();
                        string errorMessage = errorMessageParam.Value?.ToString();

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            return StatusCode(400, errorMessage); // Return bad request with error message
                        }
                        else if (!string.IsNullOrEmpty(message))
                        {
                            return Ok(new { ExecuteMessage = message }); // Return success message
                        }
                        else
                        {
                            return StatusCode(500, "Error: No response from the database."); // No response from database
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}"); // Internal server error
            }
        }
    }
}
