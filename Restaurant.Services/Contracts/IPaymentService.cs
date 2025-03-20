using Restaurant.Domain;
using Restaurant.Shared.Common;
using Restaurant.Shared.Models.Payment;

namespace Restaurant.Services.Contracts;

public interface IPaymentService
{
    public Task<Result<List<Payment>>> GetPaymentsAsync();
    public Task<Result<Payment>> GetPaymentByIdAsync(Guid paymentId);
    public Task<Result<Payment>> CreatePaymentAsync(CreatePaymentModel createPaymentModel);
}