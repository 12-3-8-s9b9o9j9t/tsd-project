using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.Entities;

[Table("note")]
public class NoteEntity
{
    [Key]
    public int id { get; set; }
    
    [Required]
    public int note { get; set; }
    
    [Required]
    public UserStoryPropositionEntity UserStoryPropositionEntity { get; set; }
    
    [Required]
    public UserEntity UserEntity { get; set; }
}