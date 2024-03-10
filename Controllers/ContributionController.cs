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
    public class ContributionController : Controller
    {
        private readonly Comp1640Context _context;

        public ContributionController(Comp1640Context context)
        {
            _context = context;
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
            return View();
        }

        // POST: Contribution/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConId,ConName,UserId,Startus,File")] Contribution contribution)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contribution);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(contribution);
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
