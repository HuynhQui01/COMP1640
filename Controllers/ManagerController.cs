using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comp1640.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Comp1640.Controllers
{
    public class ManagerController : Controller
    {
        private readonly Comp1640Context _context;
        private readonly UserManager<CustomUser> _userManager;

        public ManagerController(Comp1640Context context, UserManager<CustomUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Manager
        public async Task<ActionResult> Index()
        {
            var contributions = _context.Contributions.Include(c => c.User)
            // .Include(c => c.Fac)
            .ToList();
            // return _context.Contributions != null ?
            //               View(await _context.Contributions.ToListAsync()) :
            //               Problem("Entity set 'Comp1640Context.Contributions'  is null.");
            return View(contributions);
        }

        // GET: Manager/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Manager/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Manager/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Manager/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Manager/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<ActionResult> studentmanage()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        // GET: Manager/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }


        // POST: Manager/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> ViewChart(int academicYearId)
        {
            

            var contributions = await _context.Contributions.ToListAsync();
            var publishedCount = contributions.Count(c => c.Buplic == "Publicized");
            var unpublishedCount = contributions.Count(c => c.Buplic == "Non-publicized");

            var columnChartData = new Dictionary<string, int>
            {
                { "Publicized", publishedCount },
                { "Non-publicized", unpublishedCount }
            };

            var columnChartLabels = columnChartData.Keys.ToArray();
            var columnChartValues = columnChartData.Values.ToArray();

            ViewBag.ColumnChartLabels = columnChartLabels;
            ViewBag.ColumnChartValues = columnChartValues;

            var contributionCounts = _context.Contributions
            .GroupBy(c => c.User.Faculty.FacName)
            .Select(g => new { FacultyName = g.Key, ContributionCount = g.Count() })
            .ToDictionary(x => x.FacultyName, x => x.ContributionCount);

            ViewBag.Con = contributionCounts;

            var contributionCountsAY = _context.Contributions
            .Include(c => c.Ay)
            .Include(c => c.User)
            .GroupBy(c => new { c.Ay.Name, c.User.Faculty.FacName })
            .Select(g => new
            {
                AcademicYear = g.Key.Name,
                FacultyName = g.Key.FacName,
                ContributionCount = g.Count()
            })
            .ToList();

            var result = new Dictionary<string, Dictionary<string, int>>();

            foreach (var item in contributionCountsAY)
            {
                if (!result.ContainsKey(item.AcademicYear))
                {
                    result[item.AcademicYear] = new Dictionary<string, int>();
                }

                result[item.AcademicYear][item.FacultyName] = item.ContributionCount;
            }

            ViewBag.ConAy = result;

            if (academicYearId == 0)
            {
                academicYearId = 1;
            }
            var totalContributions = _context.Contributions
                .Count(c => c.Ayid == academicYearId);

            var contributionCountsByFaculty = _context.Contributions
                .Where(c => c.Ayid == academicYearId)
                .GroupBy(c => c.User.Faculty.FacName)
                .Select(g => new
                {
                    FacultyName = g.Key,
                    ContributionCount = g.Count()
                })
                .ToDictionary(x => x.FacultyName, x => (double)x.ContributionCount / totalContributions * 100);

            ViewBag.ConFac = contributionCountsByFaculty;
            ViewBag.AcademicYear = new SelectList(_context.AcademicYears, "Ayid", "Name");
            
    

            return View();
        }
        public async Task<IActionResult> ExceptionReports(){
            ViewBag.ConNoComment = _context.Contributions.Count(c => string.IsNullOrEmpty(c.Feedbacks.FirstOrDefault().Comment));

            DateTime cutoffDate = DateTime.Today.AddDays(-14);

        // Query contributions submitted after the cutoff date and without feedback
        var contributionsWithoutCommentAfter14DaysCount = _context.Contributions
            .Count(c => c.SubmitDate <= cutoffDate && !c.Feedbacks.Any());

            ViewBag.ConNoComment14 = contributionsWithoutCommentAfter14DaysCount;

        // return View(contributionsWithoutCommentAfter14DaysCount);
            return View();
        }
    }
}