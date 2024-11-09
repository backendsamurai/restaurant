using Restaurant.API.Types;

namespace Restaurant.API.Storage;

public interface IStorageService
{
    public Task<Result> UploadFile(string bucketName, IFormFile file);
}