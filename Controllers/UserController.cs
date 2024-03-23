using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Comp1640.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;
using COMP1640.Service;


namespace Comp1640.Controllers
{
    public class UserController : Controller
    {
        private readonly Comp1640Context _context;
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserStore<CustomUser> _userStore;
        private readonly IUserEmailStore<CustomUser> _emailStore;
        private readonly ILogger<UserController> _logger;
        private readonly EmailSender _emailSender;

        public UserController
        (
            UserManager<CustomUser> userManager,
            RoleManager<IdentityRole> roleManager,
            Comp1640Context context,
            EmailSender emailSender,
            IUserStore<CustomUser> userStore,
            ILogger<UserController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _emailSender = emailSender;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            // if (User.Identity.IsAuthenticated)
            // {
            //     if (User.IsInRole("Student"))
            //     {
            var users = await _userManager.Users.ToListAsync();
            return View(users);
            //     }
            // }
            // return Redirect("Identity/Account/Login");
        }

        public async Task<IActionResult> Delete(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            try
            {
                _context.Database.ExecuteSqlRaw("ALTER TABLE dbo.Contributions NOCHECK CONSTRAINT FK__Contribut__UserI__571DF1D5");

                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                _context.Database.ExecuteSqlRaw("ALTER TABLE dbo.Contributions WITH CHECK CHECK CONSTRAINT FK__Contribut__UserI__571DF1D5");

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                // Handle exception or display error message
                // return RedirectToAction(nameof(Error));
            }
            return View("Index");
        }

        public IActionResult ManageRoles(string id)
        {
            // if (User.Identity.IsAuthenticated)
            // {
            //     if (User.IsInRole("Admin"))
            //     {
            var user = _userManager.FindByIdAsync(id).Result;
            var roles = _roleManager.Roles.Select(r => r.Name).ToList();
            var fac = _context.Faculties.Select(f => f.FacName).ToList();
            return View(model: new UserRoleViewModel
            {
                User = user,
                Roles = roles,
                Faculties = fac

            });
            //     }
            // }
            // return Redirect("/");
        }
        public async Task<IActionResult> RemoveRoles(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            var roles = await _userManager.GetRolesAsync(user);

            var userRoleViewModel = new UserRoleViewModel
            {
                User = user,
                Roles = roles.ToList()
            };

            return View(userRoleViewModel);
        }


        [HttpPost]
        public IActionResult SetRoles(string id, List<string> Roles)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            var userRoles = _userManager.GetRolesAsync(user).Result;

            // Xóa tất cả các vai trò hiện có của người dùng
            var removeResult = _userManager.RemoveFromRolesAsync(user, userRoles).Result;
            if (!removeResult.Succeeded)
            {
                // Xử lý lỗi nếu cần thiết
                return View("Error");
            }

            // Thêm các vai trò mới cho người dùng
            var addResult = _userManager.AddToRolesAsync(user, Roles).Result;
            if (addResult.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
            {
                // Xử lý lỗi nếu cần thiết
                return View("Error");
            }
        }

        public IActionResult ManageFaculty(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            var fac = _context.Faculties.Select(f => f.FacName).ToList();
            return View(model: new UserRoleViewModel
            {
                User = user,
                Faculties = fac
            });

        }

        [HttpPost]
        public async Task<IActionResult> SetFaculty(string id, List<string> Faculties)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            var faculty = Faculties[0];
            user.FacName = faculty;
            await _userManager.UpdateAsync(user);
            return RedirectToAction("Index");
        }

        public IActionResult TestEmail()
        {
            var message = new Message(new string[] { "quihvgcc210153@fpt.edu.vn" }, "Test async email", "This is the content from our async email.");
            _emailSender.SendEmail(message);
            return RedirectToAction("Index");
        }

        public IActionResult CreateUser()
        {
            ViewBag.Roles = _roleManager.Roles.ToList();
            var faculties = _context.Faculties.ToList();
            if (faculties != null && faculties.Any())
            {
                ViewData["FacId"] = new SelectList(_context
                    .Faculties, "FacName", "FacName");
                ViewData["role"] = new SelectList(_roleManager.Roles, "Id", "Name");
            }
            else
            {
                ViewBag.FacName = new SelectList(new List<Faculty>(), "FacName", "Name");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateUser(CustomUser model, string password, string role)
        {
            // if (ModelState.IsValid)
            // {
            var user = new CustomUser
            {
                UserName = model.UserName,
                // Email = model.Email,
                FacName = model.FacName,
                ProfileImagePath = model.ProfileImagePath

            };

            await _userStore.SetUserNameAsync(user, model.Email, CancellationToken.None);
            await _emailStore.SetEmailAsync(user, model.Email, CancellationToken.None);
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password." + password);

                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
                var resultEmail = await _userManager.ConfirmEmailAsync(user, code);

                var selectedRole = await _roleManager.FindByIdAsync(role);

                if (selectedRole != null)
                {
                    // Assign the user to the selected role
                    await _userManager.AddToRoleAsync(user, selectedRole.Name);


                }
                string body = "Title: Account Created\n" +
                "Dear sir/madam, \n" +
                "Your new account on aaaaaa system has been created.\n" +
                "- User Name:" + user.Email + "\n" +
                "- Email:" + user.Email + "\n" +
                "- Password: " + password + "\n\n" +
                "Sincerely, \n" +
                "Developer team";

                List<string> emails = new List<string>();
                emails.Add(user.Email);

                var message = new Message(emails, "Account Created", body);
                await _emailSender.SendEmailAsync(message);

                return RedirectToAction("Index");
            }
            else
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
                // ModelState.AddModelError(string.Empty, "Failed to create user. Please check the provided information and try again.");
            }
            // }
            ViewBag.Roles = _roleManager.Roles.ToList();
            var faculties = _context.Faculties.ToList();
            if (faculties != null && faculties.Any())
            {
                ViewData["FacId"] = new SelectList(_context
                    .Faculties, "FacId", "FacName");
                ViewData["role"] = new SelectList(_roleManager.Roles, "Id", "Name");
            }
            else
            {
                ViewBag.FacultyId = new SelectList(new List<Faculty>(), "FacultyId", "Name");
            }

            return View("CreateUser", model);
        }

        private IUserEmailStore<CustomUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<CustomUser>)_userStore;
        }

    }
}

