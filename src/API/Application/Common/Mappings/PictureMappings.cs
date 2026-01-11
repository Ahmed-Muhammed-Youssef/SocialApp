namespace Application.Common.Mappings;

public static class PictureMappings
{
    public static PictureDTO ToDto(Picture picture) => new(picture.Id, picture.Url);
    public static Expression<Func<Picture, PictureDTO>> ToDtoExpression => picture => new(picture.Id, picture.Url);
}