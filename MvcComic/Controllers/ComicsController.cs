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
        [Route("Comics/GetVolumeIssues")]
        public async Task<IActionResult> GetVolumeIssues(string volumeName)
        {
            if (string.IsNullOrEmpty(_apiKey))
            {
                ViewData["ErrorMessage"] = "API key is missing or invalid.";
                return View("VolumeIssues", new VolumeIssuesViewModel { VolumeTitle = volumeName });
            }

            if (string.IsNullOrEmpty(volumeName))
            {
                ViewData["ErrorMessage"] = "Volume name is required.";
                return View("VolumeIssues", new VolumeIssuesViewModel { VolumeTitle = volumeName });
            }

            string volumeUrl = $"https://comicvine.gamespot.com/api/volumes/?api_key={_apiKey}&format=json&filter=name:{volumeName}";

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("MvcComicApp/1.0");

                try
                {
                    // First API call to get the volume ID
                    HttpResponseMessage volumeResponse = await client.GetAsync(volumeUrl);
                    if (volumeResponse.IsSuccessStatusCode)
                    {
                        string volumeJsonResponse = await volumeResponse.Content.ReadAsStringAsync();
                        JObject volumeData = JObject.Parse(volumeJsonResponse);
                        var volume = volumeData["results"]?.FirstOrDefault();

                        if (volume == null)
                        {
                            ViewData["ErrorMessage"] = "No volume found for the specified name.";
                            return View("VolumeIssues", new VolumeIssuesViewModel { VolumeTitle = volumeName });
                        }

                        string volumeId = volume["id"]?.ToString() ?? string.Empty;
                        string volumeTitle = volume["name"]?.ToString() ?? string.Empty;

                        // Second API call to get the issues of the volume
                        string issuesUrl = $"https://comicvine.gamespot.com/api/issues/?api_key={_apiKey}&format=json&filter=volume:{volumeId}";
                        List<Comic> issues = new List<Comic>();
                        int offset = 0;
                        bool hasMoreResults = true;

                        while (hasMoreResults)
                        {
                            HttpResponseMessage issuesResponse = await client.GetAsync($"{issuesUrl}&offset={offset}");
                            if (issuesResponse.IsSuccessStatusCode)
                            {
                                string issuesJsonResponse = await issuesResponse.Content.ReadAsStringAsync();
                                JObject issuesData = JObject.Parse(issuesJsonResponse);
                                var results = issuesData["results"]?.ToList();

                                if (results == null || results.Count == 0)
                                {
                                    hasMoreResults = false;
                                }
                                else
                                {
                                    foreach (var result in results)
                                    {
                                        issues.Add(new Comic
                                        {
                                            Title = result["name"]?.ToString(),
                                            ImageUrl = result["image"]?["thumb_url"]?.ToString()
                                        });
                                    }
                                    offset += results.Count;
                                }
                            }
                            else
                            {
                                string errorContent = await issuesResponse.Content.ReadAsStringAsync();
                                ViewData["ErrorMessage"] = $"Error fetching data from API. Status Code: {issuesResponse.StatusCode}, Content: {errorContent}";
                                return View("VolumeIssues", new VolumeIssuesViewModel { VolumeTitle = volumeTitle });
                            }
                        }

                        var viewModel = new VolumeIssuesViewModel
                        {
                            VolumeTitle = volumeTitle,
                            Issues = issues
                        };

                        return View("VolumeIssues", viewModel);
                    }
                    else
                    {
                        string errorContent = await volumeResponse.Content.ReadAsStringAsync();
                        ViewData["ErrorMessage"] = $"Error fetching data from API. Status Code: {volumeResponse.StatusCode}, Content: {errorContent}";
                        return View("VolumeIssues", new VolumeIssuesViewModel { VolumeTitle = volumeName });
                    }
                }
                catch (Exception ex)
                {
                    ViewData["ErrorMessage"] = $"Exception occurred while fetching data from API: {ex.Message}";
                    return View("VolumeIssues", new VolumeIssuesViewModel { VolumeTitle = volumeName });
                }
            }
        }







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
                        string imageUrl = data["results"]?.FirstOrDefault()?["image"]?["thumb_url"]?.ToString() ?? string.Empty;

                        if (string.IsNullOrEmpty(imageUrl))
                        {
                            ViewData["ComicImageUrl"] = "No image found for the specified issue.";
                        }
                        else
                        {
                            ViewData["ComicImageUrl"] = imageUrl;
                        }

                        var comic = await _context.Comic.FirstOrDefaultAsync(m => m.Title == issueName);
                        if (comic != null)
                        {
                            comic.ImageUrl = imageUrl;
                            _context.Update(comic);
                            await _context.SaveChangesAsync();
                        }
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

            var comicDetails = await _context.Comic.FirstOrDefaultAsync(m => m.Title == issueName);
            if (comicDetails == null)
            {
                return NotFound();
            }

            return View("Details", comicDetails);
        }

        public IActionResult VolumeSearch()
        {
            var viewModel = new VolumeIssuesViewModel
            {
                VolumeTitle = string.Empty // or provide a default value
            };
            return View(viewModel);
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
        // POST: Comics/Edit/5
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
                    var existingComic = await _context.Comic.AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
                    if (existingComic == null)
                    {
                        return NotFound();
                    }

                    bool titleChanged = !string.Equals(existingComic.Title, comic.Title, StringComparison.OrdinalIgnoreCase);

                    _context.Update(comic);
                    await _context.SaveChangesAsync();

                    if (titleChanged)
                    {
                        return RedirectToAction(nameof(GetComicImage), new { issueName = comic.Title });
                    }
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
