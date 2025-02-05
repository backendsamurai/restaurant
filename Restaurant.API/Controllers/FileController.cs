using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Storage;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("files")]
public sealed class FileController(IStorageService storageService) : ControllerBase
{
    private readonly IStorageService _storageService = storageService;

    [HttpPost("upload")]
    public async Task<Result> UploadFileToStorageAsync([FromForm] IFormFile file) =>
        await _storageService.UploadFile("test", file);
}