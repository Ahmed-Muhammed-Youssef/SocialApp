namespace Domain.MediaAggregate;

public class Picture : EntityBase, IAggregateRoot
{
    private Picture() { }

    public string Url { get; private set; } = string.Empty;
    public DateTime Created { get; private set; }
    public string PublicId { get; private set; } = string.Empty;

    public static Picture Create(string url, string publicId)
    {
        if (string.IsNullOrWhiteSpace(url))
        {
            throw new ArgumentException("Url is required", nameof(url));
        }

        if (string.IsNullOrWhiteSpace(publicId))
        {
            throw new ArgumentException("PublicId is required", nameof(publicId));
        }

        return new Picture
        {
            Url = url,
            PublicId = publicId,
            Created = DateTime.UtcNow
        };
    }
}
