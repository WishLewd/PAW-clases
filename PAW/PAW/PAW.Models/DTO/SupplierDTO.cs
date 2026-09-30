using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class SupplierDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("supplierId")]
    public int SupplierId { get; set; }

    [JsonPropertyName("supplierName")]
    public string SupplierName { get; set; } = string.Empty;

    [JsonPropertyName("contactName")]
    public string? ContactName { get; set; }

    [JsonPropertyName("contactTitle")]
    public string? ContactTitle { get; set; }

    [JsonPropertyName("phone")]
    public string? Phone { get; set; }

    [JsonPropertyName("address")]
    public string? Address { get; set; }

    [JsonPropertyName("city")]
    public string? City { get; set; }

    [JsonPropertyName("country")]
    public string? Country { get; set; }

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

    public static SupplierDTO ConvertFrom(Supplier supplier)
    {
        if (supplier == null) return null!;
        return new SupplierDTO
        {
            Id = Guid.NewGuid(),
            SupplierId = supplier.SupplierId,
            SupplierName = supplier.SupplierName ?? string.Empty,
            ContactName = supplier.ContactName,
            ContactTitle = supplier.ContactTitle,
            Phone = supplier.Phone,
            Address = supplier.Address,
            City = supplier.City,
            Country = supplier.Country,
            ModifiedBy = supplier.ModifiedBy,
            CreatedBy = supplier.ModifiedBy,
            Comments = supplier.Comments ?? string.Empty,
            CreatedDate = supplier.LastModified ?? DateTime.Now,
            ModifiedDate = supplier.LastModified ?? DateTime.Now
        };
    }

    public static Supplier ConvertTo(SupplierDTO dto)
    {
        if (dto == null) return null!;
        return new Supplier
        {
            SupplierId = dto.SupplierId,
            SupplierName = dto.SupplierName,
            ContactName = dto.ContactName,
            ContactTitle = dto.ContactTitle,
            Phone = dto.Phone,
            Address = dto.Address,
            City = dto.City,
            Country = dto.Country,
            ModifiedBy = dto.ModifiedBy,
            LastModified = dto.ModifiedDate
        };
    }
}
