using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SangtuariCareerCompass.Services
{
    public class CloudflareR2Service : IStorageService
    {
        private readonly IAmazonS3 _s3Client;
        private readonly string _bucketName;

        public CloudflareR2Service(IConfiguration config)
        {
            var r2Config = config.GetSection("CloudflareR2");
            _bucketName = r2Config["BucketName"];

            var s3Config = new AmazonS3Config
            {
                ServiceURL = r2Config["Endpoint"],
                AuthenticationRegion = "auto" // Wajib "auto" untuk R2
            };

            _s3Client = new AmazonS3Client(r2Config["AccessKey"], r2Config["SecretKey"], s3Config);
        }

        public async Task<string> UploadSignatureAsync(IFormFile file, string oldObjectKey = null)
        {
            // Hapus file lama jika ada agar tidak yatim (orphan)
            if (!string.IsNullOrEmpty(oldObjectKey))
            {
                await DeleteFileAsync(oldObjectKey);
            }

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var objectKey = $"signatures/{Guid.NewGuid()}{extension}";

            using var stream = file.OpenReadStream();
            var request = new PutObjectRequest
            {
                BucketName = _bucketName,
                Key = objectKey,
                InputStream = stream,
                ContentType = file.ContentType,
                DisablePayloadSigning = true // Optimasi performa untuk R2
            };

            await _s3Client.PutObjectAsync(request);
            return objectKey;
        }

        public async Task DeleteFileAsync(string objectKey)
        {
            if (string.IsNullOrEmpty(objectKey)) return;

            var request = new DeleteObjectRequest
            {
                BucketName = _bucketName,
                Key = objectKey
            };
            await _s3Client.DeleteObjectAsync(request);
        }

        public string GeneratePresignedUrl(string objectKey, int expiryMinutes = 60)
        {
            if (string.IsNullOrEmpty(objectKey)) return null;

            var request = new GetPreSignedUrlRequest
            {
                BucketName = _bucketName,
                Key = objectKey,
                Expires = DateTime.UtcNow.AddMinutes(expiryMinutes),
                Protocol = Protocol.HTTPS
            };

            return _s3Client.GetPreSignedURL(request);
        }
    }
}