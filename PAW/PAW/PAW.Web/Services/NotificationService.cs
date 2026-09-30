using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationDTO>> GetNotificationsAsync();
    Task<NotificationDTO?> GetNotificationByIdAsync(int id);
    Task<bool> CreateNotificationAsync(NotificationDTO dto);
    Task<bool> UpdateNotificationAsync(int id, NotificationDTO dto);
    Task<bool> DeleteNotificationAsync(int id);
}

public class NotificationService : ServiceBase, INotificationService
{
    private const string _path = "Notification";
    private readonly IRestProvider _restProvider;

    public NotificationService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<NotificationDTO>> GetNotificationsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var notifications = await JsonProvider.DeserializeAsync<IEnumerable<NotificationDTO>>(response);
        return notifications ?? [];
    }

    public async Task<NotificationDTO?> GetNotificationByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path) + "/", id.ToString());
        return await JsonProvider.DeserializeAsync<NotificationDTO>(response);
    }

    public async Task<bool> CreateNotificationAsync(NotificationDTO dto)
    {
        var entity = NotificationDTO.ConvertTo(dto);
        var content = JsonProvider.Serialize(entity);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return bool.TryParse(response, out var result) && result;
    }

    public async Task<bool> UpdateNotificationAsync(int id, NotificationDTO dto)
    {
        var entity = NotificationDTO.ConvertTo(dto);
        var content = JsonProvider.Serialize(entity);
        var response = await _restProvider.PutAsync(SetPathUrl(_path) + "/", id.ToString(), content);
        return bool.TryParse(response, out var result) && result;
    }

    public async Task<bool> DeleteNotificationAsync(int id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path) + "/", id.ToString());
        return bool.TryParse(response, out var result) && result;
    }
}
