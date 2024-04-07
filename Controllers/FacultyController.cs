using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comp1640.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Comp1640.Controllers
{
    public class FacultyController : Controller
    {
        private readonly Comp1640Context _context;

        public FacultyController(Comp1640Context context)
        {
            _context = context;
        }

        // GET: Faculty
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var comp1640Context = _context.Faculties;
            return View(await comp1640Context.ToListAsync());
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ViewF()
        {
            var comp1640Context = _context.Faculties;
            return View(await comp1640Context.ToListAsync());
        }

        // GET: Faculty/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Faculties == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties.FirstOrDefaultAsync(m => m.FacId == id);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // GET: Faculty/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            ViewData["Ayid"] = new SelectList(_context.AcademicYears, "Ayid", "CloseDate");
            return View();
        }

        // POST: Faculty/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FacId,FacName")] Faculty faculty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(faculty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(faculty);
        }

        // GET: Faculty/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Faculties == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties.FindAsync(id);
            // var ayId =  _context.AcademicYear.Select(y => y.CloseDate).ToList();
            if (faculty == null)
            {
                return NotFound();
            }
            // ViewData["Ayid"] = new SelectList(_context.AcademicYears, "Ayid", "CloseDate", faculty.Ayid);
            return View(faculty);
        }

        // POST: Faculty/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("FacId,FacName")] Faculty faculty)
        {
            if (id != faculty.FacId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(faculty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FacultyExists(faculty.FacId))
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
            return View(faculty);
        }

        // GET: Faculty/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Faculties == null)
            {
                return NotFound();
            }

            var faculty = await _context.Faculties.FirstOrDefaultAsync(m => m.FacId == id);
            if (faculty == null)
            {
                return NotFound();
            }

            return View(faculty);
        }

        // POST: Faculty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var faculty = await _context
                .Faculties.Include(f => f.CustomUser)
                .ThenInclude(u => u.Contributions)
                .ThenInclude(c => c.Feedbacks)
                .FirstOrDefaultAsync(m => m.FacId == id);

            if (faculty == null)
            {
                return NotFound();
            }

            // Xóa tất cả các Feedback của các Contribution của tất cả các User thuộc Faculty
            foreach (var user in faculty.CustomUser)
            {
                foreach (var contribution in user.Contributions)
                {
                    _context.Feedbacks.RemoveRange(contribution.Feedbacks);
                }
            }

            // Xóa tất cả các Contribution của tất cả các User thuộc Faculty
            foreach (var user in faculty.CustomUser)
            {
                _context.Contributions.RemoveRange(user.Contributions);
            }

            // Xóa tất cả các User thuộc Faculty
            _context.Users.RemoveRange(faculty.CustomUser);

            // Xóa Faculty
            _context.Faculties.Remove(faculty);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool FacultyExists(int id)
        {
            return (_context.Faculties?.Any(e => e.FacId == id)).GetValueOrDefault();
        }
    }
}
