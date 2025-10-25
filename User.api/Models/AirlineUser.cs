using User.api.Requests;

namespace User.api.Models;

public class AirlineUser
{
    public int AirlineUserId { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string Document { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
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