using Restaurant.Domain.BonusCards;
using Restaurant.Domain.Core.Abstractions;
using Restaurant.Domain.Core.Guards;
using Restaurant.Domain.Core.Primitives;
using Restaurant.Domain.Users.DomainEvents;

namespace Restaurant.Domain.Users
{
    public sealed class User : AggregateRoot, IAuditableEntity
    {
        private string _passwordHash;
        private readonly List<LikedProduct> _likedProducts = [];

        public FirstName FirstName { get; private set; }
        public LastName LastName { get; private set; }
        public string FullName => $"{FirstName} {LastName}";
        public Email Email { get; private set; }
        public DateOfBirth DateOfBirth { get; private set; }
        public BonusCard BonusCard { get; set; }
        public Address Address { get; set; } = Address.Empty;
        public IReadOnlyCollection<LikedProduct> LikedProducts => _likedProducts.AsReadOnly();

        private User(
            FirstName firstName,
            LastName lastName,
            Email email,
            DateOfBirth dateOfBirth,
            BonusCard bonusCard,
            string passwordHash
        ) : base(Guid.NewGuid())
        {
            Ensure.NotNull(firstName, "The first name is required.", nameof(firstName));
            Ensure.NotNull(lastName, "The last name is required.", nameof(lastName));
            Ensure.NotNull(email, "The email is required.", nameof(email));
            Ensure.NotNull(bonusCard, "The bonus card is required.", nameof(bonusCard));
            Ensure.NotNull(passwordHash, "The password hash is required.", nameof(passwordHash));

            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DateOfBirth = dateOfBirth;
            BonusCard = bonusCard;
            _passwordHash = passwordHash;
        }

        public static User Create(
            FirstName firstName,
            LastName lastName,
            Email email,
            DateOfBirth dateOfBirth,
            BonusCard bonusCard,
            string passwordHash)
        {
            var user = new User(firstName, lastName, email, dateOfBirth, bonusCard, passwordHash);

            user.AddDomainEvent(new UserCreatedDomainEvent(user));

            return user;
        }

        public bool ChangePassword(string newPasswordHash)
        {
            Ensure.NotEmpty(newPasswordHash, "The password hash is required.", nameof(newPasswordHash));

            if (newPasswordHash == _passwordHash)
                return false;

            _passwordHash = newPasswordHash;
            return true;
        }

        public void AddAddress(Address address)
        {
            if (address == Address.Empty)
                throw new ArgumentException("The address is required.", nameof(address));

            if (Address != Address.Empty)
                throw new InvalidOperationException("Cannot set address value because it already has a value.");

            Address = address;
        }

        public void UpdateAddress(Address address)
        {
            if (address == Address.Empty)
                throw new ArgumentException("The address is required.", nameof(address));

            Address = address;
        }

        public void RemoveAddress() => Address = Address.Empty;

        public void AddLikedProduct(LikedProduct likedProduct) => _likedProducts.Add(likedProduct);

        public void RemoveLikedProduct(LikedProduct likedProduct) => _likedProducts.Remove(likedProduct);

        public DateTime CreatedOnUTC { get; }
        public DateTime? ModifiedOnUTC { get; }
    }
}