namespace PAW.Web.Models;

public class PaginationModel
{
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 25;
    public int TotalItems { get; set; }
    public int TotalPages => (int)Math.Max(1, Math.Ceiling((double)TotalItems / PageSize));
    public string Controller { get; set; } = string.Empty;
    public string Action { get; set; } = "Index";

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}
