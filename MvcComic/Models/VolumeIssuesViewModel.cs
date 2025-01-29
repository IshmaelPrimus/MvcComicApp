using System.ComponentModel.DataAnnotations;

namespace MvcComic.Models;

public class VolumeIssuesViewModel
{
    public string? VolumeTitle { get; set; }
    public List<Comic> Issues { get; set; } = new List<Comic>();
    public Dictionary<int, int> SelectedQuantities { get; set; } = new Dictionary<int, int>();
    public int Id { get; set; }
    public int SelectedIssueId { get; set; } // Property for selected issue ID
    public string? Title { get; set; }
    [DataType(DataType.Date)]
    public string? Volume { get; set; }
    public string? IssueNumber { get; set; }

    public string? ImageUrl { get; set; }
}
