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

    public async Task<IActionResult> Index()
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

    public IActionResult About()
    {
        return View();

    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public async Task<IActionResult> GuestChart()
    {
        var curUser = await _userManager.GetUserAsync(User);
        var facId = curUser.FacId;
        var faculty = await _context.Faculties.FindAsync(facId);
        var facName = faculty.FacName;


        var contributionsByFaculty = await _context.Contributions.Where(c => c.User.Faculty.FacName == facName)
       .GroupBy(c => c.User.Faculty.FacName)
       .Select(group => new { Faculty = group.Key, Count = group.Count() })
       .ToListAsync();

        List<string> facultyNames = new List<string>();
        List<int> contributionCounts = new List<int>();

        foreach (var item in contributionsByFaculty)
        {
            facultyNames.Add(item.Faculty);
            contributionCounts.Add(item.Count);
        }

        ViewBag.FacultyNames = facultyNames;
        ViewBag.ContributionCounts = contributionCounts;

        var contributionCount = await _context.Contributions
            .Where(c => c.Buplic == "Publicized").GroupBy(c => c.User.Faculty.FacName)
            .CountAsync();

        ViewBag.ContributionCountP = contributionCount;

        return View();
    }

    public async Task<IActionResult> GuestView()
    {
        var curUser = await _userManager.GetUserAsync(User);
        var facId = curUser.FacId;
        var faculty = await _context.Faculties.FindAsync(facId);
        var facName = faculty.FacName;

        // Lấy tất cả các Contribution của Faculty mà tài khoản Guest đang được xét
        var contributions = await _context.Contributions
            .Include(c => c.User)
            .Where(c => c.User.Faculty.FacName == facName)
            .ToListAsync();

        return View(contributions);
    }

}
