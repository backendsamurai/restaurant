using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Payment.GetPaymentById;

public sealed class GetPaymentByIdQueryHandler(IPaymentService paymentService) : IRequestHandler<GetPaymentByIdQuery, Result<Domain.Payment>>
{
    public async Task<Result<Domain.Payment>> Handle(GetPaymentByIdQuery request, CancellationToken cancellationToken) =>
        await paymentService.GetPaymentByIdAsync(request.PaymentId);
}