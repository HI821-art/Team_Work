using HotelDb.Data;
using Microsoft.AspNetCore.Mvc;

namespace HotelApp.Controllers
{
    public class RoomsImgController : Controller
    {
       
        private readonly IWebHostEnvironment _env;

        public RoomsImgController( IWebHostEnvironment env)
        {
           
            _env = env;
        }


        // POST: Rooms/UploadRoomImages
        [HttpPost]
        public async Task<IActionResult> UploadRoomImages(List<IFormFile> RoomImages)
        {
            var imageUrls = new List<string>();
            var uploadsFolder = Path.Combine(_env.WebRootPath, "images", "rooms");
            Directory.CreateDirectory(uploadsFolder);

            foreach (var file in RoomImages)
            {
                if (file.Length > 0)
                {
                    var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using var stream = new FileStream(filePath, FileMode.Create);
                    await file.CopyToAsync(stream);

                    var relativePath = "/images/rooms/" + uniqueFileName;
                    imageUrls.Add(relativePath);
                }
            }

            return Json(new { success = true, images = imageUrls });
        }

        [HttpPost]
        public IActionResult DeleteRoomImage([FromBody] ImageDeleteRequest request)
        {
            if (string.IsNullOrWhiteSpace(request?.ImageUrl))
                return BadRequest();

            var decodedUrl = Uri.UnescapeDataString(request.ImageUrl);
            var filePath = Path.Combine(_env.WebRootPath, decodedUrl.TrimStart('/'));

            try
            {
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                    Console.WriteLine($"Deleted: {filePath}");
                }
                else
                {
                    Console.WriteLine($"File not found: {filePath}");
                }

                return Json(new { success = true });
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
