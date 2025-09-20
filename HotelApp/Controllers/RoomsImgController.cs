using HotelDb.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HotelApp.Controllers
{
    public class RoomsImgController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public RoomsImgController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UploadRoomImages(List<IFormFile> RoomImages)
        {
            if (RoomImages == null || RoomImages.Count == 0)
                return Json(new { success = false, error = "No files uploaded." });

            var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "rooms");
            Directory.CreateDirectory(uploadsFolder);

            var imageUrls = new List<string>();

            foreach (var file in RoomImages)
            {
                if (file.Length == 0) continue;

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
                if (!allowed.Contains(ext)) continue;

                var uniqueFileName = $"{Guid.NewGuid()}{ext}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                imageUrls.Add($"/images/rooms/{uniqueFileName}");
            }

            return Json(new { success = true, images = imageUrls });
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteRoomImage([FromBody] ImageDeleteRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.ImageUrl))
                return BadRequest("Image URL is required");

            var decodedUrl = Uri.UnescapeDataString(request.ImageUrl);
            var filePath = Path.Combine(_env.WebRootPath, decodedUrl.TrimStart('/'));

            Console.WriteLine($"Attempting to delete: {filePath}");

            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    Console.WriteLine($"Successfully deleted: {filePath}");
                    return Json(new { success = true });
                }
                else
                {
                    Console.WriteLine($"File not found: {filePath}");
                    return Json(new { success = false, error = "File not found" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Delete error: {ex.Message}");
                return StatusCode(500, new { success = false, error = ex.Message });
            }
        }

        public class ImageDeleteRequest
        {
            public string ImageUrl { get; set; }
        }
    }
}