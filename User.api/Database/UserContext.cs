using Microsoft.EntityFrameworkCore;

using User.api.Models;

namespace User.api.Database;

public class UserContext(DbContextOptions<UserContext> options) : DbContext(options)
{
    public DbSet<AirlineUser> AirlineUsers { get; set; }
    public DbSet<UserType> UserTypes { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AirlineUser>()
            .HasOne(e => e.UserType)
            .WithMany()
            .HasForeignKey(e => e.UserTypeId);

        modelBuilder.Entity<UserType>().HasData(
            new UserType { UserTypeId = 1, Description = "Airline User" },
            new UserType { UserTypeId = 2, Description = "Customer User" }
        );
    }
}