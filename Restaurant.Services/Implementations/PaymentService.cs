using FluentValidation;
using Restaurant.Domain;
using Restaurant.Services.Contracts;
using Restaurant.Shared.Common;
using Restaurant.Shared.Database;
using Restaurant.Shared.Models.Payment;

namespace Restaurant.Services.Implementations;

public sealed class PaymentService(IRepository<Payment> paymentRepository, IRepository<Order> orderRepository, IValidator<CreatePaymentModel> validator) : IPaymentService
{
    public async Task<Result<List<Payment>>> GetPaymentsAsync() =>
        await paymentRepository.SelectAllAsync();

    public async Task<Result<Payment>> GetPaymentByIdAsync(Guid paymentId) =>
        Result.Success(await paymentRepository.SelectByIdAsync(paymentId)) ?? DetailedError.NotFound("Payment not found", "Provide correct payment ID");

    public async Task<Result<Payment>> CreatePaymentAsync(CreatePaymentModel createPaymentModel)
    {
        var validationResult = await validator.ValidateAsync(createPaymentModel);

        if (!validationResult.IsValid)
            return DetailedError.Invalid("One of field are not valid", "Check all fields and try again");

        var order = await orderRepository.WhereFirstAsync<Order>(o => o.Id == createPaymentModel.OrderId);

        if (order == null)
            return DetailedError.NotFound("Cannot found order with provided id!");


        var payment = await paymentRepository.AddAsync(new Payment(order, createPaymentModel.Bill, createPaymentModel.Tip));

        if (payment is null)
            return DetailedError.CreatingProblem("Cannot create payment");

        return Result.Created(payment);
    }
}