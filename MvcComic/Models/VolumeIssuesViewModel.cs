using System.ComponentModel.DataAnnotations;

namespace MvcComic.Models;

public class VolumeIssuesViewModel
{
    public string? VolumeTitle { get; set; }
    public List<Comic> Issues { get; set; } = new List<Comic>();
    public int Id { get; set; }
    public string? Title { get; set; }
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }
    public int? Issue { get; set; }
    public string? Genre { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; }
}
