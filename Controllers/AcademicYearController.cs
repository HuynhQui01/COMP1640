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
    public class AcademicYearController : Controller
    {
        private readonly Comp1640Context _context;

        public AcademicYearController(Comp1640Context context)
        {
            _context = context;
        }

        // GET: AcademicYear
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            return _context.AcademicYears != null
                ? View(await _context.AcademicYears.ToListAsync())
                : Problem("Entity set 'Comp1640Context.AcademicYear'  is null.");
        }

        public async Task<IActionResult> ViewA()
        {
            return _context.AcademicYears != null
                ? View(await _context.AcademicYears.ToListAsync())
                : Problem("Entity set 'Comp1640Context.AcademicYear'  is null.");
        }

        // GET: AcademicYear/Details/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AcademicYears == null)
            {
                return NotFound();
            }

            var academicYear = await _context.AcademicYears.FirstOrDefaultAsync(m => m.Ayid == id);
            if (academicYear == null)
            {
                return NotFound();
            }

            return View(academicYear);
        }

        // GET: AcademicYear/Create
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: AcademicYear/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            [Bind("Ayid,CloseDate,Name,FinalCloseDate")] AcademicYear academicYear
        )
        {
            if (ModelState.IsValid)
            {
                var count = await _context.AcademicYears.CountAsync();
                // academicYear.Ayid = count;
                _context.Add(academicYear);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(academicYear);
        }

        // GET: AcademicYear/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.AcademicYears == null)
            {
                return NotFound();
            }

            var academicYear = await _context.AcademicYears.FindAsync(id);
            if (academicYear == null)
            {
                return NotFound();
            }
            return View(academicYear);
        }

        // POST: AcademicYear/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            [Bind("Ayid,CloseDate,FinalCloseDate,Name")] AcademicYear academicYear
        )
        {
            if (id != academicYear.Ayid)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(academicYear);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AcademicYearExists(academicYear.Ayid))
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
            return View(academicYear);
        }

        // GET: AcademicYear/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AcademicYears == null)
            {
                return NotFound();
            }

            var academicYear = await _context.AcademicYears.FirstOrDefaultAsync(m => m.Ayid == id);
            if (academicYear == null)
            {
                return NotFound();
            }

            return View(academicYear);
        }

        // POST: AcademicYear/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var academicYear = await _context
                .AcademicYears.Include(ay => ay.Contributions)
                .ThenInclude(c => c.Feedbacks)
                .FirstOrDefaultAsync(m => m.Ayid == id);

            if (academicYear == null)
            {
                return NotFound();
            }

            foreach (var contribution in academicYear.Contributions)
            {
                _context.Feedbacks.RemoveRange(contribution.Feedbacks);
            }

            _context.Contributions.RemoveRange(academicYear.Contributions);

            _context.AcademicYears.Remove(academicYear);

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool AcademicYearExists(int id)
        {
            return (_context.AcademicYears?.Any(e => e.Ayid == id)).GetValueOrDefault();
        }
    }
}
