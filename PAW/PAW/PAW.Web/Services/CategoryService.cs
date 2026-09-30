using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface ICategoryService
{
    Task<IEnumerable<CategoryDTO>> GetCategoriesAsync();
    Task<CategoryDTO?> GetCategoryByIdAsync(int id);
    Task<bool> CreateCategoryAsync(CategoryDTO dto);
    Task<bool> UpdateCategoryAsync(int id, CategoryDTO dto);
    Task<bool> DeleteCategoryAsync(int id);
}

public class CategoryService : ServiceBase, ICategoryService
{
    private const string _path = "Category";
    private readonly IRestProvider _restProvider;

    public CategoryService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<CategoryDTO>> GetCategoriesAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var categories = await JsonProvider.DeserializeAsync<IEnumerable<CategoryDTO>>(response);
        return categories ?? [];
    }

    public async Task<CategoryDTO?> GetCategoryByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path) + "/", id.ToString());
        return await JsonProvider.DeserializeAsync<CategoryDTO>(response);
    }

    public async Task<bool> CreateCategoryAsync(CategoryDTO dto)
    {
        var entity = CategoryDTO.ConvertTo(dto);
        var content = JsonProvider.Serialize(entity);
        var response = await _restProvider.PostAsync(SetPathUrl(_path), content);
        return bool.TryParse(response, out var result) && result;
    }

    public async Task<bool> UpdateCategoryAsync(int id, CategoryDTO dto)
    {
        var entity = CategoryDTO.ConvertTo(dto);
        var content = JsonProvider.Serialize(entity);
        var response = await _restProvider.PutAsync(SetPathUrl(_path) + "/", id.ToString(), content);
        return bool.TryParse(response, out var result) && result;
    }

    public async Task<bool> DeleteCategoryAsync(int id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path) + "/", id.ToString());
        return bool.TryParse(response, out var result) && result;
    }
}
