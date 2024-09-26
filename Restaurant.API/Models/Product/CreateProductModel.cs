namespace Restaurant.API.Models.Product;

public record CreateProductModel(string Name, string Description, decimal Price, Guid CategoryId);