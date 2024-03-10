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
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        // GET: Roles
        public IActionResult Index()
        {
            // if (User.Identity.IsAuthenticated)
            // {
                // if (User.IsInRole("Admin"))
                // {
                    var roles = _roleManager.Roles.ToList();
                    return View(roles);
                // }
            // }
            return Redirect("/");
        }

        // GET: Roles/Create
        public ActionResult Create()
        {
            // if (User.Identity.IsAuthenticated)
            // {
                // if (User.IsInRole("Admin"))
                // {
                    return View();
                // }
            // }
            // return Redirect("/");
        }

        // POST: Roles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRoleViewModel model)
        {
            // if (User.Identity.IsAuthenticated)
            // {
                // if (User.IsInRole("Admin"))
                // {
                    if (!string.IsNullOrEmpty(model.roleName))
                    {
                        var role = new IdentityRole(model.roleName);
                        var result = await _roleManager.CreateAsync(role);

                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }

                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError(string.Empty, error.Description);
                        }
                    }

                    return View();
                // }
            // }
            // return Redirect("/");
        }

        public async Task<IActionResult> Edit(string id)
        {
            // if (User.Identity.IsAuthenticated)
            // {
            //     if (User.IsInRole("Admin"))
            //     {
                    var role = await _roleManager.FindByIdAsync(id);
                    if (role != null)
                    {
                        return View(role);
                    }

                    return View("Error");
                // }
            // }
            // return Redirect("/");
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, string Name)
        {
            // if (User.Identity.IsAuthenticated)
            // {
                // if (User.IsInRole("Admin"))
                // {
                    var role = await _roleManager.FindByIdAsync(id);
                    if (role != null)
                    {
                        role.Name = Name;
                        var result = await _roleManager.UpdateAsync(role);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                    }

                    // return View("Error");
                // }
            // }
            return Redirect("/");
        }

        public async Task<IActionResult> Delete(string id)
        {
        //     if (User.Identity.IsAuthenticated)
        //     {
        //         if (User.IsInRole("Admin"))
        //         {
                    var role = await _roleManager.FindByIdAsync(id);
                    if (role != null)
                    {
                        var result = await _roleManager.DeleteAsync(role);
                        if (result.Succeeded)
                        {
                            return RedirectToAction("Index");
                        }
                    }

            //         return View("Error");
            //     }
            // }
            return Redirect("/");
        }
    }
}