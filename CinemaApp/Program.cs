using CinemaApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using CinemaApp.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CinemaAppContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("CinemaAppContext")));

// Add services to the container.
builder.Services.AddControllersWithViews();

// ?? ??????
builder.Services.AddSession();
var app = builder.Build();
app.UseSession();
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ?? ??????
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();