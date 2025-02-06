using Microsoft.AspNetCore.Mvc;
using Restaurant.API.Storage;
using Restaurant.API.Types;

namespace Restaurant.API.Controllers;

[ApiController]
[Route("files")]
public sealed class FileController(IStorageService storageService) : ControllerBase
{
    [HttpPost("upload")]
    public async Task<Result> UploadFileToStorageAsync([FromForm] IFormFile file) =>
        await storageService.UploadFile("test", file);
}