using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Comp1640.Models;

namespace Comp1640.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<CustomUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly Comp1640Context _context;

        public UserController(UserManager<CustomUser> userManager, RoleManager<IdentityRole> roleManager, Comp1640Context context)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            // if (User.Identity.IsAuthenticated)
            // {
            //     if (User.IsInRole("Student"))
            //     {
            var users = _userManager.Users.ToList();
            return View(users);
            //     }
            // }
            // return Redirect("Identity/Account/Login");
        }

        public async Task<IActionResult> Delete(string id)
        {
            // if (User.Identity.IsAuthenticated)
            // {
            //     if (User.IsInRole("Admin"))
            //     {
            var user = _userManager.FindByIdAsync(id).Result;
            if (user != null)
            {
                var result = _userManager.DeleteAsync(user).Result;
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return View("Error");
            //     }
            // }
            // return Redirect("/");
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
        public async Task<IActionResult> RemoveRoles(string userId, string selectedRole)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return View("Error");
            }

            if (!string.IsNullOrEmpty(selectedRole))
            {
                var result = await _userManager.RemoveFromRoleAsync(user, selectedRole);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error");
                }
            }

            return RedirectToAction("Index");
        }


        [HttpPost]
        public IActionResult SetRoles(string id, List<string> Roles, List<string> Faculties )
        {
            // if (User.Identity.IsAuthenticated)
            // {
            //     if (User.IsInRole("Admin"))
            //     {
            var user = _userManager.FindByIdAsync(id).Result;
            var faculty = Faculties[0];
            user.FacName = faculty;
            var result = _userManager.AddToRolesAsync(user, Roles).Result;
            if (result.Succeeded)
            {
                return RedirectToAction("Index");
            }
            else
                return View("Error");
            //         }
            //     }
            //     return Redirect("/");
        }

        [HttpPost]
        public IActionResult SetFaculty(string id, List<string> Faculties)
        {
            var user = _userManager.FindByIdAsync(id).Result;
            var faculty = Faculties[0];
            user.FacName = faculty;
            
            return RedirectToAction("Index");
        }
    }
}

