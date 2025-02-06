namespace Restaurant.Domain
{
    public sealed class OrderLineItem : Entity
    {
        public Guid ProductId { get; private set; }
        public Product? Product { get; private set; }
        public int Count { get; private set; }
        public decimal Price => Product?.Price * Count ?? 0;

        private OrderLineItem() : base(Guid.NewGuid()) { }

        public OrderLineItem(Guid productId, int count) : base(Guid.NewGuid())
        {
            ProductId = productId;
            Count = count;
        }

        public OrderLineItem(Guid productId, Product product, int count) : base(Guid.NewGuid())
        {
            ProductId = productId;
            Product = product;
            Count = count;
        }
    }
}