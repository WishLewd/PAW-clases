using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController(ILogger<RoleController> logger, IRoleRepository roleRepository) : ControllerBase
    {
        [HttpGet(Name = "GetRoles")]
        public async Task<IEnumerable<RoleDTO>> GetAll()
        {
            var roles = await roleRepository.ReadAsync() ?? [];
            return roles.Select(RoleDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetRoleById")]
        public async Task<ActionResult<RoleDTO>> GetById(int id)
        {
            var role = await roleRepository.FindAsync(id);
            if (role == null) return NotFound();
            return RoleDTO.ConvertFrom(role);
        }

        [HttpPost]
        public async Task<bool> Create([FromBody] Role role)
        {
            return await roleRepository.CreateAsync(role);
        }

        [HttpPut("{id:int}")]
        public async Task<bool> Update(int id, [FromBody] Role role)
        {
            role.RoleId = id;
            return await roleRepository.UpdateAsync(role);
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> DeleteById(int id)
        {
            var role = await roleRepository.FindAsync(id);
            if (role == null) return false;
            return await roleRepository.DeleteAsync(role);
        }
    }
}
