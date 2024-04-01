using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Comp1640.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> ViewChart()
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

            return View();
        }
    }
}