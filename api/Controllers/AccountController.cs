using System.Security.Cryptography;
using System.Text;
using api.Data;
using api.DTOs;
using api.Entities;
using api.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController(DataContext context,ITokenService tokenService) : ControllerBase
    {
        [HttpPost("Register")]
        public async Task<ActionResult<UserDto>> RegisterUser(RegisterDto registerDto)
        {
            if(await UserExists(registerDto.Username)) return BadRequest("Usesr already exists!");

            var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDto.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key
            };

            context.Users.Add(user);
            await context.SaveChangesAsync();

            return new UserDto
            {
                Username = registerDto.Username,
                Token = tokenService.CreateToken(user)
            };
        }

        [HttpPost("Login")]
        public async Task<ActionResult<UserDto>> LoginUser(LoginDto loginDto)
        {
            var user = await context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
            if(user == null) return Unauthorized("Invalid Username");

            var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if(computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }
            
            return new UserDto
            {
                Username = loginDto.Username,
                Token =  tokenService.CreateToken(user)
            };
        }

        public async Task<bool> UserExists(string Username)
        {
            return await context.Users.AnyAsync(x => x.UserName.ToLower() == Username.ToLower());
        }
    }
}