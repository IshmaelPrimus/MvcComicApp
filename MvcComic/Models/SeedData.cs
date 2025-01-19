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
                    Volume = "A",
                    Title = "Test",
                    IssueNumber = "1",
                    ImageUrl = "https://comicvine.gamespot.com/a/uploads/scale_avatar/0/3125/7709750-bat158.jpg"
                },
                new Comic
                {
                    Volume = "A",
                    Title = "Test",
                    IssueNumber = "2",
                    ImageUrl = "https://comicvine.gamespot.com/a/uploads/scale_avatar/0/3125/7709750-bat158.jpg"
                },
                new Comic
                {
                    Volume = "A",
                    Title = "Test",
                    IssueNumber = "3",
                    ImageUrl = "https://comicvine.gamespot.com/a/uploads/scale_avatar/0/3125/7709750-bat158.jpg"
                },
                new Comic
                {
                    Volume = "A",
                    Title = "Test",
                    IssueNumber = "4",
                    ImageUrl = "https://comicvine.gamespot.com/a/uploads/scale_avatar/0/3125/7709750-bat158.jpg"
                }
            );
            context.SaveChanges();
        }
    }
}