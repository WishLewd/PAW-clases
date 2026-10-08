using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class ProductDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("rating")]
    public int Rating { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }
    [JsonPropertyName("createdBy")]
    public string? CreatedBy { get; set; }
    [JsonPropertyName("comments")]
    public string Comments { get; set; }
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }

    [JsonPropertyName("categoryId")]
    public int? CategoryId { get; set; }
    [JsonPropertyName("category")]
    public CategoryDTO? Category { get; set; }

    [JsonPropertyName("supplierId")]
    public int? SupplierId { get; set; }
    [JsonPropertyName("supplier")]
    public SupplierDTO? Supplier { get; set; }

    [JsonPropertyName("inventoryId")]
    public int? InventoryId { get; set; }
    [JsonPropertyName("inventory")]
    public InventoryDTO? Inventory { get; set; }

    public static ProductDTO ConvertFrom(Product product)
    {
        if (product == null) return null!;
        return new ProductDTO
        {
            Id = Guid.NewGuid(),
            ProductId = product.ProductId,
            Name = product.ProductName ?? string.Empty,
            Description = product.Description ?? string.Empty,
            Rating = (int)(product.Rating ?? 0),
            ModifiedBy = product.ModifiedBy,
            CreatedBy = product.CreatedBy,
            Comments = product.Comments ?? string.Empty,
            CreatedDate = product.LastModified ?? DateTime.Now,
            ModifiedDate = product.LastModified ?? DateTime.Now,
            CategoryId = product.CategoryId,
            SupplierId = product.SupplierId,
            InventoryId = product.InventoryId,
            Category = product.Category != null ? CategoryDTO.ConvertFrom(product.Category) : null,
            Supplier = product.Supplier != null ? SupplierDTO.ConvertFrom(product.Supplier) : null,
            Inventory = product.Inventory != null ? InventoryDTO.ConvertFrom(product.Inventory) : null
        };
    }

    public static Product ConvertTo(ProductDTO productDTO)
    {
        if (productDTO == null) return null!;
        return new Product
        {
            ProductId = productDTO.ProductId,
            ProductName = productDTO.Name,
            Description = productDTO.Description,
            Rating = productDTO.Rating,
            ModifiedBy = productDTO.ModifiedBy,
            CreatedBy = productDTO.CreatedBy,
            LastModified = productDTO.ModifiedDate,
            CategoryId = productDTO.CategoryId,
            SupplierId = productDTO.SupplierId,
            InventoryId = productDTO.InventoryId
        };
    }
}
