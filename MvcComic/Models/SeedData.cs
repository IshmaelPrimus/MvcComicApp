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
                    Title = "When Harry Met Sally",
                    ReleaseDate = DateTime.Parse("1989-2-12"),
                    Issue = 1,
                    Genre = "Romantic Comedy",
                    Grading = 9,
                    Price = 7.99M
                },
                new Comic
                {
                    Title = "Ghostbusters ",
                    ReleaseDate = DateTime.Parse("1984-3-13"),
                    Genre = "Comedy",
                    Issue = 2,
                    Grading = 8,
                    Price = 8.99M
                },
                new Comic
                {
                    Title = "Ghostbusters 2",
                    ReleaseDate = DateTime.Parse("1986-2-23"),
                    Genre = "Comedy",
                    Issue = 3,
                    Grading = 7,
                    Price = 9.99M
                },
                new Comic
                {
                    Title = "Rio Bravo",
                    ReleaseDate = DateTime.Parse("1959-4-15"),
                    Genre = "Western",
                    Issue = 4,
                    Grading = 6,
                    Price = 3.99M
                }
            );
            context.SaveChanges();
        }
    }
}