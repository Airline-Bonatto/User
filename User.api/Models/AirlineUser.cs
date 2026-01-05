using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using User.api.Requests;

namespace User.api.Models;

[Table("AirlineUsers")]
public class AirlineUser
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("airlineUserId")]
    public int AirlineUserId { get; set; }

    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Column("password")]
    public string Password { get; set; } = string.Empty;

    [Column("document")]
    public string Document { get; set; } = string.Empty;

    [Column("name")]
    public string Name { get; set; } = string.Empty;

    [Column("lastName")]
    public string LastName { get; set; } = string.Empty;


    public AirlineUser()
    {
    }

    public AirlineUser(AirlineUserCreateRequest request)
    {
        Email = request.Email;
        Password = request.Password;
        Document = request.Document;
        Name = request.Name;
        LastName = request.LastName;
    }
}