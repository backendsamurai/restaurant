namespace Restaurant.Shared.Models.Product;

public record CreateProductModel(string Name, string Description, decimal Price, Guid CategoryId);