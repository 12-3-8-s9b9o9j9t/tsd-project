using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.Entities;

[Table("userStoryProposition")]
public class UserStoryPropositionEntity
{
    [Required, Key]
    public int id { get; set; }
    
    [Required]
    public string? description { get; set; }
    
    [Required]
    public string? tasks { get; set; }  // in json format 
    
    public UserStoryPropositionEntity() {}

    public UserStoryPropositionEntity(string? descr)
    {
        this.description = descr;
        this.tasks = "{tasks:[]}"; 
    }
}

public class UserStoryPropositionInput
{
    public string? description { get; set; }
    
    public string? tasks { get; set; }
}