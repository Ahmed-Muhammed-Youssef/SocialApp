using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;

namespace API.Application.Interfaces.Services
{
    public interface IPictureService
    {
        Task<ImageUploadResult> AddPictureAsync(IFormFile file);
        Task<DeletionResult> DeletePictureAsync(string photoId);
    }
}
