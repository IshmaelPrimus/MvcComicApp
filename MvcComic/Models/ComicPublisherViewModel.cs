using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace MvcComic.Models;

public class ComicPublisherViewModel
{
    public List<Comic>? Comics { get; set; }
    public SelectList? Publisher { get; set; }
    public string? ComicPublisher { get; set; }
    public string? SearchString { get; set; }
}