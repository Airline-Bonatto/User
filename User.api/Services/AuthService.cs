
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

using User.api.Configuration;
using User.api.DTOs;
using User.api.Models;
using User.api.Repositories;

namespace User.api.Services;

public class AuthService(
    AirlineUserRepository airlineUserRepository,
    IOptions<JwtConfiguration> jwtConfig
)
{
    private readonly AirlineUserRepository _airlineUserRepository = airlineUserRepository;
    private readonly JwtConfiguration _jwtConfig = jwtConfig.Value;


    public async Task<AuthResponse?> Login(AuthenticationDto authDto)
    {

        AirlineUser? user = await _airlineUserRepository.GetByCredentialsAsync(authDto);
        if(user == null)
        {
            return null;
        }

        string token = CreateToken(user.AirlineUserId);
        return new AuthResponse()
        {
            Token = token
        };
    }
    
    private string CreateToken(int userId)
    {
        SymmetricSecurityKey key = new(Encoding.ASCII.GetBytes(_jwtConfig.Secret));
        SigningCredentials creds = new(key, SecurityAlgorithms.HmacSha256);

        Claim[] claims =
        [
            new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        ];

        JwtSecurityToken token = new(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtConfig.ExpiryInMinutes),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

}