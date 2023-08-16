using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace API.Interfaces
{
    public interface IPictureService
    {
        Task<ImageUploadResult> AddPictureAsync(IFormFile file);
        Task<DeletionResult> DeletePictureAsync(string photoId);
    }
}
