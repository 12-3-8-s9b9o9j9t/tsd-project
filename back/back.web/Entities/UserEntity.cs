using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace back.Entities;

[Table("user")]
public class UserEntity
{
    [Key, Required]
    public int id { get; set; }
    
    [Required]
    public string name { get; set; }
}

public class UserInput
{
    public string name { get; set; }
}