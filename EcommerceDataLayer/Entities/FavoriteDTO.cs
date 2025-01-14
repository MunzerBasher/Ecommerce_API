using System.ComponentModel.DataAnnotations;

public class FavoriteDTO
{
    [Required]
    public int ProductID { get; set; }
    [Required]
    public int UserID { get; set; }
    public bool IsFavorite { get; set; }
}