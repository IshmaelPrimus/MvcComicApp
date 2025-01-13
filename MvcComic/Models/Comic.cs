using System.ComponentModel.DataAnnotations;

namespace MvcComic.Models;

public class Comic
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Volume { get; set; }

    //[DataType(DataType.Date)]
    //public DateTime ReleaseDate { get; set; }
    public string? IssueNumber { get; set; }
    public string? ImageUrl { get; set; }

    public int Quantity { get; set; }
}