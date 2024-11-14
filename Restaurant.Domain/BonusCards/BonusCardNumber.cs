using System.Text;
using Restaurant.Domain.Core.Primitives;

namespace Restaurant.Domain.BonusCards
{
    public sealed class BonusCardNumber : ValueObject
    {
        private static readonly StringBuilder _sb = new();
        private static readonly Random _random = new();

        private static readonly char[] Characters =
        [
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M',
            'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        ];

        private const int CardNumberLength = 8;

        private BonusCardNumber(string value) => Value = value;

        public string Value { get; }

        public static BonusCardNumber Create()
        {
            while (_sb.Length < CardNumberLength)
            {
                _sb.Append(_random.Next(10, 99));
                _sb.Append(_random.Next(0, Characters.Length - 1));
                _sb.Append(_random.Next(0, Characters.Length - 1));
            }

            var result = _sb.ToString();

            _sb.Clear();

            return new BonusCardNumber(result);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Value;
        }
    }
}