using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Comp1640.Models;
using Comp1640.Service;


var builder = WebApplication.CreateBuilder(args);
var connectionTestDbConnection = builder.Configuration.GetConnectionString("MyConnect");

builder.Services.AddDbContext<Comp1640.Models.Comp1640Context>(options =>
   options.UseSqlServer(connectionTestDbConnection));   

builder.Services.AddDefaultIdentity<CustomUser>(options => options.SignIn.RequireConfirmedAccount = true)
.AddRoles<IdentityRole>().AddEntityFrameworkStores<Comp1640Context>();



builder.Services.AddControllersWithViews();

var emailConfig = builder.Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();

#pragma warning disable CS8634 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'class' constraint.
builder.Services.AddSingleton(emailConfig);
#pragma warning restore CS8634 // The type cannot be used as type parameter in the generic type or method. Nullability of type argument doesn't match 'class' constraint.

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<Comp1640.Service.EmailSender>();
builder.Services.AddScoped<Comp1640.Models.ContributionFeedbackView>();

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
