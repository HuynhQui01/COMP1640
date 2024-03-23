using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Comp1640.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.IO.Compression;
using Microsoft.AspNetCore.StaticFiles;

namespace Comp1640.Controllers
{
    public class ContributionController : Controller
    {
        private readonly Comp1640Context _context;

        private readonly IWebHostEnvironment _webHost;
        private readonly UserManager<CustomUser> _userManager;



        public ContributionController(IWebHostEnvironment webHost, Comp1640Context context, UserManager<CustomUser> userManager)
        {
            _webHost = webHost;
            _context = context;
            _userManager = userManager;
        }

        // GET: Contribution
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated && User.IsInRole("Student"))
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var userContributions = await _context.Contributions
                        .Where(c => c.UserId == user.Id)
                        .Include(f => f.Fac) // Bổ sung Include để load thông tin về khoa (faculty)
                        .ToListAsync();

                    return View(userContributions);
                }
            }

            return Redirect("/");
        }
        public IActionResult Approve()
        {
            var approvedContributions = _context.Contributions
                .Where(c => c.Status == "Approved")
                .Include(c => c.User)
                .Include(c => c.Fac)
                .ToList();

            return View(approvedContributions);
        }

        [HttpPost]
        public IActionResult Search(string searchString)
        {
            var contributions = _context.Contributions
                .Where(c => c.Status == "Approved" && c.ConName.Contains(searchString))
                .Include(c => c.User)
                .Include(c => c.Fac)
                .ToList();

            return View("Approve", contributions);
        }
        [HttpGet]
        public async Task<IActionResult> Download(int fileId)
        {
            var contribution = await _context.Contributions.FindAsync(fileId);

            if (contribution == null)
            {
                return NotFound();
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", contribution.Filepath);

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return PhysicalFile(filePath, contentType, contribution.Filepath);
        }

        [HttpGet]
        public async Task<IActionResult> DownloadApproved()
        {
            var approvedContributions = await _context.Contributions
                .Where(c => c.Status == "Approved")
                .ToListAsync();
            var memoryStream = new MemoryStream();
            try
            {
                using (var archive = new ZipArchive(memoryStream, ZipArchiveMode.Create, leaveOpen: true))
                {
                    foreach (var contribution in approvedContributions)
                    {
                        var fileDetails = await _context.Contributions
                            .Where(fd => fd.ConId == contribution.ConId)
                            .ToListAsync();

                        foreach (var fileDetail in fileDetails)
                        {
                            var filePath = Path.Combine(_webHost.WebRootPath, "uploads", fileDetail.Filepath);

                            if (System.IO.File.Exists(filePath))
                            {
                                var entry = archive.CreateEntry(Path.GetFileName(filePath));

                                using (var entryStream = entry.Open())
                                using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                                {
                                    await fileStream.CopyToAsync(entryStream);
                                }
                            }
                        }
                    }
                }
                memoryStream.Position = 0;
                return File(memoryStream, "application/zip", "ApprovedFiles.zip");
            }
            catch
            {
                memoryStream.Close();
                throw;
            }
        }

        public async Task<IActionResult> Manage()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Coordinator"))
                {
                    return _context.Contributions != null ?
                          View(await _context.Contributions.ToListAsync()) :
                          Problem("Entity set 'Comp1640Context.Contributions'  is null.");
                }
            }
            return Redirect("/");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(int id, string status)
        {
            var contribution = await _context.Contributions.FindAsync(id);
            if (contribution == null)
            {
                return NotFound();
            }

            switch (status)
            {
                case "Rejected":
                    contribution.Status = "Rejected";
                    break;
                case "Approved":
                    contribution.Status = "Approved";
                    break;
                default:
                    contribution.Status = "Pending";
                    break;
            }

            _context.Update(contribution);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Manage));

        }
        // GET: Contribution/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Contributions == null)
            {
                return NotFound();
            }

            var contribution = await _context.Contributions
                .FirstOrDefaultAsync(m => m.ConId == id);
            if (contribution == null)
            {
                return NotFound();
            }

            return View(contribution);
        }

        // GET: Contribution/Create
        public IActionResult Create()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Student"))
                {
                    ViewData["FacId"] = new SelectList(_context
                    .Faculties, "FacId", "FacName");
                    return View();
                }
            }
            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile imageFile, IFormFile file, [Bind("ConID,ConName,UserId,Status,Filepath,FeedbackId,SubmitDate,FacId")] Contribution contribution)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Error: No file selected or empty file.";
                return View();
            }

            try
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "uploads");

                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string fileName = Path.GetFileName(file.FileName);
                string imageFileName = Path.GetFileName(imageFile.FileName);

                var filepath = fileName;
                var imageFilePath = imageFileName;

                fileName = Path.Combine(uploadsFolder, fileName);
                imageFileName = Path.Combine(uploadsFolder, imageFileName);

                if (System.IO.File.Exists(fileName))
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    string fileExtension = Path.GetExtension(fileName);
                    fileName = Path.Combine(uploadsFolder, $"{fileNameWithoutExtension}_{DateTime.Now.Ticks}{fileExtension}");
                }

                if (file.Length > 0 && imageFile.Length > 0 && IsFileValid(file) && IsImageFileValid(imageFile))
                {
                    using (FileStream fileStream = new FileStream(fileName, FileMode.Create))
                    using (FileStream imageFileStream = new FileStream(imageFileName, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                        await imageFile.CopyToAsync(imageFileStream);
                    }

                    var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var count = await _context.Contributions.CountAsync();

                    contribution.UserId = userID;
                    contribution.Filepath = filepath;
                    contribution.ImageFilePath = imageFilePath;
                    contribution.Status = "Pending";
                    contribution.SubmitDate = DateTime.Now;

                    _context.Add(contribution);
                    await _context.SaveChangesAsync();

                    ViewBag.Message = $"{Path.GetFileName(fileName)} uploaded successfully!";
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Error: Invalid file.");
                    Create();
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        private bool IsFileValid(IFormFile file)
        {
            long fileSizeLimit = 3 * 1024 * 1024;
            string[] allowedExtensions = { ".docx", ".doc" };

            return file.Length <= fileSizeLimit && allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower());
        }

        private bool IsImageFileValid(IFormFile file)
        {
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png" };
            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            return allowedExtensions.Contains(fileExtension);
        }

        // GET: Contribution/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Contributions == null)
            {
                return NotFound();
            }

            var contribution = await _context.Contributions.FindAsync(id);
            if (contribution == null)
            {
                return NotFound();
            }
            return View(contribution);
        }

        // POST: Contribution/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, IFormFile file, [Bind("ConID,ConName,UserId,Status,Filepath,FeedbackId,SubmitDate,FacId")] Contribution newCon)
        {
            var existingContribution = await _context.Contributions.FindAsync(id);
            if (file != null && file.Length > 0 && IsFileValid(file))
            {
                if (!string.IsNullOrEmpty(existingContribution.Filepath))
                {
                    string existingFilePath = Path.Combine(_webHost.WebRootPath, "uploads", existingContribution.Filepath);
                    if (System.IO.File.Exists(existingFilePath))
                    {
                        System.IO.File.Delete(existingFilePath);
                    }
                }
                string uniqueFileName = GetUniqueFileName(file.FileName);
                string newFilePath = Path.Combine(_webHost.WebRootPath, "uploads", uniqueFileName);
                using (var fileStream = new FileStream(newFilePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                existingContribution.Filepath = uniqueFileName;
                await _context.SaveChangesAsync();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Error: Invalid file.");
            }
            return RedirectToAction(nameof(Index));
        }
        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }

        // GET: Contribution/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Contributions == null)
            {
                return NotFound();
            }

            var contribution = await _context.Contributions
                .FirstOrDefaultAsync(m => m.ConId == id);
            if (contribution == null)
            {
                return NotFound();
            }

            return View(contribution);
        }

        // POST: Contribution/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Contributions == null)
            {
                return Problem("Entity set 'Comp1640Context.Contributions'  is null.");
            }
            var contribution = await _context.Contributions.FindAsync(id);
            if (contribution != null)
            {
                _context.Contributions.Remove(contribution);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContributionExists(int id)
        {
            return (_context.Contributions?.Any(e => e.ConId == id)).GetValueOrDefault();
        }
    }
}
