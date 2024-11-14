using Restaurant.Domain.Core.Primitives;
using Restaurant.Domain.Core.Primitives.Result;

namespace Restaurant.Domain.Users
{
    public sealed class Address : ValueObject
    {
        public string Country { get; private set; }
        public string City { get; private set; }
        public string Region { get; private set; }
        public string Street { get; private set; }
        public string ZipCode { get; private set; }

        private Address(string country, string city, string region, string street, string zipCode)
        {
            Country = country;
            City = city;
            Region = region;
            Street = street;
            ZipCode = zipCode;
        }

        public static Result<Address> Create(
            string country,
            string city,
            string region,
            string street,
            string zipCode
        )
        {
            var countryResult = Result.Create(country, UserErrors.Address.Country.NotEmpty)
                .Ensure(c => !string.IsNullOrWhiteSpace(c), UserErrors.Address.Country.NotEmpty)
                .Ensure(c => char.IsUpper(c[0]), UserErrors.Address.Country.InvalidFormat);

            var cityResult = Result.Create(city, UserErrors.Address.City.NotEmpty)
                .Ensure(c => !string.IsNullOrWhiteSpace(c), UserErrors.Address.City.NotEmpty)
                .Ensure(c => char.IsUpper(c[0]), UserErrors.Address.City.InvalidFormat);

            var regionResult = Result.Create(region, UserErrors.Address.Region.NotEmpty)
                .Ensure(r => !string.IsNullOrWhiteSpace(r), UserErrors.Address.Region.NotEmpty)
                .Ensure(r => char.IsUpper(r[0]), UserErrors.Address.Region.InvalidFormat);

            var streetResult = Result.Create(street, UserErrors.Address.Street.NotEmpty)
                .Ensure(s => !string.IsNullOrWhiteSpace(s), UserErrors.Address.Street.NotEmpty)
                .Ensure(s => char.IsUpper(s[0]), UserErrors.Address.Street.InvalidFormat);

            var zipCodeResult = Result
                .Create(zipCode, UserErrors.Address.ZipCode.NotEmpty)
                .Ensure(z => !string.IsNullOrWhiteSpace(z), UserErrors.Address.ZipCode.NotEmpty)
                .Ensure(CheckZipCodeIsValid, UserErrors.Address.ZipCode.InvalidFormat);


            var result = Result.FirstFailureOrSuccess(
                countryResult, cityResult, regionResult, streetResult, zipCodeResult
            );

            if (result.IsFailure)
                return Result.Failure<Address>(result.Error);

            var address = new Address(country, city, region, street, zipCode);

            return Result.Success(address);
        }

        internal static Address Empty => new(
            country: string.Empty,
            city: string.Empty,
            region: string.Empty,
            street: string.Empty,
            zipCode: string.Empty
        );

        private static bool CheckZipCodeIsValid(string zipCode)
        {
            if (zipCode.Length < 5 || zipCode.Length > 5)
                return false;

            return zipCode.Where(char.IsDigit).Count() == zipCode.Length;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Country;
            yield return City;
            yield return Region;
            yield return Street;
        }
    }
}