using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Payment.CreatePayment;
using Restaurant.Application.Payment.GetPaymentById;
using Restaurant.Application.Payment.GetPayments;
using Restaurant.Domain;
using Restaurant.Shared.Common;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("payments")]
public sealed class PaymentController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<Result<List<Payment>>> GetPayments() =>
        await mediator.Send(new GetPaymentsQuery());

    [HttpGet("{paymentId:guid}")]
    public async Task<Result<Payment>> GetPaymentById([FromRoute(Name = "paymentId")] Guid paymentId) =>
        await mediator.Send(new GetPaymentByIdQuery { PaymentId = paymentId });

    [HttpPost]
    public async Task<Result<Payment>> CreatePayment([FromBody] CreatePaymentCommand command) => await mediator.Send(command);
}