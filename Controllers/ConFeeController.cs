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
using Comp1640.Service;


namespace Comp1640.Controllers
{
    public class ConFeeController : Controller
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly ContributionFeedbackView _conFeeView;
        private readonly Comp1640Context _context;
        private readonly EmailSender _emailSender;


        public ConFeeController(ContributionFeedbackView confeeView, Comp1640Context context, UserManager<CustomUser> userManager, EmailSender emailSender)
        {
            _conFeeView = confeeView;
            _context = context;
            _userManager = userManager;
            _emailSender = emailSender;
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
        public async Task<IActionResult> Create([Bind("FeedbackId, Comment, Fbtime, UserId, ConId")] Feedback feedback, int id)
        {
            if (User.IsInRole("Student"))
            {
                var userId = _userManager.GetUserId(User);

                feedback.Fbtime = DateTime.Now;
                feedback.ConId = id;
                feedback.UserId = userId;
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                string content = "Your feedback has been reply at contribution ";
                var message = new Message(new string[] { "quihvgcc210153@fpt.edu.vn" }, "Contribution feedback", content);
                _emailSender.SendEmail(message);
                return RedirectToAction("Details", new { id = id });


            }
            if (User.IsInRole("Coordinator"))
            {
                var userId = _userManager.GetUserId(User);
                var userName = _userManager.GetUserName(User);
                var user = _userManager.FindByIdAsync(feedback.UserId);

                List<string> email = new List<string>();
                email.Add(userName + "@gmail.com");
                feedback.ConId = id;
                feedback.UserId = _userManager.GetUserId(User);
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                string content = "Your contribution has been feedbacked at contribution ";
                var message = new Message(email, "Contribution feedback", content);
                return RedirectToAction("Details", new { id = id });

            }
            return View("/Contribution/Index");

        }

        public async Task<IActionResult> Details(int id)
        {
            ViewBag.feedback = _context.Feedbacks.Include(f => f.User).Where(f => f.ConId == id);

            var fee = await _context.Feedbacks.Where(f => f.ConId == id).Include(c => c.Con).ToListAsync();
            var con = await _context.Contributions.FindAsync(id);
            return View(model: new ContributionFeedbackView
            {
                contribution = con,
                feedbacks = fee
            });
        }
    }
}