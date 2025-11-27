namespace Application.Common.Mappings;

public static class PostMappings
{
    public static Expression<Func<Post, PostDTO>> ToPostDTOExpression = p => new PostDTO
    {
        Id = p.Id,
        OwnerName = p.ApplicationUser!.FirstName + " " + p.ApplicationUser!.LastName,
        OwnerId = p.UserId,
        Content = p.Content,
        OwnerPictureUrl = "",
        DateEdited = p.DateEdited,
        DatePosted = p.DatePosted
    };

}
