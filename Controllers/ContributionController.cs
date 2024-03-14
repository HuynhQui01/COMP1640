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

namespace Comp1640.Controllers
{
    public class ContributionController : Controller
    {
        private readonly Comp1640Context _context;

        private readonly IWebHostEnvironment _webHost;
        private readonly UserManager<IdentityUser> _userManager;



        public ContributionController(IWebHostEnvironment webHost, Comp1640Context context, UserManager<IdentityUser> userManager)
        {
            _webHost = webHost;
            _context = context;
            _userManager = userManager;
        }

        // GET: Contribution
        public async Task<IActionResult> Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Student"))
                {
                    return _context.Contributions != null ?
                          View(await _context.Contributions.ToListAsync()) :
                          Problem("Entity set 'Comp1640Context.Contributions'  is null.");
                }
            }
            return Redirect("/");
        }

             public async Task<IActionResult> ManageContribution()
        {
             if (User.Identity.IsAuthenticated)
            {
                if (User.IsInRole("Marketing Coordinator "))
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

            return RedirectToAction(nameof(ManageContribution));

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
            if (User.Identity.IsAuthenticated){
                if (User.IsInRole("Student")){
                    ViewData["FacId"] = new SelectList(_context
                    .Faculties, "FacId", "FacName");
                    return View();
                }
            }
            return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> Create(IFormFile file, [Bind("ConID,ConName,UserId,Status,Filepath,FeedbackId,SubmitDate")] Contribution contribution)
        {
            if (file == null || file.Length == 0)
            {
                ViewBag.Message = "Error: No file selected or empty file.";
                return View();
            }

            try
            {
                string uploadsFolder = Path.Combine(_webHost.WebRootPath, "uploads");

                // Ensure the uploads directory exists
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Sanitize the filename to prevent directory traversal attacks
                string fileName = Path.GetFileName(file.FileName);
                var filepath= fileName;
                fileName = Path.Combine(uploadsFolder, fileName);

                // Check if the file already exists and generate a unique filename if necessary
                if (System.IO.File.Exists(fileName))
                {
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);
                    string fileExtension = Path.GetExtension(fileName);
                    fileName = Path.Combine(uploadsFolder, $"{fileNameWithoutExtension}_{DateTime.Now.Ticks}{fileExtension}");
                }

                // Validate the file (e.g., size, type) before saving
                if (file.Length > 0 && IsFileValid(file))
                {
                    // Use asynchronous file operations for improved performance
                    using (FileStream stream = new FileStream(fileName, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    var userID = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var count = await _context.Contributions.CountAsync();
                    contribution.ConId = count + 1;
                    contribution.UserId = userID;
                    contribution.Filepath = filepath;

                    contribution.Status = "Pending";
                    contribution.SubmitDate = DateTime.Now;
                    _context.Add(contribution);
                    await _context.SaveChangesAsync();

                    ViewBag.Message = $"{Path.GetFileName(fileName)} uploaded successfully!";
                }
                else
                {
                    ViewBag.Message = "Error: Invalid file.";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error: {ex.Message}";
            }

            return View();
        }

        private bool IsFileValid(IFormFile file)
        {
            // Example validation: Check file size and allowed extensions
            long fileSizeLimit = 10 * 1024 * 1024; // 10 MB
            string[] allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif", ".docx", ".doc", ".pdf" };

            return file.Length <= fileSizeLimit && allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLower());
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
        public async Task<IActionResult> Edit(int id, [Bind("ConId,ConName,UserId,Startus,File")] Contribution contribution)
        {
            if (id != contribution.ConId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(contribution);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContributionExists(contribution.ConId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(contribution);
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
