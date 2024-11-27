using Restaurant.Domain.Core.Primitives;
using Restaurant.Domain.Core.Primitives.Result;

namespace Restaurant.Domain.PaymentMethods
{
    public sealed class PaymentCard : ValueObject
    {
        public string Number { get; private set; }
        public string ExpireAt { get; private set; }
        public string CVV2 { get; private set; }

        private PaymentCard(string number, string expireAt, string cvv2)
        {
            Number = number;
            ExpireAt = expireAt;
            CVV2 = cvv2;
        }

        public static implicit operator string(PaymentCard card) => card.Number;

        public static Result<PaymentCard> Create(string number, DateOnly expireAt, string cvv2)
        {
            var numberResult = Result.Create(number, PaymentMethodErrors.PaymentCard.Number.NullOrEmpty)
                .Ensure(n => !string.IsNullOrWhiteSpace(n), PaymentMethodErrors.PaymentCard.Number.NullOrEmpty)
                .Ensure(CardNumberIsValid, PaymentMethodErrors.PaymentCard.Number.Invalid);

            var expireAtResult = Result.Create(expireAt, PaymentMethodErrors.PaymentCard.ExpireAt.Invalid)
                .Ensure(e => e != DateOnly.FromDateTime(DateTime.Now), PaymentMethodErrors.PaymentCard.ExpireAt.Invalid);

            var cvv2Result = Result.Create(cvv2, PaymentMethodErrors.PaymentCard.CVV2.NullOrEmpty)
                .Ensure(c => !string.IsNullOrWhiteSpace(c), PaymentMethodErrors.PaymentCard.CVV2.NullOrEmpty)
                .Ensure(c => c.Length < 3, PaymentMethodErrors.PaymentCard.CVV2.TooShort)
                .Ensure(CVV2IsValid, PaymentMethodErrors.PaymentCard.CVV2.Invalid);

            var result = Result.FirstFailureOrSuccess(numberResult, expireAtResult, cvv2Result);

            if (result.IsFailure)
                return Result.Failure<PaymentCard>(result.Error);

            var paymentCard = new PaymentCard(number, expireAt.ToString("M/y"), cvv2);

            return Result.Success(paymentCard);
        }

        public static bool CardNumberIsValid(string value)
        {
            int checksum = 0;
            bool evenDigit = false;

            for (int i = value.Length - 1; i >= 0; i--)
            {
                char digit = value[i];
                if (!char.IsAsciiDigit(digit))
                {
                    if (digit is '-' or ' ')
                    {
                        continue;
                    }

                    return false;
                }

                int digitValue = (digit - '0') * (evenDigit ? 2 : 1);
                evenDigit = !evenDigit;

                while (digitValue > 0)
                {
                    checksum += digitValue % 10;
                    digitValue /= 10;
                }
            }

            return (checksum % 10) == 0;
        }

        private static bool CVV2IsValid(string cvv2)
        {
            bool result = false;

            foreach (char c in cvv2)
                result = char.IsDigit(c);

            return result;
        }

        public override string ToString()
        {
            string result = "";
            int lastIdx = 0;

            for (int i = 1; i <= Number.Length; i++)
            {
                if (i % 4 == 0)
                {
                    result += Number.AsSpan(lastIdx, 4).ToString();
                    lastIdx = i;
                }

                if (i != Number.Length)
                    result += " ";
            }

            return result;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Number;
            yield return ExpireAt;
            yield return CVV2;
        }
    }
}