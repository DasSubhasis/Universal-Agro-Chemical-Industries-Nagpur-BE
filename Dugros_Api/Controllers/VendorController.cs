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
    public class VendorController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public VendorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class GetVendor
        {
            public string ledger_id { get; set; }
            public string ledger_name { get; set; }
            public string address { get; set; }
            public string state { get; set; }
            public string group_id { get; set; }
            public string group_name { get; set; }
            public string gst_no { get; set; }
        }
        public class GetAddress
        {
            public Guid id { get; set; }
            public string address { get; set; }
            public string gst { get; set; }
            public string pan { get; set; }
            public string state { get; set; }
            public string pin { get; set; }
        }
        public class VendorGroup
        {
            public string id { get; set; }
            public string group { get; set; }
        }
        [HttpGet("GetVendorGroup")]
        public IActionResult Get_Vendor_Group()
        {
            try
            {

                List<VendorGroup> itemCategories = new List<VendorGroup>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.VendorGroup_Get", connection))
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
                                    VendorGroup color = new VendorGroup
                                    {
                                        id = reader["ParentID"].ToString(),
                                        group = reader["Parent"].ToString()
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
                    return NotFound("No vendors found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpGet("GetVendor")]
        public IActionResult Get_Vendor(Guid userId,string? vendor_group)
        {
            try
            {

                List<GetVendor> itemCategories = new List<GetVendor>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.VendorGet", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@group_id", vendor_group);

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
                                    GetVendor color = new GetVendor
                                    {
                                        ledger_id = reader["Ledger_ID"].ToString(),
                                        ledger_name = reader["Name"].ToString(),
                                        address = reader["Address"].ToString(),
                                        state = reader["state"].ToString(),
                                        group_id = reader["ParentID"].ToString(),
                                        group_name = reader["Parent"].ToString(),
                                        gst_no = reader["gst_no"].ToString()
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
                    return NotFound("No vendors found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("BillingAddress")]
        public IActionResult GetBilling(Guid user_id)
        {
            try
            {
                List<GetAddress> itemCategories = new List<GetAddress>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.BillingAddress_Get", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", user_id);

                        // Add OUTPUT parameter to capture the executeMessage from the stored procedure
                        var executeMessageParam = new SqlParameter("@executeMessage", SqlDbType.NVarChar, 1000);
                        executeMessageParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(executeMessageParam);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    GetAddress color = new GetAddress
                                    {       
                                        id = (Guid)reader["id"],
                                        address = reader["bill_address"].ToString(),
                                        gst = reader["gst_no"].ToString(),
                                        pan = reader["pan_no"].ToString(),
                                        state = reader["state_name"].ToString(),
                                        pin = reader["bill_pin"].ToString()
                                    };

                                    itemCategories.Add(color);
                                }
                            }
                        }

                        // Capture the executeMessage value (but do not use it)
                        string executeMessage = executeMessageParam.Value.ToString();
                    }
                }

                if (itemCategories.Any())
                {
                    return Ok(itemCategories);
                }
                else
                {
                    return NotFound("No vendors found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }


        [HttpGet("ShippingAddress")]
        public IActionResult GetShipping(string warehouse_id)
        {
            try
            {

                List<GetAddress> itemCategories = new List<GetAddress>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.sp_shipping_addressbyvendor", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@warehouse_id", warehouse_id);

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
                                    GetAddress color = new GetAddress
                                    {
                                        address = reader["address"].ToString(),
                                        gst = reader["gst"].ToString(),
                                        pan = reader["pan"].ToString(),
                                        state = reader["state"].ToString(),
                                        pin = reader["pin"].ToString()
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
                    return NotFound("No vendors found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }
    }
}
