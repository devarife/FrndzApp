using api.Data;
using api.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(DataContext dataContext) : ControllerBase
    {
        [HttpGet("All")]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var user = await dataContext.Users.ToListAsync();
            return user;
        }

        [HttpGet("single/{id}")]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var User = await dataContext.Users.FindAsync(id);

            if(User == null) return BadRequest();
            return User;
        }
    }
}
