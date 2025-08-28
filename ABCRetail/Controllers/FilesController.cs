using ABCRetail.Models;
using ABCRetail.Services;
using Microsoft.AspNetCore.Mvc;
using FileShare = ABCRetail.Services.FileShare;

namespace ABCRetail.Controllers
{
    public class FilesController : Controller
    {
        private readonly FileShare _fileShare;

        public FilesController(FileShare fileShare)
        {
            _fileShare = fileShare;
        }

        public async Task<IActionResult> Index()
        {
            List<FileModel> files;
            try
            {
                files = await _fileShare.ListFilesAsync("upload");
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Failed to load files: {ex.Message}";
                files = new List<FileModel>();
            }
            return View(files);
        }
        [HttpPost]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("File", "Please select a file to upload");
                return await Index();
            }
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    string directoryName = "upload";
                    string fileName = file.FileName;
                    await _fileShare.UploadFileAsync(directoryName, fileName, stream);
                }
                TempData["Message"] = $"File '{file.FileName}' uploaded successfully";
            }
            catch (Exception e)
            {
                TempData["Message"] = $"File upload failed: {e.Message}";
            }
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return BadRequest("File name cannot be null or empty");
            }
            try
            {
                var fileStream = await _fileShare.DownloadFileAsync("uploads", fileName);
                if (fileStream == null)
                {
                    return NotFound($"File '{fileName}' not found");
                }
                return File(fileStream, "application/octet-stream", fileName);
            }
            catch (Exception e)
            {
                return BadRequest($"Failed to download file {e.Message}");
            }
        }
    }
}
