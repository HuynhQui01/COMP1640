using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Comp1640.Models;
var builder = WebApplication.CreateBuilder(args);
var connectionTestDbConnection = builder.Configuration.GetConnectionString("MyConnect");

builder.Services.AddDbContext<Comp1640.Models.Comp1640Context>(options =>
   options.UseSqlServer(connectionTestDbConnection));

builder.Services.AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = true)
.AddRoles<IdentityRole>().AddEntityFrameworkStores<Comp1640Context>();
// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();
app.Run();
