using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Comp1640.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace Comp1640.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UserManager<CustomUser> _userManager;
    private readonly SignInManager<CustomUser> _signInManager;
    private readonly Comp1640Context _context;

    public HomeController(ILogger<HomeController> logger, UserManager<CustomUser> userManager, SignInManager<CustomUser> signInManager, Comp1640Context context)
    {
        _logger = logger;
        _userManager = userManager;
        _signInManager = signInManager;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Admin()
    {
        var users = await _userManager.Users.ToListAsync();

        Dictionary<string, int> loginFrequency = new Dictionary<string, int>();

        foreach (var user in users)
        {
            var userLogins = await _signInManager.UserManager.GetLoginsAsync(user);
            loginFrequency[user.UserName] = userLogins.Count;
        }

        return View(loginFrequency);
    }

    public IActionResult ManagerIndex()
    {
        return View();
    }
    public IActionResult Coordinator()
    {
        return View();
    }

    public async Task<IActionResult> CoordinatorChart()
    {
        IQueryable<dynamic> chartData;

        ViewBag.weeklyData = _context.Contributions
        .GroupBy(c => EF.Functions.DateDiffWeek(c.SubmitDate, DateTime.Now))
        .Select(g => new { Week = g.Key, Count = g.Count() })
        .ToList();
        ViewBag.monthlyData = _context.Contributions
        .GroupBy(c => new { c.SubmitDate.Year, c.SubmitDate.Month })
        .Select(g => new { Month = $"{g.Key.Year}-{g.Key.Month}", Count = g.Count() })
        .ToList();
        var totalContributions = _context.Contributions.Count();
        var approvedContributions = _context.Contributions.Count(c => c.Status == "Approved");
        var rejectedContributions = _context.Contributions.Count(c => c.Status == "Rejected");

        ViewBag.approvalData = new List<dynamic>
    {
        new { Label = "Approved", Count = approvedContributions },
        new { Label = "Rejected", Count = rejectedContributions }
    };
        return View();
    }

    public async Task<IActionResult> GuestChart()
    {
        var contributions = await _context.Contributions.ToListAsync();


        var approvedCount = contributions.Count(c => c.Status == "approved");
        var rejectedCount = contributions.Count(c => c.Status == "rejected");
        var pendingCount = contributions.Count(c => c.Status == "pending");

        var publishedCount = contributions.Count(c => c.Buplic == "publish");
        var unpublishedCount = contributions.Count(c => c.Buplic == "unpublish");

        var pieChartData = new Dictionary<string, int>
    {
        { "Approved", approvedCount },
        { "Rejected", rejectedCount },
        { "Pending", pendingCount }
    };

        var columnChartData = new Dictionary<string, int>
    {
        { "Published", publishedCount },
        { "Unpublished", unpublishedCount }
    };

        var pieChartLabels = pieChartData.Keys.ToArray();
        var pieChartValues = pieChartData.Values.ToArray();

        var columnChartLabels = columnChartData.Keys.ToArray();
        var columnChartValues = columnChartData.Values.ToArray();

        ViewBag.PieChartLabels = pieChartLabels;
        ViewBag.PieChartValues = pieChartValues;

        ViewBag.ColumnChartLabels = columnChartLabels;
        ViewBag.ColumnChartValues = columnChartValues;

        return View();
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
