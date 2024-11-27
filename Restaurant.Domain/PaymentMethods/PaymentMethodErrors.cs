using Restaurant.Domain.Core.Primitives;

namespace Restaurant.Domain.PaymentMethods
{
    public static class PaymentMethodErrors
    {
        public static class PaymentCard
        {
            public static class Number
            {
                public static readonly Error NullOrEmpty = new(
                    "PaymentCard.Number.NullOrEmpty", "The card number cannot be empty."
                );

                public static readonly Error Invalid = new(
                    "PaymentCard.Number.Invalid", "The card number has invalid format."
                );
            }

            public static class ExpireAt
            {
                public static readonly Error Invalid = new(
                    "PaymentCard.ExpireAt.Invalid", "The card expireAt date is invalid."
                );
            }

            public static class CVV2
            {
                public static readonly Error NullOrEmpty = new(
                    "PaymentCard.CVV2.NullOrEmpty", "The cvv2 cannot be empty."
                );

                public static readonly Error TooShort = new(
                    "PaymentCard.CVV2.TooShort", "The cvv2 must contain 3 or more digits."
                );

                public static readonly Error Invalid = new(
                    "PaymentCard.CVV2.Invalid", "The cvv2 is invalid."
                );
            }
        }
    }
}