using Microsoft.AspNetCore.Mvc;

using User.api.Requests;
using User.api.Services;

namespace User.api.Controllers;

[ApiController]
[Route("airline-user")]
public class AirlineUserController(
    AirlineUserCreateService airlineUserCreateService
) : ControllerBase
{

    private readonly AirlineUserCreateService _airlineUserCreateService = airlineUserCreateService;

    [HttpPost("create")]
    public async Task<IActionResult> CreateAirlineUser([FromBody] AirlineUserCreateRequest data)
    {
        try
        {
            int airlineUserId = await _airlineUserCreateService.CreateAirlineUserAsync(data);
            return Ok(new { AirlineUserId = airlineUserId });
        }
        catch(Exception ex)
        {
            return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
        }
    }
}