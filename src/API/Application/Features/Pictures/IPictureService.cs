namespace Application.Features.Pictures;

public interface IPictureService
{
    Task<ImageUploadResult> AddPictureAsync(IFormFile file);
    Task<DeletionResult> DeletePictureAsync(string photoId);
}
