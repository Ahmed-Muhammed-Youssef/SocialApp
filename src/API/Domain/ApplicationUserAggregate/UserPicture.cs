namespace Domain.ApplicationUserAggregate;

public class UserPicture : EntityBase
{
    private UserPicture() { }

    public UserPicture(int userId, int pictureId)
    {
        UserId = userId;
        PictureId = pictureId;
    }

    public int UserId { get; private set; }
    public int PictureId { get; private set; }
}
