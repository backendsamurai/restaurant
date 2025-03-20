using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Payment.CreatePayment;

public sealed class CreatePaymentCommand : IRequest<Result<Domain.Payment>>
{
    public Guid OrderId { get; set; }
    public decimal Bill { get; set; }
    public decimal? Tip { get; set; }
}