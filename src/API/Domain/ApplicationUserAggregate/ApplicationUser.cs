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
    public ICollection<UserPicture> Pictures { get; private set; } = [];

    private ApplicationUser() { } 

    public ApplicationUser(string firstName, string lastName, DateTime dateOfBirth, Gender gender, int cityId)
    {
        if (string.IsNullOrWhiteSpace(firstName)) throw new ArgumentException("First name is required.");
        if (string.IsNullOrWhiteSpace(lastName)) throw new ArgumentException("Last name is required.");
        if (dateOfBirth > DateTime.UtcNow) throw new ArgumentException("Date of birth cannot be in the future.");

        FirstName = firstName;
        LastName = lastName;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        CityId = cityId;

        Created = DateTime.UtcNow;
        LastActive = Created;
    }

    public void AssociateWithIdentity(string identityId)
    {
        if(!string.IsNullOrWhiteSpace(IdentityId)) throw new InvalidOperationException("Identity cannot be changed once assigned.");
        IdentityId = identityId;
    }

    public void Update(string firstName, string lastName, int cityId, string? bio)
    {
        FirstName = firstName;
        LastName = lastName;
        Bio = bio?.Trim();
        CityId = cityId;
        MarkActive();
    }

    public void MarkActive()
    {
        LastActive = DateTime.UtcNow;
    }
}
