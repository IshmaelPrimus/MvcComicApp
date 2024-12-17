namespace MvcComic.Models;

public class VolumeIssuesViewModel
{
    public string? VolumeTitle { get; set; }
    public List<Comic> Issues { get; set; } = new List<Comic>();
}
