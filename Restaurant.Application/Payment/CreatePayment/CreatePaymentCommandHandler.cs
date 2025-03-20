using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Payment;

namespace Restaurant.Application.Payment.CreatePayment;

public sealed class CreatePaymentCommandHandler(IPaymentService paymentService) : IRequestHandler<CreatePaymentCommand, Result<Domain.Payment>>
{
    public async Task<Result<Domain.Payment>> Handle(CreatePaymentCommand request, CancellationToken cancellationToken) =>
        await paymentService.CreatePaymentAsync(new CreatePaymentModel(request.OrderId, request.Bill, request.Tip));
}