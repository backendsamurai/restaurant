using Restaurant.Domain.Core.Primitives;

namespace Restaurant.Domain.Core.Exceptions
{
    public class DomainException(Error error)
        : Exception(error.Message)
    {
        public Error Error { get; } = error;
    }
}