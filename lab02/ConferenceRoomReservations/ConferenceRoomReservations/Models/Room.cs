using System.ComponentModel.DataAnnotations;

public class Room
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required, Range(1, int.MaxValue)]
    public int Capacity { get; set; }

}