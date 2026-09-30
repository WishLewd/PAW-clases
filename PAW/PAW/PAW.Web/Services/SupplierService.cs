using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDTO>> GetSuppliersAsync();
    Task<SupplierDTO?> GetSupplierByIdAsync(int id);
    Task<bool> CreateSupplierAsync(SupplierDTO dto);
    Task<bool> UpdateSupplierAsync(int id, SupplierDTO dto);
    Task<bool> DeleteSupplierAsync(int id);
}

public class SupplierService : ServiceBase, ISupplierService
{
    private const string _path = "Supplier";
    private readonly IRestProvider _restProvider;

    public SupplierService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<SupplierDTO>> GetSuppliersAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var suppliers = await JsonProvider.DeserializeAsync<IEnumerable<SupplierDTO>>(response);
        return suppliers ?? [];
    }

    public async Task<SupplierDTO?> GetSupplierByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path) + "/", id.ToString());
        return await JsonProvider.DeserializeAsync<SupplierDTO>(response);
    }

    public async Task<bool> CreateSupplierAsync(SupplierDTO dto)
    {
        var entity = SupplierDTO.ConvertTo(dto);
        var content = JsonProvider.Serialize(entity);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return bool.TryParse(response, out var result) && result;
    }

    public async Task<bool> UpdateSupplierAsync(int id, SupplierDTO dto)
    {
        var entity = SupplierDTO.ConvertTo(dto);
        var content = JsonProvider.Serialize(entity);
        var response = await _restProvider.PutAsync(SetPathUrl(_path) + "/", id.ToString(), content);
        return bool.TryParse(response, out var result) && result;
    }

    public async Task<bool> DeleteSupplierAsync(int id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path) + "/", id.ToString());
        return bool.TryParse(response, out var result) && result;
    }
}
