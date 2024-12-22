using Restaurant.Domain.Core.Primitives;
using Restaurant.Domain.Core.Primitives.Result;

namespace Restaurant.Domain.Users
{
    public sealed class PhoneNumber : ValueObject
    {
        public string Value { get; private set; }

        private PhoneNumber(string phoneNumber) => Value = phoneNumber;

        public static Result<PhoneNumber> Create(string phoneNumber) =>
            Result.Create(phoneNumber, UserErrors.PhoneNumber.NullOrEmpty)
                .Ensure(p => !string.IsNullOrWhiteSpace(p), UserErrors.PhoneNumber.NullOrEmpty)
                .Ensure(CheckPhoneNumber, UserErrors.PhoneNumber.InvalidFormat)
                .Map(p => new PhoneNumber(p));

        public static implicit operator string(PhoneNumber phoneNumber) => phoneNumber.Value;

        private static bool CheckPhoneNumber(string phoneNumber) =>
            !phoneNumber.ToArray().Any(c => !char.IsDigit(c));

        public override string ToString() => Value;

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return Value;
        }
    }
}