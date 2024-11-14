using Restaurant.Domain.Core.Events;

namespace Restaurant.Domain.BonusCards.DomainEvents
{
    public sealed class BonusCardPointsSubtractedDomainEvent : IDomainEvent
    {
        internal BonusCardPointsSubtractedDomainEvent(BonusCard bonusCard) => BonusCard = bonusCard;

        public BonusCard BonusCard { get; }
    }
}