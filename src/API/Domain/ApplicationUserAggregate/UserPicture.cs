namespace Domain.ApplicationUserAggregate;

public class UserPicture : EntityBase
{
    public required int UserId { get; set; }
    public required int PictureId { get; set; }
}
