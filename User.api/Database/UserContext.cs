using Microsoft.EntityFrameworkCore;

using User.api.Models;

namespace User.api.Database;

public class UserContext(DbContextOptions<UserContext> options) : DbContext(options)
{
    public DbSet<AirlineUser> AirlineUsers { get; set; }
}