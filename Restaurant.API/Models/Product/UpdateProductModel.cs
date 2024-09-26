namespace Restaurant.API.Models.Product;

public record UpdateProductModel(string? Name, string? Description, decimal? Price, Guid? CategoryId);