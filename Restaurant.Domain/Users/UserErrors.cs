using Restaurant.Domain.Core.Primitives;

namespace Restaurant.Domain.Users
{
    public static class UserErrors
    {
        public static class FirstName
        {
            public static readonly Error NotEmpty = new(
                "User.FirstName.NotEmpty",
                "The first name cannot be empty."
            );

            public static readonly Error ShortenThanAllowed = new(
                "User.FirstName.ShortenThanAllowed",
                "The first name is shorten than allowed."
            );

            public static readonly Error LongerThanAllowed = new(
                "User.FirstName.LongerThanAllowed",
                "The first name is longer than allowed."
            );
        }

        public static class LastName
        {
            public static readonly Error NotEmpty = new(
                "User.LastName.NotEmpty",
                "The last name cannot be empty."
            );

            public static readonly Error ShortenThanAllowed = new(
                "User.LastName.ShortenThanAllowed",
                "The last name is shorten than allowed."
            );

            public static readonly Error LongerThanAllowed = new(
                "User.LastName.LongerThanAllowed",
                "The last name is longer than allowed."
            );
        }

        public static class Email
        {
            public static readonly Error NotEmpty = new(
                "User.Email.NotEmpty",
                "The email cannot be empty."
            );

            public static readonly Error LongerThanAllowed = new(
                "User.Email.LongerThanAllowed",
                "The email is longer than allowed."
            );

            public static readonly Error InvalidFormat = new(
                "User.Email.InvalidFormat",
                "The email has invalid format."
            );
        }

        public static class DateOfBirth
        {
            public static readonly Error MinAge = new(
                "User.DateOfBirth.MinAge",
                "The date of birth cannot be allowed by age requirements."
            );

            public static readonly Error MaxAge = new(
                "User.DateOfBirth.MaxAge",
                "The date of birth cannot be allowed by age requirements."
            );
        }

        public static class Address
        {
            public static class Country
            {
                public static readonly Error NotEmpty = new(
                    "User.Address.Country.NotEmpty",
                    "The country cannot be empty."
                );

                public static readonly Error InvalidFormat = new(
                    "User.Address.Country.InvalidFormat",
                    "The country has invalid format."
                );
            }

            public static class City
            {
                public static readonly Error NotEmpty = new(
                    "User.Address.City.NotEmpty",
                    "The city cannot be empty."
                );

                public static readonly Error InvalidFormat = new(
                    "User.Address.City.InvalidFormat",
                    "The city has invalid format."
                );
            }

            public static class Region
            {
                public static readonly Error NotEmpty = new(
                    "User.Address.Region.NotEmpty",
                    "The region cannot be empty."
                );

                public static readonly Error InvalidFormat = new(
                    "User.Address.Region.InvalidFormat",
                    "The region has invalid format."
                );
            }

            public static class Street
            {
                public static readonly Error NotEmpty = new(
                    "User.Address.Street.NotEmpty",
                    "The street cannot be empty."
                );

                public static readonly Error InvalidFormat = new(
                    "User.Address.Street.InvalidFormat",
                    "The street has invalid format."
                );
            }

            public static class ZipCode
            {
                public static readonly Error NotEmpty = new(
                    "User.Address.ZipCode.NotEmpty",
                    "The zip code cannot be empty."
                );

                public static readonly Error InvalidFormat = new(
                    "User.Address.ZipCode.InvalidFormat",
                    "The zip code has invalid format."
                );
            }
        }
        public static class PhoneNumber
        {
            public static readonly Error NullOrEmpty = new("User.PhoneNumber.NullOrEmpty", "The phone number cannot be empty");
            public static readonly Error InvalidFormat = new("User.PhoneNumber.InvalidFormat", "The format of phone number is invalid");
        }
    }
}