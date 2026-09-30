using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models.DTO;
using PawTask = PAW.Models.Task;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TaskController(ILogger<TaskController> logger, ITaskRepository taskRepository) : ControllerBase
    {
        [HttpGet(Name = "GetTasks")]
        public async Task<IEnumerable<TaskDTO>> GetAll()
        {
            var tasks = await taskRepository.ReadAsync() ?? [];
            return tasks.Select(TaskDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetTaskById")]
        public async Task<ActionResult<TaskDTO>> GetById(int id)
        {
            var task = await taskRepository.FindAsync(id);
            if (task == null) return NotFound();
            return TaskDTO.ConvertFrom(task);
        }

        [HttpPost]
        public async Task<bool> Create([FromBody] PawTask task)
        {
            return await taskRepository.CreateAsync(task);
        }

        [HttpPut("{id:int}")]
        public async Task<bool> Update(int id, [FromBody] PawTask task)
        {
            task.Id = id;
            return await taskRepository.UpdateAsync(task);
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> DeleteById(int id)
        {
            var task = await taskRepository.FindAsync(id);
            if (task == null) return false;
            return await taskRepository.DeleteAsync(task);
        }
    }
}
