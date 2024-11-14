using Restaurant.Domain.BonusCards.DomainEvents;
using Restaurant.Domain.Core.Abstractions;
using Restaurant.Domain.Core.Guards;
using Restaurant.Domain.Core.Primitives;

namespace Restaurant.Domain.BonusCards
{
    public sealed class BonusCard : AggregateRoot, IAuditableEntity
    {
        public BonusCardNumber CardNumber { get; private set; }

        public int Points { get; private set; }

        private BonusCard(BonusCardNumber bonusCardNumber) : base(Guid.NewGuid())
        {
            Ensure.NotNull(bonusCardNumber, "The bonus card number is required.", nameof(bonusCardNumber));

            CardNumber = bonusCardNumber;
            Points = 0;
        }

        public static BonusCard Create(BonusCardNumber bonusCardNumber)
        {
            var bonusCard = new BonusCard(bonusCardNumber);

            bonusCard.AddDomainEvent(new BonusCardCreatedDomainEvent(bonusCard));

            return bonusCard;
        }

        public void AddPoints(int points)
        {
            Points += points;
            AddDomainEvent(new BonusCardPointsAddedDomainEvent(this));
        }

        public void SubtractPoints(int points)
        {
            Points -= points;
            AddDomainEvent(new BonusCardPointsSubtractedDomainEvent(this));
        }

        public DateTime CreatedOnUTC { get; }

        public DateTime? ModifiedOnUTC { get; }
    }
}