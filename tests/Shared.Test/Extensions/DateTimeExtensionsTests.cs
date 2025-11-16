using Shared.Extensions;

namespace Shared.Test.Extensions;

public class DateTimeExtensionsTests
{
    [Fact]
    public void CaculateAge_ShouldReturnCorrectAge_ForValidInputs()
    {
        // Arrange
        int expectedAge = 25;
        DateTime dob = DateTime.UtcNow.AddYears(-expectedAge);

        // Act
        int age = dob.CalculateAge();
        
        // Assert
        Assert.Equal(expectedAge, age);
    }

    [Fact]
    public void CaculateAge_ShouldReturnZero_ForFutureDates()
    {
        // Arrange
        DateTime dob = DateTime.UtcNow.AddYears(1);
        
        // Act
        int age = dob.CalculateAge();

        // Assert
        Assert.Equal(0, age);
    }

    [Fact]
    public void CaculateAge_ShouldReturnCorrectAge_ForValidIncompleteYears()
    {
        // Arrange
        int expectedAge = 25;
        DateTime dob = DateTime.UtcNow.AddYears(-expectedAge).AddHours(1);

        // Act
        int age = dob.CalculateAge();

        // Assert
        Assert.Equal(expectedAge - 1, age);
    }
}
