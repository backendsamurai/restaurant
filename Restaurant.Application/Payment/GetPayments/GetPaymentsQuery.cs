using MediatR;
using Restaurant.Shared.Common;

namespace Restaurant.Application.Payment.GetPayments;

public sealed class GetPaymentsQuery : IRequest<Result<List<Domain.Payment>>> { }