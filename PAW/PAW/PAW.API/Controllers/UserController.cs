using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController(ILogger<UserController> logger, IUserRepository userRepository) : ControllerBase
    {
        [HttpGet(Name = "GetUsers")]
        public async Task<IEnumerable<UserDTO>> GetAll()
        {
            var users = await userRepository.ReadAsync() ?? [];
            return users.Select(UserDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetUserById")]
        public async Task<ActionResult<UserDTO>> GetById(int id)
        {
            var user = await userRepository.FindAsync(id);
            if (user == null) return NotFound();
            return UserDTO.ConvertFrom(user);
        }

        [HttpPost]
        public async Task<bool> Create([FromBody] User user)
        {
            return await userRepository.CreateAsync(user);
        }

        [HttpPut("{id:int}")]
        public async Task<bool> Update(int id, [FromBody] User user)
        {
            user.UserId = id;
            return await userRepository.UpdateAsync(user);
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> DeleteById(int id)
        {
            var user = await userRepository.FindAsync(id);
            if (user == null) return false;
            return await userRepository.DeleteAsync(user);
        }
    }
}
