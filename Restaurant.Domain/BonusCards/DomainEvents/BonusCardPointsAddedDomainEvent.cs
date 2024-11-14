using Restaurant.Domain.Core.Events;

namespace Restaurant.Domain.BonusCards.DomainEvents
{
    public sealed class BonusCardPointsAddedDomainEvent : IDomainEvent
    {
        internal BonusCardPointsAddedDomainEvent(BonusCard bonusCard) => BonusCard = bonusCard;

        public BonusCard BonusCard { get; }
    }
}