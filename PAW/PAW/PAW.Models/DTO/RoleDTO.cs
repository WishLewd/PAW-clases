using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class RoleDTO
{
    [JsonPropertyName("roleId")]
    public int RoleId { get; set; }

    [JsonPropertyName("roleName")]
    public string RoleName { get; set; } = string.Empty;

    public static RoleDTO ConvertFrom(Role role)
    {
        if (role == null) return null!;
        return new RoleDTO
        {
            RoleId = role.RoleId,
            RoleName = role.RoleName ?? string.Empty
        };
    }

    public static Role ConvertTo(RoleDTO dto)
    {
        if (dto == null) return null!;
        return new Role
        {
            RoleId = dto.RoleId,
            RoleName = dto.RoleName
        };
    }
}
