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
    public int? estimatedCost { get; set; }
    
    public UserStoryEntity() {}

    public UserStoryEntity(string? descr, int? cost)
    {
        this.description = descr;
        this.estimatedCost = cost;
    }
}

public class UserStoryInput
{
    public string? description { get; set; }
    
    public int? estimatedCost { get; set; }
}