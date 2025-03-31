namespace Restaurant.Persistence.Seeders;

public static class ProductCategorySeeder
{
    public static object[] GetProductCategories()
    {
        return [
            new { Id = Guid.Parse("04a8f982-b59f-41a8-b119-db5acdf96700"), Name = "Sushi" },
            new { Id = Guid.Parse("28a273f6-53b2-4815-b00c-41473b9cb346"),Name= "Drinks" },
            new { Id = Guid.Parse("3f468941-50ae-4bdf-aea4-47fdfd4cbb1e"), Name = "Steaks" },
            new { Id = Guid.Parse("50c1ab12-22f6-4c9e-8895-dd997e6286dd"), Name = "Barbecue" },
            new { Id = Guid.Parse("7c9883c4-a24a-4403-8588-5972ba14a90e"), Name = "Desserts" },
            new { Id = Guid.Parse("7cff41a6-125b-44c9-8a02-98a3808885fb"), Name = "Coffee" },
            new { Id = Guid.Parse("a3f97e6b-6fe9-4c52-bfe7-29fcedf10246"), Name = "Hot Dogs" },
            new { Id = Guid.Parse("c9c2d401-12d9-40ca-befa-2ffc70449e30"), Name = "Seafood" },
            new { Id = Guid.Parse("fbb342e6-910e-493c-93c7-5b1b58f69cd7"), Name = "Fast Food" },
            new { Id = Guid.Parse("fc32339e-b6cc-44e0-80cb-f0e38fc12d6b"), Name ="Pizzas" }
        ];
    }
}
