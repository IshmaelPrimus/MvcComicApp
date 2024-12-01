using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcComic.Data;
using MvcComic.Models;
using MvcComic.Services; // Add this line to include the ComicVineService

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<MvcComicContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("MvcComicContext") ?? throw new InvalidOperationException("Connection string 'MvcComicContext' not found.")));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient<ComicVineService>(client =>
{
    client.BaseAddress = new Uri("https://comicvine.gamespot.com/api/");
});
builder.Services.AddScoped(sp => new ComicVineService(sp.GetRequiredService<HttpClient>(), "2194908e26505271c0a8b22937d61d9af0d9ac54")); // Replace with your actual API key
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    SeedData.Initialize(services);
}

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

app.Run();
