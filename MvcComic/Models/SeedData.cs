using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MvcComic.Data;
using NuGet.Packaging.Signing;
using System;
using System.Linq;

namespace MvcComic.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new MvcComicContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<MvcComicContext>>()))
        {
            // Look for any movies.
            if (context.Comic.Any())
            {
                return;   // DB has been seeded
            }
            context.Comic.AddRange(
                new Comic
                {
                    Title = "Icon",
                    ReleaseDate = DateTime.Parse("1989-2-12"),
                    Issue = 1,
                    Publisher = "DC",
                    Grading = 9.0M,
                    Price = 7.99M,
                    ImageUrl = "https://example.com/icon.jpg"
                },
                new Comic
                {
                    Title = "Hardware ",
                    ReleaseDate = DateTime.Parse("1984-3-13"),
                    Publisher = "DC",
                    Issue = 2,
                    Grading = 8.0M,
                    Price = 8.99M,
                    ImageUrl = "https://example.com/icon.jpg"
                },
                new Comic
                {
                    Title = "Icon",
                    ReleaseDate = DateTime.Parse("1986-2-23"),
                    Publisher = "Comedy",
                    Issue = 3,
                    Grading = 7.0M,
                    Price = 9.99M,
                    ImageUrl = "https://example.com/icon.jpg"
                },
                new Comic
                {
                    Title = "Static",
                    ReleaseDate = DateTime.Parse("1959-4-15"),
                    Publisher = "DC",
                    Issue = 4,
                    Grading = 6.9M,
                    Price = 3.99M,
                    ImageUrl = "https://example.com/icon.jpg"
                }
            );
            context.SaveChanges();
        }
    }
}