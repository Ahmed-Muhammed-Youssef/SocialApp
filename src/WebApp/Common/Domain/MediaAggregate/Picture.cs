namespace Domain.MediaAggregate;

public class Picture : EntityBase, IAggregateRoot
{
    public required string Url { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public required string PublicId { get; set; }
}