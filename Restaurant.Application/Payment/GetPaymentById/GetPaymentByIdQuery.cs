using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Payment.GetPaymentById;

public sealed class GetPaymentByIdQuery : IRequest<Result<Domain.Payment>>
{
    public Guid PaymentId { get; set; }
}