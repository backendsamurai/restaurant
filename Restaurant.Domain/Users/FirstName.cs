using Restaurant.Domain.Core.Primitives;
using Restaurant.Domain.Core.Primitives.Result;

namespace Restaurant.Domain.Users
{
    public sealed class FirstName : ValueObject
    {
        public const int MinLength = 2;
        public const int MaxLength = 100;

        public string Value { get; private set; }

        private FirstName(string value) => Value = value;

        public static implicit operator string(FirstName firstName) => firstName.Value;

        public static Result<FirstName> Create(string firstName) =>
            Result.Create(firstName, UserErrors.FirstName.NotEmpty)
                .Ensure(f => !string.IsNullOrWhiteSpace(f), UserErrors.FirstName.NotEmpty)
                .Ensure(f => f.Length >= MinLength, UserErrors.FirstName.ShortenThanAllowed)
                .Ensure(f => f.Length <= MaxLength, UserErrors.FirstName.LongerThanAllowed)
                .Map(f => new FirstName(f));

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Value;
        }
    }
}