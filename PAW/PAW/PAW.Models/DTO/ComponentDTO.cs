using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class ComponentDTO
{
    [JsonPropertyName("id")]
    public decimal Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    [JsonPropertyName("guidId")]
    public Guid GuidId { get; set; }

    [JsonPropertyName("comments")]
    public string Comments { get; set; } = string.Empty;

    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }

    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }

    public static ComponentDTO ConvertFrom(Component component)
    {
        if (component == null) return null!;
        return new ComponentDTO
        {
            GuidId = Guid.NewGuid(),
            Id = component.Id,
            Name = component.Name,
            Content = component.Content,
            Comments = component.Comments ?? string.Empty,
            CreatedDate = DateTime.Now,
            ModifiedDate = DateTime.Now
        };
    }

    public static Component ConvertTo(ComponentDTO dto)
    {
        if (dto == null) return null!;
        return new Component
        {
            Id = dto.Id,
            Name = dto.Name,
            Content = dto.Content
        };
    }
}
