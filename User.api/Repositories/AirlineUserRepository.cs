
using User.api.Database;
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
}