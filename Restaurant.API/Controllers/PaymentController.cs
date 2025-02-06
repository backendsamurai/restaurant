using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Models.Payment;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;
using Restaurant.Domain;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("payments")]
public sealed class PaymentController(IPaymentService paymentService) : ControllerBase
{
    [HttpGet]
    public async Task<Result<List<Payment>>> GetPayments() =>
        await paymentService.GetPaymentsAsync();

    [HttpGet("{paymentId:guid}")]
    public async Task<Result<Payment>> GetPaymentById([FromRoute(Name = "paymentId")] Guid paymentId) =>
        await paymentService.GetPaymentByIdAsync(paymentId);

    [HttpPost]
    public async Task<Result<Payment>> CreatePayment([FromBody] CreatePaymentModel createPaymentModel) =>
        await paymentService.CreatePaymentAsync(createPaymentModel);
}