using InvoiceSystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

var builder = WebApplication.CreateBuilder(args);

// DATABASE CONFIGURATION
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));   // Use SqlServer (recommended)

// Enable DB migration error page during development
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// IDENTITY CONFIGURATION
builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false; // You can set to true if needed
})
.AddEntityFrameworkStores<ApplicationDbContext>();

// MVC + RAZOR
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Middleware
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Identity Middleware
app.UseAuthentication();
app.UseAuthorization();

// MVC routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Invoices}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();
