using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IRoleService
{
    Task<IEnumerable<RoleDTO>> GetRolesAsync();
    Task<RoleDTO?> GetRoleByIdAsync(int id);
    Task<bool> CreateRoleAsync(RoleDTO dto);
    Task<bool> UpdateRoleAsync(int id, RoleDTO dto);
    Task<bool> DeleteRoleAsync(int id);
}

public class RoleService : ServiceBase, IRoleService
{
    private const string _path = "Role";
    private readonly IRestProvider _restProvider;

    public RoleService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<RoleDTO>> GetRolesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var roles = await JsonProvider.DeserializeAsync<IEnumerable<RoleDTO>>(response);
        return roles ?? [];
    }

    public async Task<RoleDTO?> GetRoleByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path) + "/", id.ToString());
        return await JsonProvider.DeserializeAsync<RoleDTO>(response);
    }

    public async Task<bool> CreateRoleAsync(RoleDTO dto)
    {
        var entity = RoleDTO.ConvertTo(dto);
        var content = JsonProvider.Serialize(entity);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return bool.TryParse(response, out var result) && result;
    }

    public async Task<bool> UpdateRoleAsync(int id, RoleDTO dto)
    {
        var entity = RoleDTO.ConvertTo(dto);
        var content = JsonProvider.Serialize(entity);
        var response = await _restProvider.PutAsync(SetPathUrl(_path) + "/", id.ToString(), content);
        return bool.TryParse(response, out var result) && result;
    }

    public async Task<bool> DeleteRoleAsync(int id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path) + "/", id.ToString());
        return bool.TryParse(response, out var result) && result;
    }
}
