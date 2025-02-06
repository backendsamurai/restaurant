using FluentValidation;
using Restaurant.API.Entities;
using Restaurant.API.Models.Payment;
using Restaurant.API.Repositories;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Services.Implementations;

public sealed class PaymentService(IRepository<Payment> paymentRepository, IValidator<CreatePaymentModel> validator) : IPaymentService
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

        var payment = await paymentRepository.AddAsync(
            new Payment { Bill = createPaymentModel.Bill, Tip = createPaymentModel.Tip }
        );

        if (payment is null)
            return DetailedError.CreatingProblem("Cannot create payment");

        return Result.Created(payment);
    }
}