﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Comp1640.Models;

namespace Comp1640.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult Admin()
    {
        return View();
    }

    public IActionResult ManagerIndex()
    {
        return View();
    }
     public IActionResult Cordinator()
    {
        return View();
        // return Redirect("~/Home/Cordinator");    
    }
    public IActionResult About()
    {
        return View();
       
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    

}
