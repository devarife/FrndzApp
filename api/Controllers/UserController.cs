using api.Data;
using api.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers
{
    public class UserController(DataContext dataContext) : BaseApiController
    {
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetUsers()
        {
            var user = await dataContext.Users.ToListAsync();
            return user;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<AppUser>> GetUser(int id)
        {
            var User = await dataContext.Users.FindAsync(id);

            if(User == null) return BadRequest();
            return User;
        }
    }
}
