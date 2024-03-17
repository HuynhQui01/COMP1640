using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Comp1640.Models;

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
        public async Task<IActionResult> Index()
        {
              return _context.AcademicYears != null ? 
                          View(await _context.AcademicYears.ToListAsync()) :
                          Problem("Entity set 'Comp1640Context.AcademicYear'  is null.");
        }

        // GET: AcademicYear/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.AcademicYears == null)
            {
                return NotFound();
            }

            var academicYear = await _context.AcademicYears
                .FirstOrDefaultAsync(m => m.Ayid == id);
            if (academicYear == null)
            {
                return NotFound();
            }

            return View(academicYear);
        }

        // GET: AcademicYear/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AcademicYear/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ayid,CloseDate,FinalCloseDate")] AcademicYear academicYear)
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
        public async Task<IActionResult> Edit(int id, [Bind("Ayid,CloseDate,FinalCloseDate")] AcademicYear academicYear)
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
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.AcademicYears == null)
            {
                return NotFound();
            }

            var academicYear = await _context.AcademicYears
                .FirstOrDefaultAsync(m => m.Ayid == id);
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
            if (_context.AcademicYears == null)
            {
                return Problem("Entity set 'Comp1640Context.AcademicYear'  is null.");
            }
            var academicYear = await _context.AcademicYears.FindAsync(id);
            if (academicYear != null)
            {
                _context.AcademicYears.Remove(academicYear);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AcademicYearExists(int id)
        {
          return (_context.AcademicYears?.Any(e => e.Ayid == id)).GetValueOrDefault();
        }
    }
}