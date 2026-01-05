using Microsoft.AspNetCore.Mvc;

using User.api.DTOs;
using User.api.Requests;
using User.api.Services;

namespace User.api.Controllers;

[ApiController]
[Route("airline-user")]
public class AirlineUserController(
    AirlineUserCreateService airlineUserCreateService,
    AuthService authService
) : ControllerBase
{

    private readonly AirlineUserCreateService _airlineUserCreateService = airlineUserCreateService;
    private readonly AuthService _authService = authService;

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

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] AuthenticationDto authDto)
    {
        try
        {
            AuthResponse? authResponse = await _authService.Login(authDto);
            if(authResponse == null)
            {
                return Unauthorized(new { Message = "Invalid credentials" });
            }
            return Ok(authResponse);
        }
        catch(Exception ex)
        {
            return StatusCode(500, new { Message = "Internal server error", Details = ex.Message });
        }
    }
}