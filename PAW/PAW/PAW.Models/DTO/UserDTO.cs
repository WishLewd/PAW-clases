using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserDTO
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("username")]
    public string Username { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("isActive")]
    public bool? IsActive { get; set; }

    [JsonPropertyName("roleId")]
    public int? RoleId { get; set; }

    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    [JsonPropertyName("lastModified")]
    public DateTime? LastModified { get; set; }

    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }

    public static UserDTO ConvertFrom(User user)
    {
        if (user == null) return null!;
        return new UserDTO
        {
            UserId = user.UserId,
            Username = user.Username ?? string.Empty,
            Email = user.Email ?? string.Empty,
            IsActive = user.IsActive,
            RoleId = user.RoleId,
            CreatedAt = user.CreatedAt,
            LastModified = user.LastModified,
            ModifiedBy = user.ModifiedBy
        };
    }

    public static User ConvertTo(UserDTO dto)
    {
        if (dto == null) return null!;
        return new User
        {
            UserId = dto.UserId,
            Username = dto.Username,
            Email = dto.Email,
            IsActive = dto.IsActive,
            RoleId = dto.RoleId,
            CreatedAt = dto.CreatedAt,
            LastModified = dto.LastModified,
            ModifiedBy = dto.ModifiedBy
        };
    }
}
