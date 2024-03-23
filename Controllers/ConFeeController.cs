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

namespace COMP1640.Controllers
{
    public class ConFeeController : Controller
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly ContributionFeedbackView _conFeeView;
        private readonly Comp1640Context _context;

        public ConFeeController(ContributionFeedbackView confeeView, Comp1640Context context, UserManager<CustomUser> userManager)
        {
            _conFeeView = confeeView;
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create(int id)
        {
            var con = await _context.Contributions.FindAsync(id);
            return View(model: new ContributionFeedbackView
            {
                contribution = con,
                feedback = null
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FeedbackId, Comment, Fbtime, UserId, ConId")] Feedback feedback, int id )
        {
            var datetime = DateTime.Now;
            feedback.Fbtime = datetime;
            feedback.ConId = id;
            feedback.UserId = _userManager.GetUserId(User);
            _context.Add(feedback);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Feedback");
        }

        public async Task<IActionResult> View(int id){
            var fee = await _context.Feedbacks.Where(f => f.ConId == id).Include(c => c.Con).ToListAsync();
            // ViewData["ConName"] = new SelectList(_context.Contributions, "Id" )
            var con = _context.Contributions.Find(id);
            // return View(fee);
            return View(model: new ContributionFeedbackView
            {
                contribution = con,
                feedbacks = fee
            });
        }

        



    }
}