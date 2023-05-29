using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.Entities;

[Table("session")]
public class SessionEntity
{
    [Key, Required]
    public int id { get; set; }
    
    [Required]
    public string identifier { get; set; }
    
    [Required]
    public List<UserEntity> users { get; set; }
    
    [Required]
    public List<UserStoryEntity> userStories { get; set; }
}