using System.Text.Json.Serialization;
using PawTask = PAW.Models.Task;

namespace PAW.Models.DTO;

public class TaskDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string? Status { get; set; }

    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("lastModified")]
    public DateTime? LastModified { get; set; }

    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }

    public static TaskDTO ConvertFrom(PawTask task)
    {
        if (task == null) return null!;
        return new TaskDTO
        {
            Id = task.Id,
            Name = task.Name ?? string.Empty,
            Description = task.Description ?? string.Empty,
            Status = task.Status,
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            LastModified = task.LastModified,
            ModifiedBy = task.ModifiedBy
        };
    }

    public static PawTask ConvertTo(TaskDTO dto)
    {
        if (dto == null) return null!;
        return new PawTask
        {
            Id = dto.Id,
            Name = dto.Name,
            Description = dto.Description,
            Status = dto.Status,
            DueDate = dto.DueDate,
            CreatedAt = dto.CreatedAt,
            LastModified = dto.LastModified,
            ModifiedBy = dto.ModifiedBy
        };
    }
}
