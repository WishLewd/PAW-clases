using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class InventoryDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }

    [JsonPropertyName("inventoryId")]
    public int InventoryId { get; set; }

    [JsonPropertyName("unitPrice")]
    public decimal? UnitPrice { get; set; }

    [JsonPropertyName("unitsInStock")]
    public int? UnitsInStock { get; set; }

    [JsonPropertyName("productId")]
    public int? ProductId { get; set; }

    [JsonPropertyName("dateAdded")]
    public DateTime? DateAdded { get; set; }

    [JsonPropertyName("lastUpdated")]
    public DateTime? LastUpdated { get; set; }

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

    public static InventoryDTO ConvertFrom(Inventory inventory)
    {
        if (inventory == null) return null!;
        return new InventoryDTO
        {
            Id = Guid.NewGuid(),
            InventoryId = inventory.InventoryId,
            UnitPrice = inventory.UnitPrice,
            UnitsInStock = inventory.UnitsInStock,
            ProductId = inventory.ProductId,
            DateAdded = inventory.DateAdded,
            LastUpdated = inventory.LastUpdated,
            ModifiedBy = inventory.ModifiedBy,
            CreatedBy = inventory.ModifiedBy,
            Comments = inventory.Comments ?? string.Empty,
            CreatedDate = inventory.DateAdded ?? DateTime.Now,
            ModifiedDate = inventory.LastUpdated ?? DateTime.Now
        };
    }

    public static Inventory ConvertTo(InventoryDTO dto)
    {
        if (dto == null) return null!;
        return new Inventory
        {
            InventoryId = dto.InventoryId,
            UnitPrice = dto.UnitPrice,
            UnitsInStock = dto.UnitsInStock,
            ProductId = dto.ProductId,
            DateAdded = dto.DateAdded,
            LastUpdated = dto.ModifiedDate,
            ModifiedBy = dto.ModifiedBy
        };
    }
}
