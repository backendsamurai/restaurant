using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Entities;
using Restaurant.API.Models.Payment;
using Restaurant.API.Services.Contracts;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("payments")]
public sealed class PaymentController(IPaymentService paymentService) : ControllerBase
{
    private readonly IPaymentService _paymentService = paymentService;

    [HttpGet]
    public async Task<Result<List<Payment>>> GetPayments() =>
        await _paymentService.GetPaymentsAsync();

    [HttpGet("{paymentId:guid}")]
    public async Task<Result<Payment>> GetPaymentById([FromRoute(Name = "paymentId")] Guid paymentId) =>
        await _paymentService.GetPaymentByIdAsync(paymentId);

    [HttpPost]
    public async Task<Result<Payment>> CreatePayment([FromBody] CreatePaymentModel createPaymentModel) =>
        await _paymentService.CreatePaymentAsync(createPaymentModel);
}