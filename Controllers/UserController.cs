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
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            // if (User.Identity.IsAuthenticated)
            // {
            //     if (User.IsInRole("Admin"))
            //     {
                    var users = _userManager.Users.ToList();
                    
                    return View(users);
            //     }
            // }
            // return Redirect("/");
        }

        public IActionResult Delete(string id)
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

        public IActionResult ManageRoles(string id)
        {
            // if (User.Identity.IsAuthenticated)
            // {
            //     if (User.IsInRole("Admin"))
            //     {
                    var user = _userManager.FindByIdAsync(id).Result;
                    var roles = _roleManager.Roles.Select(r => r.Name).ToList();
                    return View(model: new UserRoleViewModel
                    {
                        User = user,
                        Roles = roles
                    });
            //     }
            // }
            // return Redirect("/");
        }

        [HttpPost]
        public IActionResult SetRoles(string id, List<string> Roles)
        {
            // if (User.Identity.IsAuthenticated)
            // {
            //     if (User.IsInRole("Admin"))
            //     {
                    var user = _userManager.FindByIdAsync(id).Result;

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
    }
}

