using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MvcComic.Models;

public class ComicGenreViewModel
{
    public List<Comic>? Comics { get; set; }
    public SelectList? Genres { get; set; }
    public string? ComicGenre { get; set; }
    public string? SearchString { get; set; }
}