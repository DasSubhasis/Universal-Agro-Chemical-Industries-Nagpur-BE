using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using static Dugros_Api.Controllers.ColorController;
using System.Data.SqlClient;
using System.Data;
using Microsoft.AspNetCore.Authorization;

namespace Dugros_Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ReportController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class TileResponse
        {
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public int Value { get; set; }
            public string Color { get; set; }
            public string Route { get; set; }
            public string Icon { get; set; }
        }

        public class NotificationGet
        {
            public string Title { get; set; }
            public string Subtitle { get; set; }
            public int Value { get; set; }
            public string Color { get; set; }
            public string Route { get; set; }
            public string Icon { get; set; }
        }

        [HttpGet]
        public IActionResult GetItemCategories(Guid userId)
        {
            try
            {
                List<TileResponse> tileResponses = new List<TileResponse>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.Tiles_GET", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    if (reader["pi_activity"].ToString()== "PI Creator" )
                                    {
                                        // Adding PI Creator tile
                                        tileResponses.Add(new TileResponse
                                        {
                                            Title = "PI Creator",
                                            Subtitle = reader["pi_sub_title"].ToString(),
                                            Value = (int)reader["pi_pending"],
                                            Color = "#3498db",
                                            Route = reader["pi_route"].ToString(),
                                            Icon = "fas fa-file-alt fa-sm"

                                        });
                                    }
                                    else if (reader["pi_activity"].ToString() == "PI Approver" || reader["pi_activity"].ToString() == "PI Special Approver")
                                    {
                                        // Adding PI Approver tile
                                        tileResponses.Add(new TileResponse
                                        {
                                            Title = "PI Approver",
                                            Subtitle = reader["pi_sub_title"].ToString(),
                                            Value = (int)reader["pi_pending"], // or other relevant column
                                            Color = "#2ecc71",
                                            Route = reader["pi_route"].ToString(),
                                            Icon = "fas fa-check-circle fa-sm"
                                        });
                                    }
                                    




                                    if (reader["po_activity"].ToString() == "PO Creator" )
                                    {
                                        // Adding PO Creator tile
                                        tileResponses.Add(new TileResponse
                                        {
                                            Title = "PO Creator",
                                            Subtitle = reader["po_sub_title"].ToString(),
                                            Value = (int)reader["po_pending"],
                                            Color = "#e74c3c",
                                            Route = reader["po_route"].ToString(),
                                            Icon = "fas fa-shopping-cart fa-sm"
                                        });

                                    }
                                    else if (reader["po_activity"].ToString() == "PO Approver" || reader["po_activity"].ToString() == "PO Special Approver")
                                    {
                                        // Adding PO Approver tile
                                        tileResponses.Add(new TileResponse
                                        {
                                            Title = "PO Approver",
                                            Subtitle = reader["po_sub_title"].ToString(),
                                            Value = (int)reader["po_pending"], // or other relevant column
                                            Color = "#5D6D7E",
                                            Route = reader["po_route"].ToString(),
                                            Icon = "fas fa-thumbs-up fa-sm"
                                        });
                                    }
                                    


                                    if (reader["pdc_access"] != DBNull.Value && (bool)reader["pdc_access"] )
                                    {
                                        // Adding PDC Issuer tile
                                        tileResponses.Add(new TileResponse
                                        {
                                            Title = "PDC Issuer",
                                            Subtitle = reader["pdc_sub_title"]?.ToString(),
                                            Value = reader["pdc_pending"] != DBNull.Value ? (int)reader["pdc_pending"] : 0,
                                            Color = "#9b59b6",
                                            Route = reader["pdc_route"]?.ToString(),
                                            Icon = "fas fa-hand-holding-usd fa-sm"
                                        });
                                    }
                                   




                                }
                            }
                        }
                    }
                }

                return Ok(tileResponses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("Notifications")]
        public IActionResult GetNotifications(Guid userId)
        {
            try
            {
                List<TileResponse> tileResponses = new List<TileResponse>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("dbo.Notification_GET", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@user_id", userId);

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    if (reader["pi_activity"].ToString() == "Creator" || reader["po_activity"].ToString() == "Creator")
                                    {
                                        // Adding PI Creator tile
                                        tileResponses.Add(new TileResponse
                                        {
                                            Title = reader["pi_title"].ToString(),
                                            Subtitle = reader["pi_sub_title"].ToString(),
                                            Value = (int)reader["pi_pending"],
                                            Color = "#3498db",
                                            Route = reader["pi_route"].ToString(),
                                            Icon = "fas fa-file-alt fa-sm"

                                        });
                                        tileResponses.Add(new TileResponse
                                        {
                                            Title = reader["pi_awaiting_title"].ToString(),
                                            Subtitle = reader["pi_awaiting_sub_title"].ToString(),
                                            Value = (int)reader["pi_awaiting_pending"],
                                            Color = "#27ae60",
                                            Route = reader["po_route"].ToString(),
                                            Icon = "fas fa-clock fa-sm"

                                        });
                                    }
                                    else
                                    {
                                        // Adding PI Approver tile
                                        tileResponses.Add(new TileResponse
                                        {
                                            Title = reader["pi_title"].ToString(),
                                            Subtitle = reader["pi_sub_title"].ToString(),
                                            Value = (int)reader["pi_pending"], // or other relevant column
                                            Color = "#2ecc71",
                                            Route = reader["pi_route"].ToString(),
                                            Icon = "fas fa-check-circle fa-sm"
                                        });
                                    }



                                    if (reader["po_activity"].ToString() == "PO Creator")
                                    {
                                        // Adding PO Creator tile
                                        tileResponses.Add(new TileResponse
                                        {
                                            Title = reader["po_title"].ToString(),
                                            Subtitle = reader["po_sub_title"].ToString(),
                                            Value = (int)reader["po_pending"],
                                            Color = "#e74c3c",
                                            Route = reader["po_route"].ToString(),
                                            Icon = "fas fa-shopping-cart fa-sm"
                                        });

                                    }
                                    else
                                    {
                                        // Adding PO Approver tile
                                        tileResponses.Add(new TileResponse
                                        {
                                            Title = reader["po_title"].ToString(),
                                            Subtitle = reader["po_sub_title"].ToString(),
                                            Value = (int)reader["po_pending"], // or other relevant column
                                            Color = "#5D6D7E",
                                            Route = reader["po_route"].ToString(),
                                            Icon = "fas fa-thumbs-up fa-sm"
                                        });
                                    }


                                    if (reader["pdc_access"] != DBNull.Value && (bool)reader["pdc_access"])
                                    {
                                        // Adding PDC Issuer tile
                                        tileResponses.Add(new TileResponse
                                        {
                                            Title = "PDC Issuer",
                                            Subtitle = reader["pdc_sub_title"]?.ToString(),
                                            Value = reader["pdc_pending"] != DBNull.Value ? (int)reader["pdc_pending"] : 0,
                                            Color = "#9b59b6",
                                            Route = reader["pdc_route"]?.ToString(),
                                            Icon = "fas fa-hand-holding-usd fa-sm"
                                        });
                                    }


                                }
                            }
                        }
                    }
                }

                return Ok(tileResponses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

    }
}

