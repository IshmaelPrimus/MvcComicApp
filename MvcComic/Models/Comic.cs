using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcComic.Models;

public class Comic
{
    public int Id { get; set; }

    [StringLength(60, MinimumLength = 3)]
    [Required] 
    public string? Title { get; set; }

    [Display(Name = "Release Date")]
    [DataType(DataType.Date)]
    public DateTime ReleaseDate { get; set; }

    [Range(1, 1000)]
    [DataType(DataType.Currency)]
    public int? Issue { get; set; }

    [RegularExpression(@"^[A-Z]+[a-zA-Z\s]*$")]
    [Required]
    [StringLength(30)]
    public string? Genre { get; set; }

    [Range(1, 100)]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal Price { get; set; }

    [Range(1, 100)]
    [DataType(DataType.Currency)]
    public int? Grading { get; set; }
}