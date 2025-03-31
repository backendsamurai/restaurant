namespace Restaurant.Domain
{
    public sealed class OrderLineItem : Entity
    {
        public Guid ProductId { get; private set; }
        public Product? Product { get; private set; }
        public int Count { get; private set; }
        public decimal Price => Product?.Price * Count ?? 0;

        private OrderLineItem() { }

        private OrderLineItem(Guid id, Guid productId, int count) : base(id)
        {
            ProductId = productId;
            Count = count;
        }

        private OrderLineItem(Guid id, Product product, int count) : base(id)
        {
            ProductId = product.Id;
            Product = product;
            Count = count;
        }

        public static OrderLineItem CreateWithProductId(Guid id, Guid productId, int count) => new(id, productId, count);

        public static OrderLineItem Create(Guid id, Product product, int count) => new(id, product, count);
    }
}