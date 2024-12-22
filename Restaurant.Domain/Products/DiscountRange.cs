using Restaurant.Domain.Core.Primitives;
using Restaurant.Domain.Core.Primitives.Result;

namespace Restaurant.Domain.Products
{
    public sealed class DiscountRange : ValueObject
    {
        public int Start { get; private set; }
        public int End { get; private set; }

        private DiscountRange(int start, int end)
        {
            Start = start;
            End = end;
        }

        public static Result<DiscountRange> Create(int start, int end)
        {
            if (start > end)
                return Result.Failure<DiscountRange>(ProductErrors.DiscountRange.StartOfRangeMustBeLessThenEnd);

            if (end < start)
                return Result.Failure<DiscountRange>(ProductErrors.DiscountRange.EndOfRangeMustBeGreaterThenStart);

            return Result.Success(new DiscountRange(start, end));
        }

        public static implicit operator string(DiscountRange discountRange) => $"{discountRange.Start} - {discountRange.End}";
        public override string ToString() => $"{Start} - {End}";

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Start;
            yield return End;
        }
    }
}