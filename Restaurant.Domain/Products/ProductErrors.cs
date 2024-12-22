using Restaurant.Domain.Core.Primitives;

namespace Restaurant.Domain.Products
{
    public static class ProductErrors
    {
        public static class DiscountRange
        {
            public static readonly Error EndOfRangeMustBeGreaterThenStart = new(
                "DiscountRange.EndOfRangeMustBeGreaterThanStart", "The end of range must be greater than start of range."
            );

            public static readonly Error StartOfRangeMustBeLessThenEnd = new(
                "DiscountRange.StartOfRangeMustBeLessThanEnd", "The start of range must be less than end of range."
            );
        }
    }
}