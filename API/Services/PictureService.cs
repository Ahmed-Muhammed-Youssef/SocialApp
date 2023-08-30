using API.Helpers;
using API.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace API.Services
{
    public class PictureService : IPictureService
    {
        private readonly Cloudinary _cloudinary;

        public PictureService(IOptions<CloudinarySettings> config)
        {
            var account = new Account(config.Value.CloudName, config.Value.APIKey, config.Value.APISecret);
            _cloudinary = new Cloudinary(account);
        }
        public async Task<ImageUploadResult> AddPictureAsync(IFormFile file)
        {
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using var stream = file.OpenReadStream();
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, stream),
                    Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
                };
                uploadResult = await _cloudinary.UploadAsync(uploadParams);
            }
            return uploadResult;
        }

        public async Task<DeletionResult> DeletePictureAsync(string publiId)
        {
            var deletionParams = new DeletionParams(publiId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result;
        }
    }
}
