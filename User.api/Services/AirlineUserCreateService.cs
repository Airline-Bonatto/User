
using User.api.Models;
using User.api.Repositories;
using User.api.Requests;

namespace User.api.Services;

public class AirlineUserCreateService(
    AirlineUserRepository airlineUserRepository
)
{
    private readonly AirlineUserRepository _airlineUserRepository = airlineUserRepository;

    public async Task<int> CreateAirlineUserAsync(AirlineUserCreateRequest request)
    {
        AirlineUser user = new(request);
        return await _airlineUserRepository.AddAsync(user);
    }
}