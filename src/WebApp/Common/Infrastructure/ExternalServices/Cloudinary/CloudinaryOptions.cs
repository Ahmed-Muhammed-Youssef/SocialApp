namespace Infrastructure.ExternalServices.Cloudinary;

public class CloudinaryOptions
{
    public required string CloudName { get; init; }
    public required string APIKey { get; init; }
    public required string APISecret { get; init; }
}
