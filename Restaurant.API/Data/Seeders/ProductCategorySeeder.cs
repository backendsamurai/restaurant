using Restaurant.API.Entities;

namespace Restaurant.API.Data.Seeders;

public static class ProductCategorySeeder
{
    public static ProductCategory[] GetProductCategories()
    {
        return [
            new ProductCategory { Id = Guid.NewGuid(), Name = "Seafood" },
            new ProductCategory { Id = Guid.NewGuid(), Name = "Steaks" },
            new ProductCategory { Id = Guid.NewGuid(), Name = "Sushi" },
            new ProductCategory { Id = Guid.NewGuid(), Name = "Barbecue" },
            new ProductCategory { Id = Guid.NewGuid(), Name = "Hot Dogs" },
            new ProductCategory { Id = Guid.NewGuid(), Name = "Pizzas" },
            new ProductCategory { Id = Guid.NewGuid(), Name = "Drinks" },
            new ProductCategory { Id = Guid.NewGuid(), Name = "Coffee" },
            new ProductCategory { Id = Guid.NewGuid(), Name = "Fast Food" },
            new ProductCategory { Id = Guid.NewGuid(), Name = "Desserts" },
        ];
    }
}
