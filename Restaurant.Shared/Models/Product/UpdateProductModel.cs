namespace Restaurant.Shared.Models.Product;

public record UpdateProductModel(string? Name, string? Description, decimal? Price, Guid? CategoryId);