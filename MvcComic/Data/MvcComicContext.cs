using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcComic.Models;

namespace MvcComic.Data
{
    public class MvcComicContext : DbContext
    {
        public MvcComicContext (DbContextOptions<MvcComicContext> options)
            : base(options)
        {
        }

        public DbSet<MvcComic.Models.Comic> Comic { get; set; } = default!;
    }
}
