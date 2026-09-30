using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models.DTO;
using PawTask = PAW.Models.Task;

namespace PAW.Web.Services;

public interface ITaskService
{
    Task<IEnumerable<TaskDTO>> GetTasksAsync();
    Task<TaskDTO?> GetTaskByIdAsync(int id);
    Task<bool> CreateTaskAsync(TaskDTO dto);
    Task<bool> UpdateTaskAsync(int id, TaskDTO dto);
    Task<bool> DeleteTaskAsync(int id);
}

public class TaskService : ServiceBase, ITaskService
{
    private const string _path = "Task";
    private readonly IRestProvider _restProvider;

    public TaskService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<TaskDTO>> GetTasksAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var tasks = await JsonProvider.DeserializeAsync<IEnumerable<TaskDTO>>(response);
        return tasks ?? [];
    }

    public async Task<TaskDTO?> GetTaskByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path) + "/", id.ToString());
        return await JsonProvider.DeserializeAsync<TaskDTO>(response);
    }

    public async Task<bool> CreateTaskAsync(TaskDTO dto)
    {
        var entity = TaskDTO.ConvertTo(dto);
        var content = JsonProvider.Serialize(entity);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return bool.TryParse(response, out var result) && result;
    }

    public async Task<bool> UpdateTaskAsync(int id, TaskDTO dto)
    {
        var entity = TaskDTO.ConvertTo(dto);
        var content = JsonProvider.Serialize(entity);
        var response = await _restProvider.PutAsync(SetPathUrl(_path) + "/", id.ToString(), content);
        return bool.TryParse(response, out var result) && result;
    }

    public async Task<bool> DeleteTaskAsync(int id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path) + "/", id.ToString());
        return bool.TryParse(response, out var result) && result;
    }
}
