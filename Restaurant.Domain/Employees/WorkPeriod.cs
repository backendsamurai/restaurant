using Restaurant.Domain.Core.Primitives;

namespace Restaurant.Domain.Employees
{
    public sealed class WorkPeriod : ValueObject
    {
        public int YearsAtWork =>
            FinishedAt is not null
                ? FinishedAt?.Year - StartedAt.Year ?? 0
                : DateTime.UtcNow.Year - StartedAt.Year;

        public DateTime StartedAt { get; }
        public DateTime? FinishedAt { get; } = null;

        private WorkPeriod(DateTime startedAt, DateTime? finishedAt = null)
        {
            StartedAt = startedAt;
            FinishedAt = finishedAt;
        }

        public static WorkPeriod Create(DateTime startedAt, DateTime? finishedAt = null) => new(startedAt, finishedAt);

        protected override IEnumerable<object?> GetAtomicValues()
        {
            yield return StartedAt;
            yield return FinishedAt;
        }
    }
}