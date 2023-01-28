using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace BlogLab.Services
{
    public interface IPhotoService
    {
        public Task<ImageUploadResult> AddPhotoAsync(IFormFile formFile);
        public Task<DeletionResult> DeletePhotoAsync(string publicId);
    }
}