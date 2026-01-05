
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace User.api.Models;

[Table("UserTypes")]
public class UserType
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("userTypeId")]
    public int UserTypeId { get; set; }

    [Column("description")]
    public string Description { get; set; } = null!;
}