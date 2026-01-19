namespace Infrastructure.Media.Cloudinary;

public class CloudinaryPictureService : IPictureService
{
    private readonly CloudinaryDotNet.Cloudinary _cloudinary;

    public CloudinaryPictureService(IOptions<CloudinaryOptions> config)
    {
        var opts = config?.Value ?? throw new ArgumentNullException(nameof(config));
        if (string.IsNullOrWhiteSpace(opts.CloudName) || string.IsNullOrWhiteSpace(opts.APIKey) || string.IsNullOrWhiteSpace(opts.APISecret))
        {
            throw new ArgumentException("Cloudinary configuration is incomplete. Ensure CloudName, APIKey and APISecret are set in configuration.");
        }

        var account = new Account(opts.CloudName, opts.APIKey, opts.APISecret);
        _cloudinary = new CloudinaryDotNet.Cloudinary(account);
        // Ensure secure URLs
        _cloudinary.Api.Secure = true;
    }
    public async Task<ImageUploadResult> AddPictureAsync(IFormFile file)
    {
        var uploadResult = new ImageUploadResult();
        ArgumentNullException.ThrowIfNull(file);

        // basic validation
        if (file.Length <= 0)
        {
            uploadResult.Error = new Error() { Message = "Empty file." };
            return uploadResult;
        }

        if (file.Length > 5 * 1024 * 1024) // 5MB limit
        {
            uploadResult.Error = new Error() { Message = "File size exceeds the allowed limit of 5MB." };
            return uploadResult;
        }

        var permitted = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
        var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(ext) || !permitted.Contains(ext))
        {
            uploadResult.Error = new Error() { Message = "File type is not permitted." };
            return uploadResult;
        }

        try
        {
            using var stream = file.OpenReadStream();
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(file.FileName, stream),
                Transformation = new Transformation().Height(500).Width(500).Crop("fill").Gravity("face")
            };
            uploadResult = await _cloudinary.UploadAsync(uploadParams);

            return uploadResult;
        }
        catch
        {
            ImageUploadResult err = new()
            {
                Error = new Error { Message = "Image upload failed due to internal error." }
            };
            return err;
        }
    }

    public async Task<DeletionResult> DeletePictureAsync(string pictureId)
    {
        if (string.IsNullOrWhiteSpace(pictureId))
        {
            throw new ArgumentNullException(nameof(pictureId));
        }

        try
        {
            var deletionParams = new DeletionParams(pictureId);
            var result = await _cloudinary.DestroyAsync(deletionParams);
            return result;
        }
        catch
        {
            return new DeletionResult { Error = new Error { Message = "Image deletion failed due to internal error." } };
        }
    }
}
