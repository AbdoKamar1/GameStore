using gamestore.dataacess.Data;
using gamestore.dataacess.Repository;
using gamestore.dataacess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNetCore.Identity;
using NuGet.Protocol.Core.Types;
using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Microsoft.AspNetCore.Identity.UI.Services;
using gamestore.utilty;
using Stripe;
using Microsoft.CodeAnalysis.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ApplicatioonDbContext>(options => options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection")
    ));
builder.Services.Configure<StripeSetting>(builder.Configuration.GetSection("Stripe"));

builder.Services.AddIdentity<IdentityUser,IdentityRole>().AddDefaultTokenProviders()
    .AddEntityFrameworkStores<ApplicatioonDbContext>();
builder.Services.AddScoped<IUnitOfWork,UnitOfWork>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddRazorPages();
builder.Services.ConfigureApplicationCookie(option =>
{
    option.LoginPath = $"/Identity/Account/Login";
    option.LogoutPath = $"/Identity/Account/Logout";
    option.AccessDeniedPath = $"/Identity/Account/AcessDenied";
}
);
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(100);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
}
);



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
StripeConfiguration.ApiKey = builder.Configuration.GetSection("Stripe:SecretKey").Get<string>();

app.UseAuthentication();;

app.UseAuthorization();
app.UseSession();
app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{area=Customer}/{controller=Home}/{action=Index}/{id?}");

app.Run();
