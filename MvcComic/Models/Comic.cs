using System.ComponentModel.DataAnnotations;

namespace MvcComic.Models;

public class Comic
{
    public int Id { get; set; }
    public string? Title { get; set; }
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }
    public int? Issue { get; set; }
    public string? IssueNumber { get; set; }
    public string? Genre { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
}