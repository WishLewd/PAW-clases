using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class CategoryDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("categoryId")]
    public int CategoryId { get; set; }

    [JsonPropertyName("categoryName")]
    public string CategoryName { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }

    [JsonPropertyName("createdBy")]
    public string? CreatedBy { get; set; }

    [JsonPropertyName("comments")]
    public string Comments { get; set; } = string.Empty;

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }

    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }

    public static CategoryDTO ConvertFrom(Category category)
    {
        if (category == null) return null!;
        return new CategoryDTO
        {
            Id = Guid.NewGuid(),
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName ?? string.Empty,
            Description = category.Description ?? string.Empty,
            ModifiedBy = category.ModifiedBy,
            CreatedBy = category.ModifiedBy,
            Comments = category.Comments ?? string.Empty,
            CreatedDate = category.LastModified ?? DateTime.Now,
            ModifiedDate = category.LastModified ?? DateTime.Now
        };
    }

    public static Category ConvertTo(CategoryDTO dto)
    {
        if (dto == null) return null!;
        return new Category
        {
            CategoryId = dto.CategoryId,
            CategoryName = dto.CategoryName,
            Description = dto.Description,
            ModifiedBy = dto.ModifiedBy,
            LastModified = dto.ModifiedDate
        };
    }
}
