using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Restaurant.Application.Consumers.Commands;
using Restaurant.Application.Consumers.Queries;
using Restaurant.Domain;
using Restaurant.Services.DTOs.Consumer;
using Wolverine;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("v1/consumers")]
[EndpointGroupName("consumers")]
public sealed class ConsumersController : ControllerBase
{
	private readonly IMessageBus _messageBus;

	public ConsumersController(IMessageBus messageBus)
	{
		_messageBus = messageBus;
	}

	[TranslateResultToActionResult]
	[HttpGet("{consumerId:guid}")]
	public async Task<Result<Consumer>> GetConsumerAsync([FromRoute] Guid consumerId) =>
		await _messageBus.InvokeAsync<Result<Consumer>>(new GetConsumerByIdQuery(consumerId));

	[TranslateResultToActionResult]
	[HttpPost]
	public async Task<Result<Consumer>> CreateConsumerAsync([FromBody] CreateConsumerDTO dto) =>
		await _messageBus.InvokeAsync<Result<Consumer>>(new CreateConsumerCommand(dto));

	[TranslateResultToActionResult]
	[HttpPatch("{consumerId:guid}")]
	public async Task<Result<Consumer>> UpdateConsumerAsync([FromRoute] Guid consumerId, [FromBody] UpdateConsumerDTO dto) =>
		await _messageBus.InvokeAsync<Result<Consumer>>(new UpdateConsumerCommand(consumerId, dto));

	[TranslateResultToActionResult]
	[HttpDelete("{consumerId:guid}")]
	public async Task<Result> RemoveConsumerAsync([FromRoute] Guid consumerId) =>
		await _messageBus.InvokeAsync<Result>(new RemoveConsumerCommand(consumerId));
}
