using APW.Architecture;
using PAW.Architecture.Providers;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.Web.Services;

public interface IProductService
{
    Task<IEnumerable<ProductDTO>> GetProductsAsync();
    Task<ProductDTO?> GetProductByIdAsync(int id);
    Task<bool> CreateProductAsync(ProductDTO productDto);
    Task<bool> UpdateProductAsync(int id, ProductDTO productDto);
    Task<bool> DeleteProductAsync(int id);
}

public class ProductService : ServiceBase, IProductService
{
    private const string _path = "Product";
    private readonly IRestProvider _restProvider;

    public ProductService(IRestProvider restProvider)
    {
        _restProvider = restProvider;
    }

    public async Task<IEnumerable<ProductDTO>> GetProductsAsync()
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path), id: null);
        var products = await JsonProvider.DeserializeAsync<IEnumerable<ProductDTO>>(response);
        return products ?? [];
    }

    public async Task<ProductDTO?> GetProductByIdAsync(int id)
    {
        var response = await _restProvider.GetAsync(SetPathUrl(_path) + "/", id.ToString());
        var product = await JsonProvider.DeserializeAsync<ProductDTO>(response);
        return product;
    }

    public async Task<bool> CreateProductAsync(ProductDTO productDto)
    {
        var entity = ProductDTO.ConvertTo(productDto);
        var content = JsonProvider.Serialize(entity);
        var response = await _restProvider.PostAsync(SetPathUrl(_path) + "/single", content);
        return bool.TryParse(response, out var result) && result;
    }

    public async Task<bool> UpdateProductAsync(int id, ProductDTO productDto)
    {
        var entity = ProductDTO.ConvertTo(productDto);
        var content = JsonProvider.Serialize(entity);
        var response = await _restProvider.PutAsync(SetPathUrl(_path) + "/", id.ToString(), content);
        return bool.TryParse(response, out var result) && result;
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        var response = await _restProvider.DeleteAsync(SetPathUrl(_path) + "/", id.ToString());
        return bool.TryParse(response, out var result) && result;
    }
}
