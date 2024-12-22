using Restaurant.Domain.Core.Primitives;
using Restaurant.Domain.Core.Primitives.Result;

namespace Restaurant.Domain.Users
{
    public sealed class LastName : ValueObject
    {
        public const int MinLength = 2;
        public const int MaxLength = 100;

        public string Value { get; private set; }

        private LastName(string value) => Value = value;

        public static implicit operator string(LastName lastName) => lastName.Value;

        public static Result<LastName> Create(string lastName) =>
            Result.Create(lastName, UserErrors.LastName.NotEmpty)
                .Ensure(l => !string.IsNullOrWhiteSpace(l), UserErrors.LastName.NotEmpty)
                .Ensure(l => l.Length >= MinLength, UserErrors.LastName.ShortenThanAllowed)
                .Ensure(l => l.Length <= MaxLength, UserErrors.LastName.LongerThanAllowed)
                .Map(l => new LastName(l));

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Value;
        }
    }
}