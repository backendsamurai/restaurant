using Restaurant.API.Models.Payment;
using Restaurant.API.Types;
using Restaurant.Domain;

namespace Restaurant.API.Services.Contracts;

public interface IPaymentService
{
    public Task<Result<List<Payment>>> GetPaymentsAsync();
    public Task<Result<Payment>> GetPaymentByIdAsync(Guid paymentId);
    public Task<Result<Payment>> CreatePaymentAsync(CreatePaymentModel createPaymentModel);
}