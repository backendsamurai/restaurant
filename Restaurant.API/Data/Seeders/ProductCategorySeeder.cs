using Restaurant.Domain;

namespace Restaurant.API.Data.Seeders;

public static class ProductCategorySeeder
{
    public static ProductCategory[] GetProductCategories()
    {
        return [
            new ("Seafood"),
            new ("Steaks"),
            new ("Sushi"),
            new ("Barbecue"),
            new ("Hot Dogs"),
            new ("Pizzas"),
            new ("Drinks"),
            new ("Coffee"),
            new ("Fast Food"),
            new ("Desserts"),
        ];
    }
}
