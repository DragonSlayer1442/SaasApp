using DocSaaSApp.Data;
using DocSaaSApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DocSaaSApp.Controllers
{
    public class UserFilesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _env;

        public UserFilesController(ApplicationDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var files = await _context.UserFiles.OrderByDescending(f => f.UploadDate).ToListAsync();
            ViewBag.FileCount = files.Count;
            return View(files);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile uploadedFile)
        {
            if (uploadedFile == null || uploadedFile.Length == 0)
            {
                ModelState.AddModelError("", "Please select a file.");
                return View();
            }

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(uploadedFile.FileName);
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await uploadedFile.CopyToAsync(stream);
            }

            var userFile = new UserFile
            {
                FileName = uploadedFile.FileName,
                FilePath = "/uploads/" + uniqueFileName,
                UploadDate = DateTime.Now
            };

            _context.UserFiles.Add(userFile);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var file = await _context.UserFiles.FindAsync(id);
            if (file == null) return NotFound();

            var fullPath = Path.Combine(_env.WebRootPath, file.FilePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));
            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);

            _context.UserFiles.Remove(file);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
