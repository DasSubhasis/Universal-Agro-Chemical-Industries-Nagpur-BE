using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;
using static Dugros_Api.Controllers.ColorController;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;


namespace Dugros_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PurchaseReportController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public PurchaseReportController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class PurchaseOrderSummary
        {
            public DateTime T_Date { get; set; }
            public string Doc_No { get; set; }
            public string Supplier_Name { get; set; }
            public string Bill_To { get; set; }
            public string Shipper_To { get; set; }
            public DateTime? Delv_Date { get; set; }
            public decimal Amount { get; set; }
            public string Status_Name { get; set; }
            public Guid Status_Id { get; set; }
            public Guid Purchase_Order_trn_Id { get; set; }
            public Guid Voucher_Id { get; set; }
            public string Vendor_Id { get; set; }
            public string Shipper_Name { get; set; }
            public DateTime? Po_Due_Date { get; set; }
            public string Warehouse_Id { get; set; }
            public string Last_Approved_By { get; set; }
            public string Cheque_Issued { get; set; }
           
        }

        public class PurchaseIndentDetailsDto
        {
            public string PI_Number { get; set; }
            public DateTime? PI_Date { get; set; }

            public string PI_Owner { get; set; }
            public string Location_WHS { get; set; }
            public DateTime? EXP_Rcpt_Date { get; set; }
            public string Item_name { get; set; }
            public string ItemID { get; set; }
            public decimal Item_PI_Qty { get; set; }
            public decimal PO_QTY { get; set; }
            public decimal Balance_QTY_OF_PI { get; set; }
            public Guid Status_Id { get; set; }
            public string Status_Name { get; set; }
            public Guid Purchase_Indent_Id { get; set; }
            public string Track_No { get; set; }
            //public Guid Item_Id { get; set; }
            public Guid Voucher_Id { get; set; }
            public Guid Department_Id { get; set; }
            public string Dept_Name { get; set; }
            //public string Narration { get; set; }
            //public decimal Total_Value { get; set; }
            //public string Approval_Status { get; set; }
            public string App_Status_Name { get; set; }
            public Guid Requested_By { get; set; }
            public string Last_Approved_By { get; set; }
           
        }

        public class PurchaseOrderDetails
        {
            public string PONumber { get; set; }
            public DateTime? PODate { get; set; }
            public string POOwner { get; set; }
            public string SupplierName { get; set; }
            public string SupplierID { get; set; }
            public string LocationWHS { get; set; }
            public string ItemName { get; set; }
            public string ItemID { get; set; }
            public decimal ItemPOQty { get; set; }
            public decimal RecptQty { get; set; }
            public decimal Balance { get; set; }
            public Guid? StatusId { get; set; }
            public string StatusName { get; set; }
            public DateTime? EXPRecptDate { get; set; }
            public string App_Status_Name { get; set; }
            public Guid? PurchaseOrderTrnId { get; set; }
            //public string Narration { get; set; }
        }

        public class ApprovalDuePIRegister
        {
            public string PI_Number { get; set; }
            public DateTime? PI_Date { get; set; }
            public string PI_Owner { get; set; }
            public string Location_WHS { get; set; }
            public string Item_name { get; set; }
            public string ItemID { get; set; }
            public decimal Item_PI_Qty { get; set; }
            public decimal PO_QTY { get; set; }
            public decimal Balance_QTY_OF_PI { get; set; }
            public Guid Status_Id { get; set; }
            public string Status_Name { get; set; }
            public DateTime? EXP_Rcpt_Date { get; set; }
            public Guid Purchase_Indent_Id { get; set; }
           
            public Guid Voucher_Id { get; set; }
            public Guid Department_Id { get; set; }
            public string Dept_Name { get; set; }
            public string App_Status_Name { get; set; }
            public Guid Requested_By { get; set; }
            public string Last_Approved_By { get; set; }
           
        }

        public class PDCDetails
        {
            public string PONumber { get; set; }
            public DateTime? PODate { get; set; }
            public string POOwner { get; set; }
            public string SupplierName { get; set; }
            public string SupplierID { get; set; }
            public string LocationWHS { get; set; }
            public string ApprovalStatusAbbr { get; set; }
            public string ItemName { get; set; }
            public string ItemID { get; set; }
            public decimal POAmt { get; set; }
            public decimal PDCAmount { get; set; }
            public decimal Balance { get; set; }
            public string Status { get; set; }
            public Guid? StatusId { get; set; }
            public Guid? PurchaseOrderTrnId { get; set; }
        }



        [HttpGet("GetPurchaseOrderSummary")]
        public IActionResult GetPurchaseOrderSummary([FromQuery] DateTime? fromDate,[FromQuery] DateTime? toDate, [FromQuery] Guid? statusId,[FromQuery] string? supplierName, [FromQuery] Guid? userId)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.sp_GetPurchaseOrderSummary", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add input parameters
                        command.Parameters.AddWithValue("@from_date", (object)fromDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@to_date", (object)toDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@status_id", (object)statusId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@user_id", (object)userId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@Supplier_Name", (object)supplierName ?? DBNull.Value);

                        // Add output parameter
                        var outputMessageParam = new SqlParameter("@output_message", SqlDbType.NVarChar, -1)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputMessageParam);

                        // Execute and fetch data
                        using (var reader = command.ExecuteReader())
                        {
                            var purchaseOrderSummaries = new List<PurchaseOrderSummary>();

                            while (reader.Read())
                            {
                                purchaseOrderSummaries.Add(new PurchaseOrderSummary
                                {
                                    T_Date = reader["T_Date"] != DBNull.Value ? (DateTime)reader["T_Date"] : DateTime.MinValue,
                                    Doc_No = reader["Doc_No"]?.ToString() ?? string.Empty,
                                    Supplier_Name = reader["Supplier_Name"]?.ToString() ?? string.Empty,
                                    Bill_To = reader["Bill_To"]?.ToString() ?? string.Empty,
                                    Shipper_To = reader["Shipper_To"]?.ToString() ?? string.Empty,
                                    Delv_Date = reader["Delv_Date"] != DBNull.Value ? (DateTime?)reader["Delv_Date"] : null,
                                    Amount = reader["Amount"] != DBNull.Value ? (decimal)reader["Amount"] : 0m,
                                    Status_Name = reader["Status_Name"]?.ToString() ?? string.Empty,
                                    Status_Id = reader["Status_Id"] != DBNull.Value && Guid.TryParse(reader["Status_Id"].ToString(), out Guid statusID) ? statusID : Guid.Empty,
                                    Purchase_Order_trn_Id = reader["purchase_Order_trn_id"] != DBNull.Value && Guid.TryParse(reader["purchase_Order_trn_id"].ToString(), out Guid poTrnId) ? poTrnId : Guid.Empty,
                                    Voucher_Id = reader["voucher_id"] != DBNull.Value && Guid.TryParse(reader["voucher_id"].ToString(), out Guid voucherId) ? voucherId : Guid.Empty,
                                    Vendor_Id = reader["vendor_id"]?.ToString(),
                                    Shipper_Name = reader["shipper_name"]?.ToString() ?? string.Empty,
                                    Po_Due_Date = reader["po_due_date"] != DBNull.Value ? (DateTime?)reader["po_due_date"] : null,
                                    Warehouse_Id = reader["warehouse_id"]?.ToString(),
                                    Last_Approved_By = reader["last_approved_by"]?.ToString() ?? string.Empty,
                                    //Cheque_Issued = reader["cheque_issued"] != DBNull.Value ? Convert.ToInt32(reader["cheque_issued"]) : 0,
                                    Cheque_Issued = reader["cheque_issued"].ToString()
                                  
                                   
                                });
                            }

                            // Check output message
                            //string outputMessage = outputMessageParam.Value?.ToString();
                            //if (string.IsNullOrEmpty(outputMessage) || outputMessage != "Records fetched successfully")
                            //{
                            //    return NotFound(new { Message = outputMessage ?? "No records found." });
                            //}

                            string outputMessage = outputMessageParam.Value?.ToString();

                            if (!string.IsNullOrEmpty(outputMessage) && outputMessage != "Data retrieved successfully.")
                            {
                                return NotFound(new { Message = outputMessage }); // Return error if no records found
                            }

                            return Ok(purchaseOrderSummaries);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet("GetPurchaseIndentDetails")]
        public IActionResult GetPurchaseIndentDetails([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] Guid? statusId, [FromQuery] Guid? userId)
        {
            try
            {
                // Connection string from configuration
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.sp_Get_PI_Register_Details", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add input parameters for the stored procedure
                        command.Parameters.AddWithValue("@from_date", (object)fromDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@to_date", (object)toDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@user_id", (object)userId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@status_id", (object)statusId ?? DBNull.Value);

                        // Add output parameter for output_message
                        var outputMessageParam = new SqlParameter("@output_message", SqlDbType.NVarChar, -1)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputMessageParam);

                        // Execute the stored procedure and retrieve data
                        using (var reader = command.ExecuteReader())
                        {
                            // Prepare a list to store the purchase indent details data
                            var purchaseIndentDetailsList = new List<PurchaseIndentDetailsDto>();

                            while (reader.Read())
                            {
                                // Map data from the reader into the DTO
                                purchaseIndentDetailsList.Add(new PurchaseIndentDetailsDto
                                {
                                    PI_Number = reader["PI_Number"]?.ToString(),
                                    PI_Date = reader["PI_Date"] != DBNull.Value ? (DateTime)reader["PI_Date"] : (DateTime?)null,
                                    PI_Owner = reader["PI_Owner"]?.ToString(),
                                    Location_WHS = reader["Location_WHS"]?.ToString(),
                                    EXP_Rcpt_Date = reader["EXP_Rcpt_Date"] != DBNull.Value ? (DateTime?)reader["EXP_Rcpt_Date"] : null,
                                    Item_name = reader["Item_name"]?.ToString(),
                                    ItemID = reader["Item_ID"]?.ToString(),
                                    Item_PI_Qty = reader["Item_PI_Qty"] != DBNull.Value ? (decimal)reader["Item_PI_Qty"] : 0,
                                    PO_QTY = reader["PO_QTY"] != DBNull.Value ? (decimal)reader["PO_QTY"] : 0,
                                    Balance_QTY_OF_PI = reader["Balance_QTY_OF_PI"] != DBNull.Value ? (decimal)reader["Balance_QTY_OF_PI"] : 0,
                                    Status_Id = reader["status_id"] != DBNull.Value && Guid.TryParse(reader["status_id"]?.ToString(), out Guid statusID) ? statusID : Guid.Empty,
                                    Status_Name = reader["status_name"]?.ToString(),
                                    Purchase_Indent_Id = reader["purchase_indent_id"] != DBNull.Value && Guid.TryParse(reader["purchase_indent_id"]?.ToString(), out Guid purchaseIndentId) ? purchaseIndentId : Guid.Empty,
                                    Track_No = reader["track_no"]?.ToString(),
                                    Voucher_Id = reader["voucher_id"] != DBNull.Value && Guid.TryParse(reader["voucher_id"]?.ToString(), out Guid voucherId) ? voucherId : Guid.Empty,
                                    Department_Id = reader["department_id"] != DBNull.Value && Guid.TryParse(reader["department_id"]?.ToString(), out Guid departmentId) ? departmentId : Guid.Empty,
                                    Dept_Name = reader["dept_name"]?.ToString(),
                                    //Narration = reader["narration"]?.ToString(),
                                    //Approval_Status = reader["Approval_Status"]?.ToString(),
                                    App_Status_Name = reader["App_Status_Name"]?.ToString(),
                                    Requested_By = reader["requested_by"] != DBNull.Value && Guid.TryParse(reader["requested_by"]?.ToString(), out Guid requestedBy) ? requestedBy : Guid.Empty,
                                    Last_Approved_By = reader["Last_Approved_By"]?.ToString()
                                    // You can set default values for any other properties if needed, such as Edit_Enable or Delete_Enable
                                   
                                });
                            }

                            // Check the output parameter for errors or success message
                            string outputMessage = outputMessageParam.Value?.ToString();

                            if (!string.IsNullOrEmpty(outputMessage) && outputMessage != "Data retrieved successfully.")
                            {
                                return NotFound(new { Message = outputMessage }); // Return error if no records found
                            }

                            return Ok(purchaseIndentDetailsList); // Return success with purchase indent details data
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [HttpGet("GetPurchaseOrderDetails")]
        public IActionResult GetPurchaseOrderDetails([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] Guid? statusId, [FromQuery] Guid? userId)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.sp_Get_PO_Register_Details", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add input parameters
                        command.Parameters.AddWithValue("@from_date", (object)fromDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@to_date", (object)toDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@user_id", (object)userId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@status_id", (object)statusId ?? DBNull.Value);

                        // Add output parameter
                        var outputMessageParam = new SqlParameter("@output_message", SqlDbType.NVarChar, -1)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputMessageParam);

                        using (var reader = command.ExecuteReader())
                        {
                            var purchaseOrderDetailsList = new List<PurchaseOrderDetails>();

                            while (reader.Read())
                            {
                                purchaseOrderDetailsList.Add(new PurchaseOrderDetails
                                {
                                    PONumber = reader["PO_Number"]?.ToString(),
                                    PODate = reader["PO_Date"] != DBNull.Value ? (DateTime)reader["PO_Date"] : (DateTime?)null,
                                    POOwner = reader["PO_Owner"]?.ToString(),
                                    SupplierName = reader["supplier_name"]?.ToString(),
                                    SupplierID = reader["supplier_id"]?.ToString(),
                                    LocationWHS = reader["Location_WHS"]?.ToString(),
                                    ItemName = reader["Item_name"]?.ToString(),
                                    ItemID = reader["Item_ID"]?.ToString(),
                                    ItemPOQty = reader["Item_PO_Qty"] != DBNull.Value ? (decimal)reader["Item_PO_Qty"] : 0,
                                    RecptQty = reader["Recpt_QTY"] != DBNull.Value ? (decimal)reader["Recpt_QTY"] : 0,
                                    Balance = reader["Balance"] != DBNull.Value ? (decimal)reader["Balance"] : 0,
                                    StatusId = reader["status_id"] != DBNull.Value && Guid.TryParse(reader["status_id"]?.ToString(), out Guid statusID) ? statusID : (Guid?)null,
                                    StatusName = reader["status_name"]?.ToString(),
                                    App_Status_Name = reader["approval_status_abbr"]?.ToString(),
                                    EXPRecptDate = reader["EXP_Rcpt_Date"] != DBNull.Value ? (DateTime?)reader["EXP_Rcpt_Date"] : null,
                                    PurchaseOrderTrnId = reader["purchase_Order_trn_id"] != DBNull.Value && Guid.TryParse(reader["purchase_Order_trn_id"]?.ToString(), out Guid poTrnId) ? poTrnId : (Guid?)null,
                                    //Narration = reader["narration"]?.ToString()
                                });
                            }

                            // Check output message
                            string outputMessage = outputMessageParam.Value?.ToString();

                            if (!string.IsNullOrEmpty(outputMessage) && outputMessage != "Data retrieved successfully.")
                            {
                                return NotFound(new { Message = outputMessage });
                            }

                            return Ok(purchaseOrderDetailsList);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [HttpGet("GetApprovalDuePIRegister")]
        public IActionResult GetApprovalDuePIRegister([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] Guid? statusId, [FromQuery] Guid? userId)
        {
            try
            {
                // Establish a connection to the database using the connection string from the configuration
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    // Initialize a SQL command to execute the stored procedure
                    using (var command = new SqlCommand("dbo.sp_Get_Approval_Due_PI_Register", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add input parameters to the stored procedure
                        command.Parameters.AddWithValue("@from_date", (object)fromDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@to_date", (object)toDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@user_id", (object)userId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@status_id", (object)statusId ?? DBNull.Value);

                        // Define an output parameter to capture messages from the stored procedure
                        var outputMessageParam = new SqlParameter("@output_message", SqlDbType.NVarChar, -1)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputMessageParam);

                        // Execute the stored procedure and process the results
                        using (var reader = command.ExecuteReader())
                        {
                            var approvalduePIDetailsList = new List<ApprovalDuePIRegister>();

                            while (reader.Read())
                            {
                                // Map the result set to the PurchaseOrderDetails class
                                approvalduePIDetailsList.Add(new ApprovalDuePIRegister
                                {
                                    PI_Number = reader["PI_Number"]?.ToString(),
                                    PI_Date = reader["PI_Date"] != DBNull.Value ? (DateTime)reader["PI_Date"] : (DateTime?)null,
                                    PI_Owner = reader["PI_Owner"]?.ToString(),
                                    Location_WHS = reader["Location_WHS"]?.ToString(),
                                    EXP_Rcpt_Date = reader["EXP_Rcpt_Date"] != DBNull.Value ? (DateTime?)reader["EXP_Rcpt_Date"] : null,
                                    Item_name = reader["Item_name"]?.ToString(),
                                    ItemID = reader["Item_ID"]?.ToString(),
                                    Item_PI_Qty = reader["Item_PI_Qty"] != DBNull.Value ? (decimal)reader["Item_PI_Qty"] : 0,
                                    PO_QTY = reader["PO_QTY"] != DBNull.Value ? (decimal)reader["PO_QTY"] : 0,
                                    Balance_QTY_OF_PI = reader["Balance_QTY_OF_PI"] != DBNull.Value ? (decimal)reader["Balance_QTY_OF_PI"] : 0,
                                    Status_Id = reader["status_id"] != DBNull.Value && Guid.TryParse(reader["status_id"]?.ToString(), out Guid statusID) ? statusID : Guid.Empty,
                                    Status_Name = reader["status_name"]?.ToString(),
                                    Purchase_Indent_Id = reader["purchase_indent_id"] != DBNull.Value && Guid.TryParse(reader["purchase_indent_id"]?.ToString(), out Guid purchaseIndentId) ? purchaseIndentId : Guid.Empty,
                                    
                                    Voucher_Id = reader["voucher_id"] != DBNull.Value && Guid.TryParse(reader["voucher_id"]?.ToString(), out Guid voucherId) ? voucherId : Guid.Empty,
                                    Department_Id = reader["department_id"] != DBNull.Value && Guid.TryParse(reader["department_id"]?.ToString(), out Guid departmentId) ? departmentId : Guid.Empty,
                                    Dept_Name = reader["dept_name"]?.ToString(),
                                  
                                    App_Status_Name = reader["App_Status_Name"]?.ToString(),
                                    Requested_By = reader["requested_by"] != DBNull.Value && Guid.TryParse(reader["requested_by"]?.ToString(), out Guid requestedBy) ? requestedBy : Guid.Empty,
                                    Last_Approved_By = reader["Last_Approved_By"]?.ToString()
                                    // You can set default values for any other properties if needed, such as Edit_Enable or Delete_Enable
                                });
                            }

                            // Check the output message from the stored procedure
                            string outputMessage = outputMessageParam.Value?.ToString();

                            if (!string.IsNullOrEmpty(outputMessage) && outputMessage != "Data retrieved successfully.")
                            {
                                return NotFound(new { Message = outputMessage });
                            }

                            // Return the result as an HTTP 200 response
                            return Ok(approvalduePIDetailsList);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle exceptions and return an HTTP 500 response with error details
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }

        [HttpGet("GetApprovalDuePORegister")]
        public IActionResult GetApprovalDuePORegister([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] Guid? statusId, [FromQuery] Guid? userId)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.sp_Get_Approval_Due_PO_Register", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add input parameters
                        command.Parameters.AddWithValue("@from_date", (object)fromDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@to_date", (object)toDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@user_id", (object)userId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@status_id", (object)statusId ?? DBNull.Value);

                        // Add output parameter
                        var outputMessageParam = new SqlParameter("@output_message", SqlDbType.NVarChar, -1)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputMessageParam);

                        using (var reader = command.ExecuteReader())
                        {
                            var purchaseOrderDetailsList = new List<PurchaseOrderDetails>();

                            while (reader.Read())
                            {
                                purchaseOrderDetailsList.Add(new PurchaseOrderDetails
                                {
                                    PONumber = reader["PO_Number"]?.ToString(),
                                    PODate = reader["PO_Date"] != DBNull.Value ? (DateTime)reader["PO_Date"] : (DateTime?)null,
                                    POOwner = reader["PO_Owner"]?.ToString(),
                                    SupplierName = reader["supplier_name"]?.ToString(),
                                    SupplierID = reader["supplier_id"]?.ToString(),
                                    LocationWHS = reader["Location_WHS"]?.ToString(),
                                    ItemName = reader["Item_name"]?.ToString(),
                                    ItemID = reader["Item_ID"]?.ToString(),
                                    ItemPOQty = reader["Item_PO_Qty"] != DBNull.Value ? (decimal)reader["Item_PO_Qty"] : 0,
                                    RecptQty = reader["Recpt_QTY"] != DBNull.Value ? (decimal)reader["Recpt_QTY"] : 0,
                                    Balance = reader["Balance"] != DBNull.Value ? (decimal)reader["Balance"] : 0,
                                    StatusId = reader["status_id"] != DBNull.Value && Guid.TryParse(reader["status_id"]?.ToString(), out Guid statusID) ? statusID : (Guid?)null,
                                    StatusName = reader["status_name"]?.ToString(),
                                    App_Status_Name = reader["approval_status_abbr"]?.ToString(),
                                    EXPRecptDate = reader["EXP_Rcpt_Date"] != DBNull.Value ? (DateTime?)reader["EXP_Rcpt_Date"] : null,
                                    PurchaseOrderTrnId = reader["purchase_Order_trn_id"] != DBNull.Value && Guid.TryParse(reader["purchase_Order_trn_id"]?.ToString(), out Guid poTrnId) ? poTrnId : (Guid?)null,
                                   
                                });
                            }

                            // Check output message
                            string outputMessage = outputMessageParam.Value?.ToString();

                            if (!string.IsNullOrEmpty(outputMessage) && outputMessage != "Data retrieved successfully.")
                            {
                                return NotFound(new { Message = outputMessage });
                            }

                            return Ok(purchaseOrderDetailsList);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }


        [HttpGet("GetPDCDetails")]
        public IActionResult GetPDCDetails([FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate, [FromQuery] Guid? statusId, [FromQuery] Guid? userId)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.sp_Get_PDC_Due_List", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add input parameters
                        command.Parameters.AddWithValue("@from_date", (object)fromDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@to_date", (object)toDate ?? DBNull.Value);
                        command.Parameters.AddWithValue("@user_id", (object)userId ?? DBNull.Value);
                        command.Parameters.AddWithValue("@status_id", (object)statusId ?? DBNull.Value);

                        // Add output parameter
                        var outputMessageParam = new SqlParameter("@output_message", SqlDbType.NVarChar, -1)
                        {
                            Direction = ParameterDirection.Output
                        };
                        command.Parameters.Add(outputMessageParam);

                        using (var reader = command.ExecuteReader())
                        {
                            var pdcDetailsList = new List<PDCDetails>();

                            while (reader.Read())
                            {
                                pdcDetailsList.Add(new PDCDetails
                                {
                                    PONumber = reader["PO_Number"]?.ToString(),
                                    PODate = reader["PO_Date"] != DBNull.Value ? (DateTime)reader["PO_Date"] : (DateTime?)null,
                                    POOwner = reader["PO_Owner"]?.ToString(),
                                    SupplierName = reader["supplier_name"]?.ToString(),
                                    SupplierID = reader["supplier_id"]?.ToString(),
                                    LocationWHS = reader["Location_WHS"]?.ToString(),
                                    ApprovalStatusAbbr = reader["approval_status_abbr"]?.ToString(),
                                    ItemName = reader["Item_name"]?.ToString(),
                                    ItemID = reader["Item_ID"]?.ToString(),
                                    POAmt = reader["PO_Amt"] != DBNull.Value ? (decimal)reader["PO_Amt"] : 0,
                                    PDCAmount = reader["PDC_Amount"] != DBNull.Value ? (decimal)reader["PDC_Amount"] : 0,
                                    Balance = reader["Balance"] != DBNull.Value ? (decimal)reader["Balance"] : 0,
                                    Status = reader["Status"]?.ToString(),
                                    StatusId = reader["status_id"] != DBNull.Value && Guid.TryParse(reader["status_id"]?.ToString(), out Guid statusID) ? statusID : (Guid?)null,
                                    PurchaseOrderTrnId = reader["purchase_Order_trn_id"] != DBNull.Value && Guid.TryParse(reader["purchase_Order_trn_id"]?.ToString(), out Guid poTrnId) ? poTrnId : (Guid?)null,
                                });
                            }

                            // Check output message
                            string outputMessage = outputMessageParam.Value?.ToString();

                            if (!string.IsNullOrEmpty(outputMessage) && outputMessage != "Data retrieved successfully.")
                            {
                                return NotFound(new { Message = outputMessage });
                            }

                            return Ok(pdcDetailsList);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
            }
        }


    }
}
