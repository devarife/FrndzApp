using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using api.Entities;
using api.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace api.Services;

public class TokenService(IConfiguration config) : ITokenService
{
    public string CreateToken(AppUser user)
    {
        var tokenKey = config["TokenKey"] ?? throw new Exception("Cannot access Token key in config.");
        if(tokenKey.Length < 64) throw new Exception("Your tokenKey needs to be longer than 64 char");

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey));
        
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserName)
        };

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(1),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
        
    }
}
