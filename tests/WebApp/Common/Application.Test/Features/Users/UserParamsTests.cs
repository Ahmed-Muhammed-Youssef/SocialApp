using Application.Features.Users;
using Domain.ApplicationUserAggregate;
using Domain.Common.Constants;

namespace Application.Test.Features.Users;

public class UserParamsTests
{
    [Fact]
    public void Constructor_DefaultInput_SetsPropertiesCorrectly()
    {
        // Arrange
        UserParams userParams = new();
        // Act

        // Assert
        Assert.Equal(SystemPolicy.UsersMinimumAge, userParams.MinAge);
        Assert.Equal(OrderByOptions.LastActive, userParams.OrderBy);
        Assert.Null(userParams.MaxAge);
    }
}
