
namespace Restaurant.Domain.Core.Primitives
{
    public sealed class Error(string code, string message) : ValueObject
    {
        public string Code { get; } = code;
        public string Message { get; } = message;

        public static implicit operator string(Error error) => error.Code;

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Code;
            yield return Message;
        }

        internal static Error None => new(string.Empty, string.Empty);
    }
}