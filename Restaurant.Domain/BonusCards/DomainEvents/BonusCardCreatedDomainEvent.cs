using Restaurant.Domain.Core.Events;

namespace Restaurant.Domain.BonusCards.DomainEvents
{
    public sealed class BonusCardCreatedDomainEvent : IDomainEvent
    {
        internal BonusCardCreatedDomainEvent(BonusCard bonusCard) => BonusCard = bonusCard;

        public BonusCard BonusCard { get; }
    }
}