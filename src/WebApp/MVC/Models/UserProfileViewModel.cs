namespace MVC.Models;

public class UserProfileViewModel
{
    public int Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public char Sex { get; set; }
    public int Age { get; set; }
    public DateTime Created { get; set; }
    public DateTime LastActive { get; set; }
    public string? Bio { get; set; }
    public SocialRelation Relation { get; set; }
}
