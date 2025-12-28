using Domain.ApplicationUserAggregate;

namespace Domain.Test.ApplicationUserAggregate;

public class ApplicationUserTests
{
    [Fact]
    public void Constructor_ValidInput_CreatesUserWithCorrectProperties()
    {
        // Arrange
        var firstName = "John";
        var lastName = "Doe";
        var dateOfBirth = new DateTime(1990, 1, 1);
        var gender = Gender.Male;
        var cityId = 1;

        // Act
        var user = new ApplicationUser(firstName, lastName, dateOfBirth, gender, cityId);

        // Assert
        Assert.Equal(firstName, user.FirstName);
        Assert.Equal(lastName, user.LastName);
        Assert.Equal(dateOfBirth, user.DateOfBirth);
        Assert.Equal(gender, user.Gender);
        Assert.Equal(cityId, user.CityId);
        Assert.Equal(user.Created, user.LastActive);
        Assert.True(user.Created <= DateTime.UtcNow);
        Assert.True(user.LastActive <= DateTime.UtcNow);
    }

    [Fact]
    public void Constructor_EmptyFirstName_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new ApplicationUser("", "Doe", new DateTime(1990, 1, 1), Gender.Male, 1));
    }

    [Fact]
    public void Constructor_WhitespaceFirstName_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new ApplicationUser("   ", "Doe", new DateTime(1990, 1, 1), Gender.Male, 1));
    }

    [Fact]
    public void Constructor_NullFirstName_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new ApplicationUser(null!, "Doe", new DateTime(1990, 1, 1), Gender.Male, 1));
    }

    [Fact]
    public void Constructor_EmptyLastName_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new ApplicationUser("John", "", new DateTime(1990, 1, 1), Gender.Male, 1));
    }

    [Fact]
    public void Constructor_WhitespaceLastName_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new ApplicationUser("John", "   ", new DateTime(1990, 1, 1), Gender.Male, 1));
    }

    [Fact]
    public void Constructor_NullLastName_ThrowsArgumentException()
    {
        // Arrange & Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new ApplicationUser("John", null!, new DateTime(1990, 1, 1), Gender.Male, 1));
    }

    [Fact]
    public void Constructor_FutureDateOfBirth_ThrowsArgumentException()
    {
        // Arrange
        var futureDate = DateTime.UtcNow.AddDays(1);

        // Act & Assert
        Assert.Throws<ArgumentException>(() => 
            new ApplicationUser("John", "Doe", futureDate, Gender.Male, 1));
    }

    [Fact]
    public void Constructor_AllGenders_AreValid()
    {
        // Arrange & Act & Assert
        foreach (Gender gender in Enum.GetValues<Gender>())
        {
            var user = new ApplicationUser("John", "Doe", new DateTime(1990, 1, 1), gender, 1);
            Assert.Equal(gender, user.Gender);
        }
    }

    [Fact]
    public void AssociateWithIdentity_ValidIdentityId_SetsIdentityId()
    {
        // Arrange
        var user = new ApplicationUser("John", "Doe", new DateTime(1990, 1, 1), Gender.Male, 1);
        var identityId = "identity-123";

        // Act
        user.AssociateWithIdentity(identityId);

        // Assert
        Assert.Equal(identityId, user.IdentityId);
    }

    [Fact]
    public void AssociateWithIdentity_EmptyIdentityId_SetsIdentityId()
    {
        // Arrange
        var user = new ApplicationUser("John", "Doe", new DateTime(1990, 1, 1), Gender.Male, 1);
        var identityId = "";

        // Act
        user.AssociateWithIdentity(identityId);

        // Assert
        Assert.Equal(identityId, user.IdentityId);
    }

    [Fact]
    public void AssociateWithIdentity_AlreadyAssigned_ThrowsInvalidOperationException()
    {
        // Arrange
        var user = new ApplicationUser("John", "Doe", new DateTime(1990, 1, 1), Gender.Male, 1);
        user.AssociateWithIdentity("identity-1");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            user.AssociateWithIdentity("identity-2"));
    }

    [Fact]
    public void AssociateWithIdentity_WhitespaceIdentityId_ThrowsInvalidOperationException()
    {
        // Arrange
        var user = new ApplicationUser("John", "Doe", new DateTime(1990, 1, 1), Gender.Male, 1);
        user.AssociateWithIdentity("identity-1");

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => 
            user.AssociateWithIdentity("   "));
    }

    [Fact]
    public void Update_ValidInput_UpdatesProperties()
    {
        // Arrange
        var user = new ApplicationUser("John", "Doe", new DateTime(1990, 1, 1), Gender.Male, 1);
        var newFirstName = "Jane";
        var newLastName = "Smith";
        var newCityId = 2;
        var newBio = "New bio";
        var originalLastActive = user.LastActive;

        // Act
        Thread.Sleep(10); // Small delay to ensure LastActive changes
        user.Update(newFirstName, newLastName, newCityId, newBio);

        // Assert
        Assert.Equal(newFirstName, user.FirstName);
        Assert.Equal(newLastName, user.LastName);
        Assert.Equal(newCityId, user.CityId);
        Assert.Equal(newBio, user.Bio);
        Assert.True(user.LastActive > originalLastActive);
    }

    [Fact]
    public void Update_WithWhitespaceBio_TrimsBio()
    {
        // Arrange
        var user = new ApplicationUser("John", "Doe", new DateTime(1990, 1, 1), Gender.Male, 1);
        var bioWithWhitespace = "  Bio with spaces  ";

        // Act
        user.Update("John", "Doe", 1, bioWithWhitespace);

        // Assert
        Assert.Equal("Bio with spaces", user.Bio);
    }

    [Fact]
    public void Update_WithNullBio_SetsBioToNull()
    {
        // Arrange
        var user = new ApplicationUser("John", "Doe", new DateTime(1990, 1, 1), Gender.Male, 1);
        user.Update("John", "Doe", 1, "Initial bio");

        // Act
        user.Update("John", "Doe", 1, null);

        // Assert
        Assert.Null(user.Bio);
    }

    [Fact]
    public void Update_WithEmptyBio_SetsBioToEmpty()
    {
        // Arrange
        var user = new ApplicationUser("John", "Doe", new DateTime(1990, 1, 1), Gender.Male, 1);

        // Act
        user.Update("John", "Doe", 1, "");

        // Assert
        Assert.Equal("", user.Bio);
    }

    [Fact]
    public void MarkActive_UpdatesLastActive()
    {
        // Arrange
        var user = new ApplicationUser("John", "Doe", new DateTime(1990, 1, 1), Gender.Male, 1);
        var originalLastActive = user.LastActive;

        // Act
        Thread.Sleep(10); // Small delay to ensure LastActive changes
        user.MarkActive();

        // Assert
        Assert.True(user.LastActive > originalLastActive);
        Assert.True(user.LastActive <= DateTime.UtcNow);
    }

    [Fact]
    public void MarkActive_MultipleCalls_UpdatesLastActiveEachTime()
    {
        // Arrange
        var user = new ApplicationUser("John", "Doe", new DateTime(1990, 1, 1), Gender.Male, 1);
        var firstActive = user.LastActive;

        // Act
        Thread.Sleep(10);
        user.MarkActive();
        var secondActive = user.LastActive;
        Thread.Sleep(10);
        user.MarkActive();
        var thirdActive = user.LastActive;

        // Assert
        Assert.True(secondActive > firstActive);
        Assert.True(thirdActive > secondActive);
    }

    [Fact]
    public void Constructor_SetsCreatedAndLastActiveToCurrentTime()
    {
        // Arrange
        var beforeCreation = DateTime.UtcNow;

        // Act
        var user = new ApplicationUser("John", "Doe", new DateTime(1990, 1, 1), Gender.Male, 1);
        var afterCreation = DateTime.UtcNow;

        // Assert
        Assert.True(user.Created >= beforeCreation);
        Assert.True(user.Created <= afterCreation);
        Assert.True(user.LastActive >= beforeCreation);
        Assert.True(user.LastActive <= afterCreation);
    }

    [Fact]
    public void Update_UpdatesLastActive()
    {
        // Arrange
        var user = new ApplicationUser("John", "Doe", new DateTime(1990, 1, 1), Gender.Male, 1);
        var originalLastActive = user.LastActive;

        // Act
        Thread.Sleep(10);
        user.Update("Jane", "Smith", 2, "Bio");

        // Assert
        Assert.True(user.LastActive > originalLastActive);
    }
}

