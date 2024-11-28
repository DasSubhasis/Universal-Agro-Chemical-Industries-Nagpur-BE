using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using static Dugros_Api.Controllers.PurchaseIndentController;

namespace Dugros_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class Approval_PurchaseIndentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public Approval_PurchaseIndentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class PurchaseIndentSummaryGet
        {
            public Guid? purchase_indent_id { get; set; }
            public Guid? voucher_id { get; set; }
            public string? voucher_no { get; set; }
            public DateTime? voucher_date { get; set; }
            public Guid? department_id { get; set; }
            public string? dept_name { get; set; }
            public string? warehouse_id { get; set; }
            public string? warehouse_name { get; set; }
            public string? narration { get; set; }
            public decimal? total_value { get; set; }
            public Guid? status_id { get; set; }
            public string? status_name { get; set; }
            public string? approval_status { get; set; }
            public string ap_status_name { get; set; }
            public bool? sync_tally_app { get; set; }
            public DateTime? sync_tally_app_time { get; set; }
            public bool? sync_app_tally { get; set; }
            public DateTime? sync_app_tally_time { get; set; }

            public Guid? requested_by { get; set; }
            public string? requested_by_name { get; set; }
            
            
           
            public int? approval_enable { get; set; }
            // public int? approve_btn { get; set; }

            public List<PurchaseIndentDetailGet>? details { get; set; }
        }

        public class PurchaseIndentDetailGet
        {
            public string? item_id { get; set; }
            public string item_name { get; set; }
            public string? uom_id { get; set; }
            public string? uom_name { get; set; }
            public decimal? indent_qty { get; set; }
            public decimal? estimated_rate { get; set; }
            public decimal? estimated_value { get; set; }
            public decimal? avl_qty { get; set; }
        }
        public class CheckerApprove
        {
            public Guid user_id { get; set; }
            public Guid trn_id { get; set; }
            public Guid checked_by { get; set; }
            public Guid checker_status { get; set; }
            public string checker_notes { get; set; }
        }
        public class ApproverApprove
        {
            public Guid user_id { get; set; }
            //public Guid trn_id { get; set; }
            //public Guid approved_by { get; set; }
            public string approver_status { get; set; }
            public string approver_notes { get; set; }
        }
        public class SpclApproverApprove
        {
            public Guid user_id { get; set; }
            //public Guid trn_id { get; set; }
            //public Guid spcl_approved_by { get; set; }
            public Guid spcl_approver_status { get; set; }
            public string spcl_approver_notes { get; set; }
        }
        [HttpGet]
        public IActionResult GetPurchaseIndents(
           [FromQuery] Guid userId,
           [FromQuery] Guid? statusId,
           [FromQuery] string? approvalStatusId,
           [FromQuery] string? warehouseId,
           [FromQuery] string final_status,
           [FromQuery] Guid? departmentId,
           [FromQuery] DateTime? fromDate,
           [FromQuery] DateTime? toDate)
           
        {
            try
            {
                List<PurchaseIndentSummaryGet> purchaseIndents = new List<PurchaseIndentSummaryGet>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PurchaseIndent_Approval_Get", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@status", statusId.HasValue ? (object)statusId.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@status_app_abbr", string.IsNullOrEmpty(approvalStatusId) ? (object)DBNull.Value : approvalStatusId);
                        command.Parameters.AddWithValue("@warehouse_id", string.IsNullOrEmpty(warehouseId) ? (object)DBNull.Value : warehouseId);
                        command.Parameters.AddWithValue("@final_appr_status", string.IsNullOrEmpty(final_status) ? (object)DBNull.Value : final_status);
                        // command.Parameters.AddWithValue("@warehouse_id", warehouseId.HasValue ? (object)warehouseId.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@department", departmentId.HasValue ? (object)departmentId.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@from_date", fromDate.HasValue ? (object)fromDate.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@to_date", toDate.HasValue ? (object)toDate.Value : DBNull.Value);

                        // Add the @output_message parameter
                        var outputParam = new SqlParameter("@output_message", SqlDbType.NVarChar, 1000);
                        outputParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputParam);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    // Map the data from SqlDataReader to PurchaseIndentSummaryGet object
                                    PurchaseIndentSummaryGet indent = new PurchaseIndentSummaryGet
                                    {
                                        
                                        purchase_indent_id = reader["purchase_indent_id"] != DBNull.Value ? (Guid)reader["purchase_indent_id"] : Guid.Empty,
                                        voucher_id = reader["voucher_id"] != DBNull.Value ? (Guid)reader["voucher_id"] : Guid.Empty,
                                        voucher_no = reader["voucher_no"] != DBNull.Value ? reader["voucher_no"].ToString() : null,
                                        voucher_date = reader["voucher_date"] != DBNull.Value ? (DateTime)reader["voucher_date"] : (DateTime?)null,
                                        department_id = reader["department_id"] != DBNull.Value ? (Guid)reader["department_id"] : Guid.Empty,
                                        dept_name = reader["dept_name"].ToString(),
                                        warehouse_id = reader["warehouse_id"] != DBNull.Value ? reader["warehouse_id"].ToString() : null,
                                        warehouse_name = reader["warehouse_name"].ToString(),
                                        narration = reader["narration"] != DBNull.Value ? reader["narration"].ToString() : null,
                                        total_value = reader["total_value"] != DBNull.Value ? (decimal)reader["total_value"] : (decimal?)null,
                                        status_id = reader["status_id"] != DBNull.Value ? (Guid)reader["status_id"] : Guid.Empty,
                                        requested_by = reader["requested_by"] != DBNull.Value ? (Guid)reader["requested_by"] : Guid.Empty,
                                        requested_by_name = reader["requested_by_name"].ToString(),
                                        status_name = reader["status_name"].ToString(),
                                        approval_status = reader["approval_status"] != DBNull.Value ? reader["approval_status"].ToString() : null,
                                        ap_status_name = reader["app_status_name"].ToString(),
                                        approval_enable = Convert.ToInt32(reader["approval_enable"]),
                                        // approve_btn = Convert.ToInt32(reader["approve_button"]),
                                    };

                                    // Add purchase indent details
                                    indent.details = new List<PurchaseIndentDetailGet>();

                                    // Retrieve JSON string of details and deserialize it
                                    string detailsJson = reader["details_json"].ToString();
                                    List<PurchaseIndentDetailGet> details = JsonConvert.DeserializeObject<List<PurchaseIndentDetailGet>>(detailsJson);
                                    indent.details.AddRange(details);

                                    purchaseIndents.Add(indent);
                                }
                            }
                        }

                        // Retrieve the value of the output parameter after executing the command
                        string outputMessage = command.Parameters["@output_message"].Value.ToString();
                        // You can use the outputMessage if needed
                    }
                }

                if (purchaseIndents.Any())
                {
                    return Ok(purchaseIndents);
                }
                else
                {
                    return NotFound("No purchase indents found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost("CheckerApprove/{trn_id}")]
        public IActionResult Checker_Approve(Guid trn_id, [FromBody] CheckerApprove checkerApprove)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PuchaseIndent_CheckerApprove", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", checkerApprove.user_id);
                        command.Parameters.AddWithValue("@trn_id", trn_id);
                        command.Parameters.AddWithValue("@checked_by", checkerApprove.checked_by);
                        command.Parameters.AddWithValue("@checker_status", checkerApprove.checker_status);
                        command.Parameters.AddWithValue("@checker_notes", checkerApprove.checker_notes);

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

        [HttpPost("ApproverApprove/{trn_id}")]
        public IActionResult Approver_Approve(Guid trn_id, [FromBody] ApproverApprove approverApprove)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PuchaseIndent_ApproverApprove", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", approverApprove.user_id);
                        command.Parameters.AddWithValue("@trn_id", trn_id);
                        //command.Parameters.AddWithValue("@approved_by", approverApprove.approved_by);
                        command.Parameters.AddWithValue("@approver_status", approverApprove.approver_status);
                        command.Parameters.AddWithValue("@approver_notes", approverApprove.approver_notes);

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

        [HttpPost("SpclApproverApprove/{trn_id}")]
        public IActionResult Spcl_Approver_Approve(Guid trn_id, [FromBody] SpclApproverApprove spclApprover)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PuchaseIndent_SpclApproverApprove", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", spclApprover.user_id);
                        command.Parameters.AddWithValue("@trn_id", trn_id);
                        //command.Parameters.AddWithValue("@spcl_approved_by", spclApprover.spcl_approved_by);
                        command.Parameters.AddWithValue("@spcl_approver_status", spclApprover.spcl_approver_status);
                        command.Parameters.AddWithValue("@spcl_approver_notes", spclApprover.spcl_approver_notes);

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
    }
}
