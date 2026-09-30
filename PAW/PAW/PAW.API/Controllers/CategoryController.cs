using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController(ILogger<CategoryController> logger, ICategoryRepository categoryRepository) : ControllerBase
    {
        [HttpGet(Name = "GetCategories")]
        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            var categories = await categoryRepository.ReadAsync() ?? [];
            return categories.Select(CategoryDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            var category = await categoryRepository.FindAsync(id);
            if (category == null) return NotFound();
            return CategoryDTO.ConvertFrom(category);
        }

        [HttpPost]
        public async Task<bool> Create([FromBody] Category category)
        {
            return await categoryRepository.CreateAsync(category);
        }

        [HttpPut("{id:int}")]
        public async Task<bool> Update(int id, [FromBody] Category category)
        {
            category.CategoryId = id;
            return await categoryRepository.UpdateAsync(category);
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> DeleteById(int id)
        {
            var category = await categoryRepository.FindAsync(id);
            if (category == null) return false;
            return await categoryRepository.DeleteAsync(category);
        }
    }
}
