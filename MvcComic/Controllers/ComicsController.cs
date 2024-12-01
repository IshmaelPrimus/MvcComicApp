using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MvcComic.Data;
using MvcComic.Models;
using MvcComic.Services;

namespace MvcComic.Controllers
{
    public class ComicsController : Controller
    {
        private readonly ComicVineService _comicVineService;
        private readonly string _apiKey = "2194908e26505271c0a8b22937d61d9af0d9ac54"; // Store securely in configuration
        private readonly MvcComicContext _context;

        public ComicsController(MvcComicContext context, ComicVineService comicVineService)
        {
            _context = context;
            _comicVineService = comicVineService;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string Title, int Issue)
        {
            if (string.IsNullOrEmpty(Title))
            {
                ModelState.AddModelError("", "Comic name is required.");
                return View();
            }

            try
            {
                var imageUrl = await _comicVineService.GetIssueImageAsync(Title, Issue);
                var newComic = new Comic
                {
                    Title = Title,
                    Issue = Issue,
                    ImageUrl = imageUrl,
                    // Set other properties as needed, e.g., ReleaseDate, Publisher, etc.
                };

                _context.Comic.Add(newComic);
                await _context.SaveChangesAsync();

                ViewBag.ImageUrl = imageUrl;
                ViewBag.ComicName = Title;
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Failed to fetch comic image.");
            }

            return View();
        }


        // GET: Comics
        public async Task<IActionResult> Index(string comicPublisher, string searchString)
        {
            if (_context.Comic == null)
            {
                return Problem("Entity set 'MvcComicContext.Comic' is null.");
            }

            // Use LINQ to get list of Publishers.
            IQueryable<string> publisherQuery = from m in _context.Comic
                                                orderby m.Publisher
                                                select m.Publisher;
            var comics = from m in _context.Comic
                         select m;

            if (!string.IsNullOrEmpty(searchString))
            {
                comics = comics.Where(s => s.Title!.ToUpper().Contains(searchString.ToUpper()));
            }

            if (!string.IsNullOrEmpty(comicPublisher))
            {
                comics = comics.Where(x => x.Publisher == comicPublisher);
            }

            var comicList = await comics.ToListAsync();

            // Fetch image URL for each comic
            foreach (var comic in comicList)
            {
                if (!string.IsNullOrEmpty(comic.Title))
                {
                    comic.ImageUrl = await _comicVineService.GetIssueImageAsync(comic.Title, comic.Issue ?? 0);
                }
            }

            var comicPublisherVM = new ComicPublisherViewModel
            {
                Publisher = new SelectList(await publisherQuery.Distinct().ToListAsync()),
                Comics = comicList
            };

            return View(comicPublisherVM);
        }

        // GET: Comic/Details/5
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

        // GET: Comic/Create
        //public IActionResult Create()
        //{
        //    return View();
        //}

        // POST: Comic/Create
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Issue,Publisher,Price,Grading,ImageUrl")] Comic comic)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        // Fetch image URL for the new comic
        //        if (!string.IsNullOrEmpty(comic.Title))
        //        {
        //            comic.ImageUrl = await _comicVineService.GetIssueImageAsync(comic.Title, comic.Issue ?? 0);
        //        }

        //        _context.Add(comic);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View(comic);
        //}

        // GET: Comic/Edit/5
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

        // POST: Comic/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,ReleaseDate,Issue,Publisher,Price,Grading,ImageUrl")] Comic comic)
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

        // GET: Comic/Delete/5
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

        // POST: Comic/Delete/5
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

        // Call the GetIssueImageAsync method and pass the result to a view.
        public async Task<IActionResult> GetIssueImage(string issueName, int issueNumber)
        {
            var imageUrl = await _comicVineService.GetIssueImageAsync(issueName, issueNumber);
            return Json(new { imageUrl });
        }
    }
}
