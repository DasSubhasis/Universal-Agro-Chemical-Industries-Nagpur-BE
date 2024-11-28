using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dugros_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public FileController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public class FileUploadModel
        {
            public List<IFormFile> Files { get; set; } // Change to List<IFormFile> to handle multiple files
        }



        public class FileUploadModel1
        {
            public IFormFile File { get; set; }
        }

        [HttpPost("logo"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadLogo([FromForm] FileUploadModel1 model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                return BadRequest("Invalid File");
            }

            var folderName = Path.Combine("Docs", "uploads", "AllFiles");
            var pathtoSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (!Directory.Exists(pathtoSave))
            {
                Directory.CreateDirectory(pathtoSave);
            }

            var filename = $"{Path.GetFileNameWithoutExtension(model.File.FileName)}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss_fff}{Path.GetExtension(model.File.FileName)}";
            var fullPath = Path.Combine(pathtoSave, filename);
            var dbPath = Path.Combine(folderName, filename).Replace("\\", "/"); // Replace backslashes with forward slashes

            if (System.IO.File.Exists(fullPath))
            {
                return BadRequest("File already exists");
            }

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            // Get the base URL of your application
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}";

            // Generate the full URL path
            var fullUrlPath = $"{baseUrl}/{dbPath}";

            // Automatically fetch filename and content type
            var contentType = model.File.ContentType;

            return Ok(new { fullUrlPath, filename, contentType });
        }

        [HttpPost("uploadcheque"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFile([FromForm] FileUploadModel1 model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                return BadRequest("Invalid File");
            }

            var folderName = Path.Combine("Docs", "uploads", "AllFiles");
            var pathtoSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (!Directory.Exists(pathtoSave))
            {
                Directory.CreateDirectory(pathtoSave);
            }

            var filename = $"{Path.GetFileNameWithoutExtension(model.File.FileName)}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss_fff}{Path.GetExtension(model.File.FileName)}";
            var fullPath = Path.Combine(pathtoSave, filename);
            var dbPath = Path.Combine(folderName, filename).Replace("\\", "/"); // Replace backslashes with forward slashes

            if (System.IO.File.Exists(fullPath))
            {
                return BadRequest("File already exists");
            }

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            // Get the base URL of your application
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}";

            // Generate the full URL path
            var fullUrlPath = $"{baseUrl}/{dbPath}";

            // Automatically fetch filename and content type
            var contentType = model.File.ContentType;

            return Ok(new { fullUrlPath, filename, contentType });
        }


        [HttpPost("upload"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadFiles([FromForm] FileUploadModel model)
        {
            if (model.Files == null || model.Files.Count == 0)
            {
                return BadRequest("No files selected for upload.");
            }

            var folderName = Path.Combine("Docs", "uploads", "AllFiles");
            var pathtoSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

            if (!Directory.Exists(pathtoSave))
            {
                Directory.CreateDirectory(pathtoSave);
            }

            var uploadedFiles = new List<object>();

            foreach (var file in model.Files)
            {
                Console.WriteLine($"Received file: {file.FileName}");
                if (file.Length == 0)
                {
                    return BadRequest($"File '{file.FileName}' is empty.");
                }

                var filename = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss_fff}{Path.GetExtension(file.FileName)}";
                var fullPath = Path.Combine(pathtoSave, filename);
                var dbPath = Path.Combine(folderName, filename).Replace("\\", "/");

                if (System.IO.File.Exists(fullPath))
                {
                    return BadRequest($"File '{file.FileName}' already exists.");
                }

                using (var stream = new FileStream(fullPath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Get the base URL of your application
                var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}";

                // Generate the full URL path
                var fullUrlPath = $"{baseUrl}/{dbPath}";

                // Automatically fetch filename and content type
                var contentType = file.ContentType;

                uploadedFiles.Add(new { fullUrlPath, filename, contentType });
            }

            return Ok(uploadedFiles);
        }


        [HttpPost("uploadexcel"), DisableRequestSizeLimit]
        public async Task<IActionResult> UploadExcel([FromForm] FileUploadModel1 model)
        {
            if (model.File == null || model.File.Length == 0)
            {
                return BadRequest("Invalid File");
            }

            var folderName = Path.Combine("Docs", "uploads", "AllFiles");
            var pathtoSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);
            if (!Directory.Exists(pathtoSave))
            {
                Directory.CreateDirectory(pathtoSave);
            }

            var filename = $"{Path.GetFileNameWithoutExtension(model.File.FileName)}_{DateTime.Now:yyyy_MM_dd_HH_mm_ss_fff}{Path.GetExtension(model.File.FileName)}";
            var fullPath = Path.Combine(pathtoSave, filename);
            var dbPath = Path.Combine(folderName, filename).Replace("\\", "/"); // Replace backslashes with forward slashes

            if (System.IO.File.Exists(fullPath))
            {
                return BadRequest("File already exists");
            }

            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await model.File.CopyToAsync(stream);
            }

            // Get the base URL of your application
            var baseUrl = $"{this.Request.Scheme}://{this.Request.Host}";

            // Generate the full URL path
            var fullUrlPath = $"{baseUrl}/{dbPath}";

            // Automatically fetch filename and content type
            var contentType = model.File.ContentType;

            return Ok(new { fullUrlPath, filename, contentType });
        }
    }
}
