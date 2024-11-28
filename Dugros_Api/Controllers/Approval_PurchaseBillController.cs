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
    public class Approval_PurchaseBillController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public Approval_PurchaseBillController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class PurchaseBillSummaryGet
        {
            public Guid? purchase_bill_id { get; set; }
            public string? voucher_id { get; set; }
            public string? voucher_no { get; set; }
            public DateTime? transaction_date { get; set; }

            public string? warehouse_id { get; set; }
            public string? warehouse_name { get; set; }
            public string? narration { get; set; }
            public decimal? total_bill_amt { get; set; }

            public string ap_status_name { get; set; }

            public Guid? requested_by { get; set; }
            public string? requested_by_name { get; set; }



            // public int? approval_enable { get; set; }
            // public int? approve_btn { get; set; }
        }

        public class ApproverApprovePB
        {
            public Guid user_id { get; set; }
            //public Guid trn_id { get; set; }
            //public Guid approved_by { get; set; }
            public string approver_status { get; set; }
            public string approver_notes { get; set; }
        }

        public class ApprovingFlowPB
        {
            public Guid trn_id { get; set; }
            public Guid user_id { get; set; }
            public string user_name { get; set; }
            public string activity { get; set; }
            // public Guid status { get; set; }
            public string approval_status { get; set; }
            public string remarks { get; set; }
            public bool is_enable { get; set; }
        }


        [HttpGet]
        public IActionResult GetPurchaseBills(
         [FromQuery] Guid userId,
         [FromQuery] string? approvalStatusId,
         [FromQuery] string final_status,
         [FromQuery] string? warehouseId,
         [FromQuery] DateTime? fromDate,
         [FromQuery] DateTime? toDate)
        {
            try
            {
                List<PurchaseBillSummaryGet> purchaseIndents = new List<PurchaseBillSummaryGet>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PurchaseBill_Approval_Get", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        //command.Parameters.AddWithValue("@status", statusId.HasValue ? (object)statusId.Value : DBNull.Value);
                        command.Parameters.AddWithValue("@status_app_abbr", string.IsNullOrEmpty(approvalStatusId) ? (object)DBNull.Value : approvalStatusId);
                        command.Parameters.AddWithValue("@warehouse_id", string.IsNullOrEmpty(warehouseId) ? (object)DBNull.Value : warehouseId);
                        command.Parameters.AddWithValue("@final_appr_status", string.IsNullOrEmpty(final_status) ? (object)DBNull.Value : final_status);
                        // command.Parameters.AddWithValue("@warehouse_id", warehouseId.HasValue ? (object)warehouseId.Value : DBNull.Value);
                        //command.Parameters.AddWithValue("@department", departmentId.HasValue ? (object)departmentId.Value : DBNull.Value);
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
                                    PurchaseBillSummaryGet indent = new PurchaseBillSummaryGet
                                    {

                                        purchase_bill_id = reader["purchase_bill_id"] != DBNull.Value ? (Guid)reader["purchase_bill_id"] : Guid.Empty,
                                        voucher_id = reader["voucher_id"] != DBNull.Value ? reader["voucher_id"].ToString() : null,
                                        voucher_no = reader["voucher_no"] != DBNull.Value ? reader["voucher_no"].ToString() : null,
                                        transaction_date = reader["voucher_date"] != DBNull.Value ? (DateTime)reader["voucher_date"] : (DateTime?)null,

                                        warehouse_id = reader["warehouse_id"] != DBNull.Value ? reader["warehouse_id"].ToString() : null,
                                        warehouse_name = reader["warehouse_name"].ToString(),
                                        narration = reader["narration"] != DBNull.Value ? reader["narration"].ToString() : null,
                                        total_bill_amt = reader["total_bill_amt"] != DBNull.Value ? (decimal)reader["total_bill_amt"] : (decimal?)null,
                                        // status_id = reader["status_id"] != DBNull.Value ? (Guid)reader["status_id"] : Guid.Empty,
                                        requested_by = reader["requested_by"] != DBNull.Value ? (Guid)reader["requested_by"] : Guid.Empty,
                                        requested_by_name = reader["requested_by_name"].ToString(),
                                        //status_name = reader["status_name"].ToString(),
                                        // approval_status = reader["approval_status"] != DBNull.Value ? reader["approval_status"].ToString() : null,
                                        ap_status_name = reader["app_status_name"].ToString(),
                                        //approval_enable = Convert.ToInt32(reader["approval_enable"]),
                                        // approve_btn = Convert.ToInt32(reader["approve_button"]),
                                    };


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
                    return NotFound("No purchase orders found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }



        [HttpPost("ApproverApprovePB/{trn_id}")]
        public IActionResult Approver_ApprovePO(Guid trn_id, [FromBody] ApproverApprovePB approverApprove)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PuchaseBill_ApproverApprove", connection))
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


        [HttpGet("ApprovingFlowPB")]
        public IActionResult GetPurchaseApproverFlow(Guid id, Guid user_id)
        {
            try
            {
                List<ApprovingFlowPB> approverDetailsList = new List<ApprovingFlowPB>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PurchaseBill_ApprovingFlow", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@trn_id", id);
                        command.Parameters.AddWithValue("@user_id", user_id);

                        // Add output parameters
                        var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                        var errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

                        command.Parameters.Add(messageParam);
                        command.Parameters.Add(errorMessageParam);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ApprovingFlowPB detail = new ApprovingFlowPB
                                {
                                    trn_id = reader.GetGuid(reader.GetOrdinal("trn_id")),
                                    user_id = reader.GetGuid(reader.GetOrdinal("user_id")),
                                    user_name = reader.IsDBNull(reader.GetOrdinal("user_name")) ? null : reader.GetString(reader.GetOrdinal("user_name")),
                                    activity = reader.IsDBNull(reader.GetOrdinal("activity")) ? null : reader.GetString(reader.GetOrdinal("activity")),
                                    // status = reader.GetGuid(reader.GetOrdinal("status")),
                                    approval_status = reader.IsDBNull(reader.GetOrdinal("status")) ? null : reader.GetString(reader.GetOrdinal("status")),
                                    remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? null : reader.GetString(reader.GetOrdinal("remarks")),
                                    is_enable = reader.IsDBNull(reader.GetOrdinal("is_enable")) ? false : reader.GetBoolean(reader.GetOrdinal("is_enable"))
                                };

                                approverDetailsList.Add(detail);
                            }
                        }

                        // Check the output parameters
                        var message = messageParam.Value.ToString();
                        var errorMessage = errorMessageParam.Value.ToString();

                        if (!string.IsNullOrEmpty(errorMessage))
                        {
                            return StatusCode(500, $"Error: {errorMessage}");
                        }
                    }
                }

                return Ok(approverDetailsList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

    }
}
