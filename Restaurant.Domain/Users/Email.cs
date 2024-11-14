using System.Text.RegularExpressions;
using Restaurant.Domain.Core.Primitives;
using Restaurant.Domain.Core.Primitives.Result;

namespace Restaurant.Domain.Users
{
    public sealed partial class Email : ValueObject
    {
        public const int MaxLength = 255;

        private const string EmailRegexPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        private static readonly Lazy<Regex> EmailFormatRegex = new(EmailRegex());

        public string Value { get; private set; }

        private Email(string value) => Value = value;

        public static implicit operator string(Email email) => email.Value;

        public static Result<Email> Create(string email) =>
            Result.Create(email, UserErrors.Email.NotEmpty)
                .Ensure(e => !string.IsNullOrWhiteSpace(e), UserErrors.Email.NotEmpty)
                .Ensure(e => e.Length <= MaxLength, UserErrors.Email.LongerThanAllowed)
                .Ensure(EmailFormatRegex.Value.IsMatch, UserErrors.Email.InvalidFormat)
                .Map(e => new Email(e));

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }

        [GeneratedRegex(EmailRegexPattern, RegexOptions.IgnoreCase | RegexOptions.Compiled, "en-US")]
        private static partial Regex EmailRegex();
    }
}