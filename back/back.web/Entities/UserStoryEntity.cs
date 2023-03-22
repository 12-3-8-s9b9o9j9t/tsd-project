using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.Entities;

[Table("userStory")]
public class UserStoryEntity
{
    [Key, Required]
    public int id { get; set; }
    
    [Required]
    public string? description { get; set; }
    
    [Required]
    public int? estimatedCost { get; set;  }
}