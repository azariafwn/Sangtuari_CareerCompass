using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace SangtuariCareerCompass.Services
{
    public interface IStorageService
    {
        Task<string> UploadSignatureAsync(IFormFile file, string oldObjectKey = null);
        Task DeleteFileAsync(string objectKey);
        string GeneratePresignedUrl(string objectKey, int expiryMinutes = 60);
    }
}