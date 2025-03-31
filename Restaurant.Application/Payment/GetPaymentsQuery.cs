using MediatR;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Payment;

public sealed record GetPaymentsQuery : IRequest<Result<List<Domain.Payment>>>;

public sealed class GetPaymentsQueryHandler(IPaymentService paymentService) : IRequestHandler<GetPaymentsQuery, Result<List<Domain.Payment>>>
{
    public async Task<Result<List<Domain.Payment>>> Handle(GetPaymentsQuery request, CancellationToken cancellationToken) =>
       await paymentService.GetPaymentsAsync();
}