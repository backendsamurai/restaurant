using Amazon.S3;
using Amazon.S3.Model;
using Restaurant.API.Types;

namespace Restaurant.API.Storage;

public sealed class StorageService(IAmazonS3 s3Client, ILogger<StorageService> logger) : IStorageService
{
    private readonly IAmazonS3 _s3Client = s3Client;
    private readonly ILogger<StorageService> _logger = logger;

    public async Task<Result> UploadFile(string bucketName, IFormFile file)
    {
        // If bucket not exists create them
        await EnsureBucketExists(bucketName);

        // Generate file object name
        var objectName = GetFileObjectName(file.FileName);

        // Put object to storage
        return await PutObjectToStorage(bucketName, objectName, file);
    }

    private async Task EnsureBucketExists(string bucketName)
    {
        try
        {
            await _s3Client.EnsureBucketExistsAsync(bucketName);
            _logger.LogInformation("Minio: Create named bucket {@bucketName}", bucketName);
        }
        catch (BucketAlreadyOwnedByYouException)
        {
            _logger.LogWarning("Minio: bucket {@bucketName} already exists", bucketName);
        }
    }

    private string GetFileObjectName(string fileName)
    {
        var fileExtension = fileName.Split('.').ElementAtOrDefault(1);
        var objectName = $"{Guid.NewGuid()}.{(fileExtension is not null ? fileExtension : "")}";

        return objectName;
    }

    private async Task<Result> PutObjectToStorage(string bucketName, string objectName, IFormFile file)
    {
        try
        {
            using var readStream = file.OpenReadStream();

            var putRequest = new PutObjectRequest
            {
                BucketName = bucketName,
                Key = objectName,
                InputStream = readStream
            };

            var res = await _s3Client.PutObjectAsync(putRequest);

            return Result.Created();
        }
        catch (Exception e)
        {
            _logger.LogError("Error while uploading file to Minio storage: {@ErrMsg}", e.Message);

            return Result.Error(
                code: "STS-000-001",
                type: "upload_file_error",
                message: "Error while uploading file to storage",
                detail: "Check provided data and try again"
            );
        }
    }
}