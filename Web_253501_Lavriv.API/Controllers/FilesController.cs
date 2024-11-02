using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Web_253501_Lavriv.Domain.Entities;

namespace Web_253501_Lavriv.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly string _imagePath;

        public FilesController(IWebHostEnvironment webHost)
        {
            _imagePath = Path.Combine(webHost.WebRootPath, "Images");
            if (!Directory.Exists(_imagePath))
            {
                Directory.CreateDirectory(_imagePath);
            }
        }

        [HttpPost]
        public async Task<IActionResult> SaveFile(IFormFile file)
        {
            if (file == null)
            {
                return BadRequest("Файл не передан.");
            }

            // Генерация случайного имени для файла, чтобы избежать дублирования
            var uniqueFileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{Path.GetRandomFileName()}{Path.GetExtension(file.FileName)}";
            var filePath = Path.Combine(_imagePath, uniqueFileName);

            // Сохранение файла
            using var fileStream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(fileStream);

            // URL для доступа к файлу
            var fileUrl = $"{Request.Scheme}://{Request.Host}/Images/{uniqueFileName}";
            return Ok(fileUrl);
        }

        [HttpDelete("{fileName}")]
        public IActionResult DeleteFile(string fileName)
        {
            var filePath = Path.Combine(_imagePath, fileName);
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound("Файл не найден.");
            }

            System.IO.File.Delete(filePath);
            return Ok("Файл успешно удален.");
        }


        

    }
}
