using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Data;
using static Dugros_Api.Controllers.ColorController;

namespace Dugros_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MenuController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public MenuController(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public class Menu
        {
            public Guid id { get; set; }
            public string menu { get; set; }
        }

        [HttpGet("Menu")]
        public IActionResult GetMenu()
        {
            try
            {
                List<Menu> menus = new List<Menu>();

                using (var connection = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
                {
                    connection.Open();

                    using (var command = new SqlCommand("MenuGet", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        using (var reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                while (reader.Read())
                                {
                                    Menu menu = new Menu
                                    {
                                        id = reader.GetGuid(reader.GetOrdinal("id")),
                                        menu = reader.GetString(reader.GetOrdinal("menu")),
                                    };

                                    menus.Add(menu);
                                }
                            }
                        }
                    }
                }

                if (menus.Any())
                {
                    return Ok(menus);
                }
                else
                {
                    return NotFound("No menu items found.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

    }
}
