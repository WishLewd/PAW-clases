using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComponentController(ILogger<ComponentController> logger, IComponentRepository componentRepository) : ControllerBase
    {
        [HttpGet(Name = "GetComponents")]
        public async Task<IEnumerable<ComponentDTO>> GetAll()
        {
            var components = await componentRepository.ReadAsync() ?? [];
            return components.Select(ComponentDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetComponentById")]
        public async Task<ActionResult<ComponentDTO>> GetById(int id)
        {
            var component = await componentRepository.FindAsync(id);
            if (component == null) return NotFound();
            return ComponentDTO.ConvertFrom(component);
        }

        [HttpPost]
        public async Task<bool> Create([FromBody] Component component)
        {
            return await componentRepository.CreateAsync(component);
        }

        [HttpPut("{id:int}")]
        public async Task<bool> Update(int id, [FromBody] Component component)
        {
            component.Id = id;
            return await componentRepository.UpdateAsync(component);
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> DeleteById(int id)
        {
            var component = await componentRepository.FindAsync(id);
            if (component == null) return false;
            return await componentRepository.DeleteAsync(component);
        }
    }
}
