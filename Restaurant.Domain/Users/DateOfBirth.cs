using Restaurant.Domain.Core.Primitives;
using Restaurant.Domain.Core.Primitives.Result;

namespace Restaurant.Domain.Users
{
    public sealed class DateOfBirth : ValueObject
    {
        public const string Format = "dd MM yyyy";

        public const int MinAge = 3;

        public const int MaxAge = 80;

        public DateOnly Value { get; }

        private DateOfBirth(DateOnly dateOfBirth) => Value = dateOfBirth;

        public static implicit operator string(DateOfBirth dateOfBirth) => dateOfBirth.Value.ToString(Format);

        public static Result<DateOfBirth> Create(DateOnly dateOfBirth) =>
            Result.Create(dateOfBirth, Error.None)
                .Ensure(d => (DateTime.UtcNow.Year - d.Year) >= MinAge, UserErrors.DateOfBirth.MinAge)
                .Ensure(d => (DateTime.UtcNow.Year - d.Year) <= MaxAge, UserErrors.DateOfBirth.MaxAge)
                .Map(d => new DateOfBirth(d));

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}