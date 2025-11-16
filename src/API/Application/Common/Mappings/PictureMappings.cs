namespace Application.Common.Mappings;

public static class PictureMappings
{
    public static PictureDTO ToDto(Picture picture) => new(picture.Id, picture.Url);
}