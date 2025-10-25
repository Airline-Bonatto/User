
using Microsoft.EntityFrameworkCore;

using User.api.Database;
using User.api.DTOs;
using User.api.Models;

namespace User.api.Repositories;

public class AirlineUserRepository(
    UserContext dbContext
)
{
    private readonly UserContext _dbContext = dbContext;


    public async Task<int> AddAsync(AirlineUser airlineUser)
    {
        _dbContext.AirlineUsers.Add(airlineUser);
        await _dbContext.SaveChangesAsync();
        return airlineUser.AirlineUserId;
    }

    public async Task<AirlineUser?> GetByCredentialsAsync(AuthenticationDto authDto)
    {
        return await _dbContext.AirlineUsers
            .FirstOrDefaultAsync(u => u.Email == authDto.Username && u.Password == authDto.Password);
    }
}