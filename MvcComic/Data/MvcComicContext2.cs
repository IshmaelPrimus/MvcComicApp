using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MvcComic.Models;

namespace MvcComic.Data
{
    public class MvcComicContext2 : DbContext
    {
        public MvcComicContext2(DbContextOptions<MvcComicContext2> options)
            : base(options)
        {
        }

        public DbSet<MvcComic.Models.Comic> Comic { get; set; } = default!;
    }
}
