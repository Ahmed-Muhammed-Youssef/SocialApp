namespace Domain.ApplicationUserAggregate;

public class ApplicationUser : EntityBase, IAggregateRoot
{
    public string IdentityId { get; private set; } = string.Empty;
    public string FirstName { get; private set; } = string.Empty;
    public string LastName { get; private set; } = string.Empty;
    public int? ProfilePictureId { get; private set; }
    public Gender Gender { get; private set; }
    public DateTime DateOfBirth { get; private set; }
    public DateTime Created { get; private set; }
    public DateTime LastActive { get; private set; }
    public string? Bio { get; private set; }
    public int CityId { get; private set; }

    private ApplicationUser() { } 

    public ApplicationUser(string identityId, string firstName, string lastName, DateTime dateOfBirth, Gender gender, int cityId, int? profilePictureId = null)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required.");
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required.");
        if (dateOfBirth > DateTime.UtcNow) throw new ArgumentException("Date of birth cannot be in the future.");

        IdentityId = identityId;
        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        CityId = cityId;
        ProfilePictureId = profilePictureId;

        Created = DateTime.UtcNow;
        LastActive = Created;
    }

    public void Update(string firstName, string lastName, int cityId, string? bio, int? profilePictureId = null)
    {
        FirstName = firstName;
        LastName = lastName;
        Bio = bio?.Trim();
        CityId = cityId;
        ProfilePictureId = profilePictureId;
        MarkActive();
    }

    public void MarkActive()
    {
        LastActive = DateTime.UtcNow;
    }
}
