
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using static Dugros_Api.Controllers.ColorController;
using Newtonsoft.Json.Linq;
using Dugros_Api.Controllers;
using static Dugros_Api.Controllers.Approval_PurchaseIndentController;
using static Dugros_Api.Controllers.PurchaseIndentController;
using static Dugros_Api.Controllers.PurchaseOrderController;

namespace Dugros_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseIndentController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PurchaseIndentController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class ApproveFlow
        {
            //public Guid trn_id { get; set; }
            //public Guid user_id { get; set; }
            public string user_name { get; set; }
            public string activity { get; set; }
            // public Guid status { get; set; }
            public string approval_status { get; set; }
            public string remarks { get; set; }
            //public bool is_enable { get; set; }
        }
        public class PurchaseIndentSummaryModel
        {
            public Guid? voucher_id { get; set; }
            public string track_no { get; set; }
            //public string? voucher_no { get; set; }
            public DateTime voucher_date { get; set; }
            public string warehouse_id { get; set; }
            public Guid department_id { get; set; }
            public string narration { get; set; }
            public decimal total_value { get; set; }
            public Guid status_id { get; set; }
            public Guid requested_by { get; set; }

            public List<fileUpload1> files1 { get; set; }
            //public Guid checked_by { get; set; }
            //public DateTime check_time { get; set; }
            //public Guid approved_by { get; set; }
            //public DateTime approve_time { get; set; }
            //public Guid spcl_approved_by { get; set; }
            //public DateTime spcl_approve_time { get; set; }
            //public Guid approval_status { get; set; }
            //public bool sync_tally_app { get; set; }
            //public DateTime sync_tally_app_time { get; set; }
            //public bool sync_app_tally { get; set; }
            //public DateTime sync_app_tally_time { get; set; }

        }

        public class fileUpload1
        {
            public string url_path { get; set; }
            public string filename { get; set; }
        }

        public class PurchaseIndentDetailModel
        {
            public string? item_id { get; set; }
            public string? item_desc { get; set; }
            public string? uom_id { get; set; }
            public decimal? indent_qty { get; set; }
            public decimal? estimated_rate { get; set; }
            public decimal? estimated_value { get; set; }
            public decimal avl_qty { get; set; }
            public decimal in_stock_qty { get; set; }
            public decimal pending_receipt { get; set; }
            public decimal total_qty { get; set; }
            public List<delivery_PI> delivery_schedule { get; set; }
        }

        public class PurchaseIndentInsertModel
        {
            public Guid UserId { get; set; }
            public PurchaseIndentSummaryModel Summary { get; set; }
            public PurchaseIndentDetailModel[] Details { get; set; }
        }
        public class delivery_PI
        {
            public string item_id { get; set; }
            public decimal qty { get; set; }
            public DateTime trn_dt { get; set; }
        }

        public class response_delivery_PI
        {
            public Guid trn_id { get; set; }
            public string item_id { get; set; }
            public string item_name { get; set; }
            public decimal qty { get; set; }
            public DateOnly trn_dt { get; set; }
        }

        public class PurchaseIndentResponse
        {
            //public int? is_creator { get; set; }
            public PurchaseIndentSummaryGet summary { get; set; }
            public List<PurchaseIndentDetailGet>? details { get; set; }
            public List<ApproveFlow> approving_flow { get; set; }
        }

        public class PurchaseIndentSummaryGet
        {


            public Guid? purchase_indent_id { get; set; }
            public string track_no { get; set; }
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
            public Guid? requested_by { get; set; }
            public string? requested_by_name { get; set; }
            //public Guid? checked_by { get; set; }
            //public DateTime? check_time { get; set; }
            //public string? checker_notes { get; set; }
            //public Guid? checker_status { get; set; }
            //public string checker_status_name { get; set; }
            //public Guid? approved_by { get; set; }
            //public DateTime? approve_time { get; set; }
            //public string? approver_notes { get; set; }
            //public Guid? approver_status { get; set; }
            //public string approver_status_name { get; set; }
            //public Guid? spcl_approved_by { get; set; }
            //public DateTime? spcl_approve_time { get; set; }
            //public string? spcl_approver_notes { get; set; }
            //public Guid? spcl_approver_status { get; set; }
            //public string spcl_status_name { get; set; }
            public string? approval_status { get; set; }
            public string? ap_status_name { get; set; }
            public bool? sync_tally_app { get; set; }
            public DateTime? sync_tally_app_time { get; set; }
            public bool? sync_app_tally { get; set; }
            public DateTime? sync_app_tally_time { get; set; }
            //public int? is_creator { get; set; }
            public int? edit_enable { get; set; }
            public int? delete_enable { get; set; }
            //public int? checker_enable { get; set; }
            //public int? approver_enable { get; set; }
            //public int? spcl_approver_enable { get; set; }

            // public List<PurchaseIndentDetailGet>? details { get; set; }
            public string last_approved_by { get; set; }
            public List<fileUpload1> files1 { get; set; }
        }

        public class PurchaseIndentDetailGet
        {
            public string? item_id { get; set; }
            public string? item_desc { get; set; }
            public string item_name { get; set; }
            public string? uom_id { get; set; }
            public string? uom_name { get; set; }
            public decimal? indent_qty { get; set; }
            public decimal? estimated_rate { get; set; }
            public decimal? estimated_value { get; set; }
            public decimal? avl_qty { get; set; }
            public decimal in_stock_qty { get; set; }
            public decimal pending_receipt { get; set; }
            public decimal total_qty { get; set; }
            public List<response_delivery_PI>? delivery_schedule { get; set; }
        }
        public class PurchaseIndentSummaryEnable
        {
            public Guid purchase_indent_id { get; set; }
            public Guid documentId { get; set; }
            public Guid approvalId { get; set; }
            public Guid creator_id { get; set; }
            public string creator_name { get; set; }
            public DateTime created_date { get; set; }
            public Guid checker { get; set; }
            public Guid approver { get; set; }
            public Guid specialApprover { get; set; }
            public string checkerName { get; set; }
            public Guid checkerStatus { get; set; }
            public string checkerNotes { get; set; }
            public DateTime? checkTime { get; set; }
            public string approverName { get; set; }
            public Guid approverStatus { get; set; }
            public string approverNotes { get; set; }
            public DateTime? approveTime { get; set; }
            public string specialApproverName { get; set; }
            public Guid spclApproverStatus { get; set; }
            public string spclApproverNotes { get; set; }
            public DateTime? spclApproveTime { get; set; }
            public int checkerEnable { get; set; }
            public int approverEnable { get; set; }
            public int spclApproverEnable { get; set; }
            public string checker_status_name { get; set; }
            public string approver_status_name { get; set; }
            public string spcl_approver_status_name { get; set; }
        }

        public class DeletePurchaseIndent
        {
            public Guid user_id { get; set; }
        }

        public class ApproverDetails
        {
            public Guid user_id { get; set; }
            public string user_name { get; set; }
            public string activity { get; set; }
            public Guid status { get; set; }
            public string approval_status { get; set; }
            public string remarks { get; set; }
        }
        public class ApprovingFlow
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
        public IActionResult GetPurchaseIndents(Guid userId,
            Guid? statusId = null,
            string? approvalStatusId = null,
            string? warehouseId = null,
            Guid? departmentId = null,
            DateTime? fromDate = null,
            DateTime? toDate = null)
        {
            try
            {
                List<PurchaseIndentResponse> purchaseIndentResponses = new List<PurchaseIndentResponse>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PurchaseIndent_Get", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add input parameters
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@status", statusId ?? (object)DBNull.Value);
                        //command.Parameters.AddWithValue("@status_app_abbr", approvalStatusId.HasValue ? approvalStatusId.ToString() : (object)DBNull.Value);
                        command.Parameters.AddWithValue("@status_app_abbr", approvalStatusId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@warehouse_id", warehouseId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@department", departmentId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@from_date", fromDate ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@to_date", toDate ?? (object)DBNull.Value);

                        // Add output parameter
                        var outputParam = new SqlParameter("@output_message", SqlDbType.NVarChar, -1);
                        outputParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputParam);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    // Construct PurchaseIndentSummaryGet object
                                    PurchaseIndentSummaryGet summary = new PurchaseIndentSummaryGet
                                    {
                                        // Map properties from reader to PurchaseIndentSummaryGet
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
                                        edit_enable = Convert.ToInt32(reader["edit_enable"]),
                                        delete_enable = Convert.ToInt32(reader["delete_enable"]),
                                        approval_status = reader["approval_status"] != DBNull.Value ? reader["approval_status"].ToString() : null,
                                        ap_status_name = reader["app_status_name"] != DBNull.Value ? reader["app_status_name"].ToString() : null,
                                        track_no = reader["track_no"] != DBNull.Value ? reader["track_no"].ToString() : null,
                                        last_approved_by = reader["last_approved_by"].ToString()
                                    };

                                    // Retrieve delivery details from JSON
                                    //string delivery = reader["delivery_schedule"] != DBNull.Value ? reader["delivery_schedule"].ToString() : null;
                                    //JArray deliveryArray = JArray.Parse(delivery);
                                    //List<response_delivery_PI> det = new List<response_delivery_PI>();

                                    //if (deliveryArray != null)
                                    //{
                                    //    foreach (var item in deliveryArray)
                                    //    {
                                    //        response_delivery_PI detail = item.ToObject<response_delivery_PI>();
                                    //        det.Add(detail);
                                    //    }
                                    //}

                                    // Retrieve details from JSON
                                    string detailsJson = reader["details_json"] != DBNull.Value ? reader["details_json"].ToString() : null;
                                    JArray detailsArray = JArray.Parse(detailsJson);
                                    List<PurchaseIndentDetailGet> details = new List<PurchaseIndentDetailGet>();

                                    if (detailsArray != null)
                                    {
                                        foreach (var item in detailsArray)
                                        {
                                            PurchaseIndentDetailGet detail = item.ToObject<PurchaseIndentDetailGet>();
                                            details.Add(detail);
                                        }
                                    }

                                    PurchaseIndentResponse purchaseIndentResponse = new PurchaseIndentResponse
                                    {
                                        summary = summary,
                                        details = details,
                                    };

                                    purchaseIndentResponses.Add(purchaseIndentResponse);
                                }
                            }
                        }

                        // Retrieve output message
                        string outputMessage = outputParam.Value.ToString();

                        // Check output message and return appropriate response
                        if (outputMessage == "Fetched records successfully.")
                        {
                            if (purchaseIndentResponses.Any())
                            {
                                return Ok(purchaseIndentResponses);
                            }
                            else
                            {
                                return NotFound("No purchase indents found.");
                            }
                        }
                        else
                        {
                            return BadRequest(new { outputMessage });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message + " " + ex.StackTrace}");
            }
        }

        [HttpGet("Approval_Status")]
        public IActionResult GetPurchaseIndentsApproval(Guid userId,
 Guid? statusId = null,
 string? warehouseId = null,
 Guid? departmentId = null,
 DateTime? fromDate = null,
 DateTime? toDate = null)
        {
            try
            {
                List<PurchaseIndentResponse> purchaseIndentResponses = new List<PurchaseIndentResponse>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PurchaseIndent_Get_2024_04_24", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@status_id", statusId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@warehouse_id", warehouseId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@department_id", departmentId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@from_date", fromDate ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@to_date", toDate ?? (object)DBNull.Value);

                        var outputParam = new SqlParameter("@output_message", SqlDbType.NVarChar, -1);
                        outputParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputParam);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    PurchaseIndentSummaryGet summary = new PurchaseIndentSummaryGet
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
                                        //status_id = reader["status_id"] != DBNull.Value ? (Guid)reader["status_id"] : Guid.Empty,
                                        requested_by = reader["requested_by"] != DBNull.Value ? (Guid)reader["requested_by"] : Guid.Empty,
                                        requested_by_name = reader["requested_by_name"].ToString(),
                                        status_name = reader["status_name"].ToString(),
                                        //approval_status = reader["approval_status"] != DBNull.Value ? (Guid)reader["approval_status"] : Guid.Empty,

                                    };


                                    // Retrieve details from JSON
                                    string detailsJson = reader["details_json"] != DBNull.Value ? reader["details_json"].ToString() : null;
                                    JArray detailsArray = JArray.Parse(detailsJson);
                                    List<PurchaseIndentDetailGet> details = new List<PurchaseIndentDetailGet>();

                                    if (detailsArray != null)
                                    {
                                        foreach (var item in detailsArray)
                                        {
                                            PurchaseIndentDetailGet detail = item.ToObject<PurchaseIndentDetailGet>();
                                            details.Add(detail);
                                        }
                                    }

                                    PurchaseIndentResponse purchaseIndentResponse = new PurchaseIndentResponse
                                    {
                                        summary = summary,
                                        details = details
                                    };

                                    purchaseIndentResponses.Add(purchaseIndentResponse);
                                }
                            }
                        }

                        string outputMessage = outputParam.Value.ToString();
                        if (outputMessage != "Fetched records successfully.")
                        {
                            return BadRequest(new { outputMessage });
                        }
                    }
                }

                if (purchaseIndentResponses.Any())
                {
                    return Ok(purchaseIndentResponses);
                }
                else
                {
                    return NotFound("No purchase indents found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message + " " + ex.StackTrace}");
            }
        }

        [HttpGet("id")]
        public IActionResult GetPurchaseIndentsById(Guid userId,
Guid purchase_indent_id)
        {
            try
            {
                List<PurchaseIndentResponse> purchaseIndentResponses = new List<PurchaseIndentResponse>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PurchaseIndent_GetById", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@purchase_indent_id", purchase_indent_id);


                        var outputParam = new SqlParameter("@output_message", SqlDbType.NVarChar, -1);
                        outputParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputParam);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    PurchaseIndentSummaryGet summary = new PurchaseIndentSummaryGet
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
                                        track_no = reader["track_no"] != DBNull.Value ? reader["track_no"].ToString() : null,
                                        total_value = reader["total_value"] != DBNull.Value ? (decimal)reader["total_value"] : (decimal?)null,
                                        status_id = reader["status_id"] != DBNull.Value ? (Guid)reader["status_id"] : Guid.Empty,
                                        requested_by = reader["requested_by"] != DBNull.Value ? (Guid)reader["requested_by"] : Guid.Empty,
                                        requested_by_name = reader["requested_by_name"].ToString(),
                                        status_name = reader["status_name"].ToString(),
                                        files1 = reader["file_details"] != DBNull.Value ? JsonConvert.DeserializeObject<List<fileUpload1>>(reader["file_details"].ToString()) : new List<fileUpload1>()
                                    };


                                    //                             // Retrieve delivery details from JSON
                                    //string delivery = reader["delivery_schedule"] != DBNull.Value ? reader["delivery_schedule"].ToString() : null;
                                    //JArray deliveryArray = JArray.Parse(delivery);
                                    //List<response_delivery_PI> det = new List<response_delivery_PI>();

                                    //if (deliveryArray != null)
                                    //{
                                    //    foreach (var item in deliveryArray)
                                    //    {
                                    //        response_delivery_PI detail = item.ToObject<response_delivery_PI>();
                                    //        det.Add(detail);
                                    //    }
                                    //}
                                    // Retrieve details from JSON
                                    string detailsJson = reader["details_json"] != DBNull.Value ? reader["details_json"].ToString() : null;
                                    JArray detailsArray = JArray.Parse(detailsJson);
                                    List<PurchaseIndentDetailGet> details = new List<PurchaseIndentDetailGet>();

                                    if (detailsArray != null)
                                    {
                                        foreach (var item in detailsArray)
                                        {
                                            PurchaseIndentDetailGet detail = item.ToObject<PurchaseIndentDetailGet>();
                                            details.Add(detail);
                                        }
                                    }

                                    string statusJson = reader["approving_flow"] != DBNull.Value ? reader["approving_flow"].ToString() : null;
                                    JArray statusArray = JArray.Parse(statusJson);
                                    List<ApproveFlow> status = new List<ApproveFlow>();

                                    if (statusArray != null)
                                    {
                                        foreach (var item in statusArray)
                                        {
                                            ApproveFlow detail = item.ToObject<ApproveFlow>();
                                            status.Add(detail);
                                        }
                                    }

                                    PurchaseIndentResponse purchaseIndentResponse = new PurchaseIndentResponse
                                    {
                                        summary = summary,
                                        details = details,
                                        approving_flow = status
                                    };

                                    purchaseIndentResponses.Add(purchaseIndentResponse);
                                }
                            }
                        }

                        string outputMessage = outputParam.Value.ToString();
                        if (outputMessage != "Fetched records successfully.")
                        {
                            return BadRequest(new { outputMessage });
                        }
                    }
                }

                if (purchaseIndentResponses.Any())
                {
                    return Ok(purchaseIndentResponses);
                }
                else
                {
                    return NotFound("No purchase indents found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message + " " + ex.StackTrace}");
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertPurchaseIndent([FromBody] PurchaseIndentInsertModel model)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                string storedProcedureName = "PurchaseIndent_Insert";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Convert Summary and Details to JSON
                        string summaryJson = Newtonsoft.Json.JsonConvert.SerializeObject(model.Summary);
                        string detailsJson = Newtonsoft.Json.JsonConvert.SerializeObject(model.Details);
                        // Add parameters
                        command.Parameters.AddWithValue("@user_id", model.UserId);
                        command.Parameters.AddWithValue("@summaryJson", summaryJson);
                        command.Parameters.AddWithValue("@detailsJson", detailsJson);
                        command.Parameters.Add("@executeMessage", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@voucherMessage", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;
                        command.Parameters.Add("@approvalMessage", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        string executeMessage = command.Parameters["@executeMessage"].Value.ToString();
                        string voucherMessage = command.Parameters["@voucherMessage"].Value.ToString();
                        string approvalMessage = command.Parameters["@approvalMessage"].Value.ToString();
                        return Ok(new { executeMessage = executeMessage,voucherMessage=voucherMessage,approvalMessage=approvalMessage });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPut("update/{purchase_indent_id}")]
        public async Task<IActionResult> UpdatePurchaseIndent(Guid purchase_indent_id, [FromBody] PurchaseIndentInsertModel model)
        {
            try
            {
                string connectionString = _configuration.GetConnectionString("DefaultConnection");
                string storedProcedureName = "PurchaseIndent_Update";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(storedProcedureName, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Convert Summary and Details to JSON
                        string summaryJson = JsonConvert.SerializeObject(model.Summary);
                        string detailsJson = JsonConvert.SerializeObject(model.Details);

                        // Add parameters
                        command.Parameters.AddWithValue("@user_id", model.UserId);
                        command.Parameters.AddWithValue("@purchase_indent_id", purchase_indent_id);
                        command.Parameters.AddWithValue("@summaryJson", summaryJson);
                        command.Parameters.AddWithValue("@detailsJson", detailsJson);
                        command.Parameters.Add("@executeMessage", SqlDbType.NVarChar, -1).Direction = ParameterDirection.Output;

                        await connection.OpenAsync();
                        await command.ExecuteNonQueryAsync();

                        string executeMessage = command.Parameters["@executeMessage"].Value.ToString();
                        return Ok(new { Message = executeMessage });
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpPut("delete/{purchase_indent_id}")]
        public IActionResult Delete_PurchaseIndent(Guid purchase_indent_id, [FromBody] DeletePurchaseIndent deletePurchaseIndent)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PurchaseIndent_Delete", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", deletePurchaseIndent.user_id);
                        command.Parameters.AddWithValue("@purchase_indent_id", purchase_indent_id);

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

        [HttpGet("ApproverEnable/{id}")]
        public IActionResult GetPurchaseApprovalEnable(Guid id)
        {
            try
            {
                List<PurchaseIndentSummaryEnable> purchaseIndentSummaryEnableList = new List<PurchaseIndentSummaryEnable>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PurchaseIndent_Approver_Enable", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@trn_id", id);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                PurchaseIndentSummaryEnable summary = new PurchaseIndentSummaryEnable
                                {
                                    purchase_indent_id = reader.IsDBNull(reader.GetOrdinal("purchase_indent_id")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("purchase_indent_id")),
                                    documentId = reader.IsDBNull(reader.GetOrdinal("document_id")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("document_id")),
                                    approvalId = reader.IsDBNull(reader.GetOrdinal("approval_id")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("approval_id")),
                                    checker = reader.IsDBNull(reader.GetOrdinal("checker")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("checker")),
                                    approver = reader.IsDBNull(reader.GetOrdinal("approver")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("approver")),
                                    specialApprover = reader.IsDBNull(reader.GetOrdinal("special_approver")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("special_approver")),
                                    checkerName = reader.IsDBNull(reader.GetOrdinal("checker_name")) ? null : reader.GetString(reader.GetOrdinal("checker_name")),
                                    checkerStatus = reader.IsDBNull(reader.GetOrdinal("checker_status")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("checker_status")),
                                    checkerNotes = reader.IsDBNull(reader.GetOrdinal("checker_notes")) ? null : reader.GetString(reader.GetOrdinal("checker_notes")),
                                    checkTime = reader.IsDBNull(reader.GetOrdinal("check_time")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("check_time")),
                                    approverName = reader.IsDBNull(reader.GetOrdinal("approver_name")) ? null : reader.GetString(reader.GetOrdinal("approver_name")),
                                    approverStatus = reader.IsDBNull(reader.GetOrdinal("approver_status")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("approver_status")),
                                    approverNotes = reader.IsDBNull(reader.GetOrdinal("approver_notes")) ? null : reader.GetString(reader.GetOrdinal("approver_notes")),
                                    approveTime = reader.IsDBNull(reader.GetOrdinal("approve_time")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("approve_time")),
                                    specialApproverName = reader.IsDBNull(reader.GetOrdinal("spcl_approver_name")) ? null : reader.GetString(reader.GetOrdinal("spcl_approver_name")),
                                    spclApproverStatus = reader.IsDBNull(reader.GetOrdinal("spcl_approver_status")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("spcl_approver_status")),
                                    spclApproverNotes = reader.IsDBNull(reader.GetOrdinal("spcl_approver_notes")) ? null : reader.GetString(reader.GetOrdinal("spcl_approver_notes")),
                                    spclApproveTime = reader.IsDBNull(reader.GetOrdinal("spcl_approve_time")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("spcl_approve_time")),
                                    checkerEnable = reader.GetInt32(reader.GetOrdinal("checker_enable")),
                                    approverEnable = reader.GetInt32(reader.GetOrdinal("approver_enable")),
                                    spclApproverEnable = reader.GetInt32(reader.GetOrdinal("spcl_approver_enable")),
                                    creator_id = reader.IsDBNull(reader.GetOrdinal("creator_id")) ? Guid.Empty : reader.GetGuid(reader.GetOrdinal("creator_id")),
                                    created_date = (DateTime)(reader.IsDBNull(reader.GetOrdinal("created_date")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("created_date"))),
                                    creator_name = reader.IsDBNull(reader.GetOrdinal("creator_name")) ? null : reader.GetString(reader.GetOrdinal("creator_name")),
                                    checker_status_name = reader["checker_status_name"].ToString(),
                                    approver_status_name = reader["approver_status_name"].ToString(),
                                    spcl_approver_status_name = reader["spcl_approver_status_name"].ToString(),
                                };

                                purchaseIndentSummaryEnableList.Add(summary);
                            }
                        }
                    }
                }

                return Ok(purchaseIndentSummaryEnableList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("ApproverDetails/{id}")]
        public IActionResult GetPurchaseApproverDetails(Guid id)
        {
            try
            {
                List<ApproverDetails> approverDetailsList = new List<ApproverDetails>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PurchaseIndent_ApproverDetails", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@trn_id", id);

                        // Add output parameters
                        var messageParam = new SqlParameter("@Message", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                        var errorMessageParam = new SqlParameter("@ErrorMessage", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };

                        command.Parameters.Add(messageParam);
                        command.Parameters.Add(errorMessageParam);

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ApproverDetails detail = new ApproverDetails
                                {
                                    user_id = reader.GetGuid(reader.GetOrdinal("user_id")),
                                    user_name = reader.IsDBNull(reader.GetOrdinal("user_name")) ? null : reader.GetString(reader.GetOrdinal("user_name")),
                                    activity = reader.IsDBNull(reader.GetOrdinal("activity")) ? null : reader.GetString(reader.GetOrdinal("activity")),
                                    status = reader.GetGuid(reader.GetOrdinal("status")),
                                    approval_status = reader.IsDBNull(reader.GetOrdinal("approval_status")) ? null : reader.GetString(reader.GetOrdinal("approval_status")),
                                    remarks = reader.IsDBNull(reader.GetOrdinal("remarks")) ? null : reader.GetString(reader.GetOrdinal("remarks"))
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


        [HttpGet("ApprovingFlow")]
        public IActionResult GetPurchaseApproverFlow(Guid id, Guid user_id)
        {
            try
            {
                List<ApprovingFlow> approverDetailsList = new List<ApprovingFlow>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.PurchaseIndent_ApprovingFlow", connection))
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
                                ApprovingFlow detail = new ApprovingFlow
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
