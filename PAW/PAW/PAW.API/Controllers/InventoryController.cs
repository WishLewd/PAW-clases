using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class InventoryController(ILogger<InventoryController> logger, IInventoryRepository inventoryRepository) : ControllerBase
    {
        [HttpGet(Name = "GetInventories")]
        public async Task<IEnumerable<InventoryDTO>> GetAll()
        {
            var inventories = await inventoryRepository.ReadAsync() ?? [];
            return inventories.Select(InventoryDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetInventoryById")]
        public async Task<ActionResult<InventoryDTO>> GetById(int id)
        {
            var inventory = await inventoryRepository.FindAsync(id);
            if (inventory == null) return NotFound();
            return InventoryDTO.ConvertFrom(inventory);
        }

        [HttpPost]
        public async Task<bool> Create([FromBody] Inventory inventory)
        {
            return await inventoryRepository.CreateAsync(inventory);
        }

        [HttpPut("{id:int}")]
        public async Task<bool> Update(int id, [FromBody] Inventory inventory)
        {
            inventory.InventoryId = id;
            return await inventoryRepository.UpdateAsync(inventory);
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> DeleteById(int id)
        {
            var inventory = await inventoryRepository.FindAsync(id);
            if (inventory == null) return false;
            return await inventoryRepository.DeleteAsync(inventory);
        }
    }
}
