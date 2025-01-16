using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static Dugros_Api.Controllers.ColorController;
using System.Data.SqlClient;
using System.Data;
using Newtonsoft.Json;
using static Dugros_Api.Controllers.ChequeController;
using System.Data.Common;

namespace Dugros_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChequeController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ChequeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class PostCheque
        {
            public Guid user_id { get; set; }
            public List<ChequeDetail> ChequeDetails { get; set; }
        }

        public class ChequeDetail
        {
            public Guid po_trn_id { get; set; }
            public string cheque_no { get; set; }
            public DateTime cheque_dt { get; set; }
            public decimal cheque_amt { get; set; }
            public string bank_name { get; set; }
            public string paymentMode { get; set; }
            public List<FileUpload> cheque_file { get; set; }
        }

        public class GetCheque
        {
            public Guid po_trn_id { get; set; }
            public string voucher_no { get; set; }
            //public DateTime transaction_date { get; set; }
            public string transaction_date { get; set; }
            public string  Name { get; set; }
            public decimal net_bill_amt { get; set; }
            public decimal cheque_amt { get; set; }
            // public string bank_name { get; set; }
            public List<FileUpload> cheque_file { get; set; }
        }

        public class GetChequeList
        {
            public Guid cheque_trn_id { get; set; }
            public string cheque_no { get; set; }
            //public DateTime cheque_date { get; set; }
            public string cheque_date { get; set; }
            public decimal cheque_amt { get; set; }
            public string bank_name { get; set; }
            public string payment_mode { get; set; }
            public List<FileUpload> cheque_file { get; set; }
        }

        public class FileUpload
        {
            public string FileName { get; set; }
            public string Path { get; set; }
        }
        [HttpPost("postCheque")]
        public IActionResult AddCheque(PostCheque postCheque)
        {
            try
            {
                List<string> messages = new List<string>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    foreach (var chequeDetail in postCheque.ChequeDetails)
                    {
                        using (var command = new SqlCommand("dbo.sp_cheque_issue", connection))
                        {
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@user_id", postCheque.user_id);
                            command.Parameters.AddWithValue("@method", "Add");
                            command.Parameters.AddWithValue("@po_trn_id", chequeDetail.po_trn_id);
                            command.Parameters.AddWithValue("@cheque_no", chequeDetail.cheque_no);
                            command.Parameters.AddWithValue("@cheque_dt", chequeDetail.cheque_dt);
                            command.Parameters.AddWithValue("@cheque_amt", chequeDetail.cheque_amt);
                            command.Parameters.AddWithValue("@bank_name", chequeDetail.bank_name);
                            command.Parameters.AddWithValue("@payment_mode", chequeDetail.paymentMode);

                            if (chequeDetail.cheque_file != null && chequeDetail.cheque_file.Count > 0)
                            {
                                var pdfDataJson = JsonConvert.SerializeObject(chequeDetail.cheque_file);
                                command.Parameters.AddWithValue("@cheque_file", pdfDataJson);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@cheque_file", DBNull.Value);
                            }

                            // Add OUTPUT parameter to capture the stored procedure message
                            var outputParam = new SqlParameter("@Message", SqlDbType.NVarChar, 1000);
                            outputParam.Direction = ParameterDirection.Output;
                            command.Parameters.Add(outputParam);

                            // Execute the stored procedure
                            command.ExecuteNonQuery();

                            // Get the message from the output parameter
                            string message = command.Parameters["@Message"].Value.ToString();
                            messages.Add(message);
                        }
                    }
                }

                // Check the messages returned by the stored procedure
                if (messages.All(m => m.StartsWith("Record with cheque_no : ")))
                {
                    return Ok(new { ExecuteMessages = messages });
                }
                else
                {
                    return BadRequest(new { ExecuteMessages = messages }); // Return error messages
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("getCheque")]
        public IActionResult GetChequeIssued(Guid userId,string method,string? voucher_no,string? party_name,DateTime? start_dt,DateTime?end_dt,string? cheque_no)
        {
            try
            {

                List<GetCheque> itemCategories = new List<GetCheque>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.sp_cheque_issue", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@method", method);
                        command.Parameters.AddWithValue("@voucher_no", voucher_no ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@party_name", party_name ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@start_dt", start_dt == DateTime.MinValue ? (object)DBNull.Value : start_dt);
                        command.Parameters.AddWithValue("@end_dt", end_dt == DateTime.MinValue ? (object)DBNull.Value : end_dt);
                        command.Parameters.AddWithValue("@cheque_no", cheque_no ?? (object)DBNull.Value);



                        //Add OUTPUT parameter to capture the stored procedure message
                        var outputParam = new SqlParameter("@Message", SqlDbType.NVarChar, 1000);
                        outputParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputParam);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    GetCheque color = new GetCheque
                                    {
                                        po_trn_id = reader["po_trn_id"] == DBNull.Value ? Guid.Empty : (Guid)reader["po_trn_id"],
                                        voucher_no = reader["voucher_no"] == DBNull.Value ? string.Empty : reader["voucher_no"].ToString(),
                                        transaction_date = reader["transaction_date"].ToString(),
                                        Name = reader["Name"] == DBNull.Value ? string.Empty : reader["Name"].ToString(),
                                        net_bill_amt = reader["net_bill_amt"] == DBNull.Value ? 0m : (decimal)reader["net_bill_amt"],
                                        cheque_amt = reader["cheque_amt"] == DBNull.Value ? 0m : (decimal)reader["cheque_amt"],
                                        // bank_name = reader["bank_name"] == DBNull.Value ? string.Empty : reader["bank_name"].ToString()
                                    };
                                    // if (!reader.IsDBNull(reader.GetOrdinal("cheque_file")))
                                    // {
                                    //     string pdfDataJson = reader["cheque_file"].ToString();
                                    //     // Deserialize the JSON data to List<FileUpload2>
                                    //     List<FileUpload> pdfData = JsonConvert.DeserializeObject<List<FileUpload>>(pdfDataJson);
                                    //     color.cheque_file = pdfData;
                                    // }

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
                    return NotFound("No cheque found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("getChequeList")]
        public IActionResult GetChequedList(Guid userId, Guid po_trn_id)
        {
            try
            {

                List<GetChequeList> itemCategories = new List<GetChequeList>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.sp_cheque_issue", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@method", "ChequeList");
                        command.Parameters.AddWithValue("@po_trn_id", po_trn_id);



                        //Add OUTPUT parameter to capture the stored procedure message
                        var outputParam = new SqlParameter("@Message", SqlDbType.NVarChar, 1000);
                        outputParam.Direction = ParameterDirection.Output;
                        command.Parameters.Add(outputParam);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    GetChequeList color = new GetChequeList
                                    {
                                        cheque_trn_id = reader["cheque_trn_id"] == DBNull.Value ? Guid.Empty : (Guid)reader["cheque_trn_id"],
                                        cheque_no = reader["cheque_no"] == DBNull.Value ? string.Empty : reader["cheque_no"].ToString(),
                                        cheque_date = reader["cheque_date"].ToString(),
                                        cheque_amt = reader["cheque_amt"] == DBNull.Value ? 0m : (decimal)reader["cheque_amt"],
                                        bank_name = reader["bank_name"] == DBNull.Value ? string.Empty : reader["bank_name"].ToString(),
                                        payment_mode = reader["payment_mode"] == DBNull.Value ? string.Empty : reader["payment_mode"].ToString()
                                    
                                    };
                                    if (!reader.IsDBNull(reader.GetOrdinal("cheque_file")))
                                    {
                                        string pdfDataJson = reader["cheque_file"].ToString();
                                        // Deserialize the JSON data to List<FileUpload2>
                                        List<FileUpload> pdfData = JsonConvert.DeserializeObject<List<FileUpload>>(pdfDataJson);
                                        color.cheque_file = pdfData;
                                    }

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
                    return NotFound("No cheque found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteItemCategory(Guid userId, Guid cheque_trn_id)
        {
            try
            {
                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.sp_cheque_issue", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@method", "Delete");
                        command.Parameters.AddWithValue("@user_id", userId);
                        command.Parameters.AddWithValue("@cheque_trn_id", cheque_trn_id);

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
