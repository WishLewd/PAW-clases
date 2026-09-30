using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupplierController(ILogger<SupplierController> logger, ISupplierRepository supplierRepository) : ControllerBase
    {
        [HttpGet(Name = "GetSuppliers")]
        public async Task<IEnumerable<SupplierDTO>> GetAll()
        {
            var suppliers = await supplierRepository.ReadAsync() ?? [];
            return suppliers.Select(SupplierDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetSupplierById")]
        public async Task<ActionResult<SupplierDTO>> GetById(int id)
        {
            var supplier = await supplierRepository.FindAsync(id);
            if (supplier == null) return NotFound();
            return SupplierDTO.ConvertFrom(supplier);
        }

        [HttpPost]
        public async Task<bool> Create([FromBody] Supplier supplier)
        {
            return await supplierRepository.CreateAsync(supplier);
        }

        [HttpPut("{id:int}")]
        public async Task<bool> Update(int id, [FromBody] Supplier supplier)
        {
            supplier.SupplierId = id;
            return await supplierRepository.UpdateAsync(supplier);
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> DeleteById(int id)
        {
            var supplier = await supplierRepository.FindAsync(id);
            if (supplier == null) return false;
            return await supplierRepository.DeleteAsync(supplier);
        }
    }
}
