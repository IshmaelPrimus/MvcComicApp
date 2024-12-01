using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcComic.Data;
using MvcComic.Models;
using Newtonsoft.Json.Linq;

namespace MvcComic.Controllers
{
    public class ComicsController : Controller
    {
        private readonly MvcComicContext _context;

        public ComicsController(MvcComicContext context)
        {
            _context = context;
        }

        private readonly string _apiKey = "2194908e26505271c0a8b22937d61d9af0d9ac54";

        [HttpGet]
        [Route("Comics/GetComicImage")]
        public async Task<IActionResult> GetComicImage(string issueName)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                ViewData["ComicImageUrl"] = "API key is missing or invalid.";
                return View("Details", new Comic { Title = issueName });
            }

            string url = $"https://comicvine.gamespot.com/api/issues/?api_key={_apiKey}&format=json&sort=name:asc&resources=issue&query=\"{issueName}\"&filter=name:{issueName}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("MvcComicApp/1.0");

                try
                {
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        JObject data = JObject.Parse(jsonResponse);
                        string imageUrl = data["results"]?.FirstOrDefault()?["image"]?["original_url"]?.ToString() ?? string.Empty;
                        ViewData["ComicImageUrl"] = imageUrl;
                    }
                    else
                    {
                        string errorContent = await response.Content.ReadAsStringAsync();
                        ViewData["ComicImageUrl"] = $"Error fetching data from API. Status Code: {response.StatusCode}, Content: {errorContent}";
                    }
                }
                catch (Exception ex)
                {
                    ViewData["ComicImageUrl"] = $"Exception occurred while fetching data from API: {ex.Message}";
                }
            }

            var comic = await _context.Comic.FirstOrDefaultAsync(m => m.Title == issueName);
            if (comic == null)
            {
                return NotFound();
            }

            return View("Details", comic);
        }







        // GET: Comics
        public async Task<IActionResult> Index()
        {
            return View(await _context.Comic.ToListAsync());
        }

        // GET: Comics/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comic = await _context.Comic
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comic == null)
            {
                return NotFound();
            }

            return View(comic);
        }

        // GET: Comics/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Comics/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Issue,Genre,Price")] Comic comic)
        {
            if (ModelState.IsValid)
            {
                _context.Add(comic);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(GetComicImage), new { issueName = comic.Title });
            }
            return View(comic);
        }

        // GET: Comics/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comic = await _context.Comic.FindAsync(id);
            if (comic == null)
            {
                return NotFound();
            }
            return View(comic);
        }

        // POST: Comics/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Issue,Genre,Price")] Comic comic)
        {
            if (id != comic.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(comic);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ComicExists(comic.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(comic);
        }

        // GET: Comics/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var comic = await _context.Comic
                .FirstOrDefaultAsync(m => m.Id == id);
            if (comic == null)
            {
                return NotFound();
            }

            return View(comic);
        }

        // POST: Comics/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comic = await _context.Comic.FindAsync(id);
            if (comic != null)
            {
                _context.Comic.Remove(comic);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ComicExists(int id)
        {
            return _context.Comic.Any(e => e.Id == id);
        }
    }
}
