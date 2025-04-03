using Application.DTOs.Pagination;
using Domain.Constants;
using Domain.Enums;

namespace Application.Test.DTOs.Pagination
{
    public class UserParamsTests
    {
        [Fact]
        public void Constructor_DefaultInput_SetsPropertiesCorrectly()
        {
            // Arrange
            UserParams userParams = new UserParams();
            // Act

            // Assert
            Assert.Equal(SystemPolicy.UsersMinimumAge, userParams.MinAge);
            Assert.Equal(OrderByOptions.LastActive, userParams.OrderBy);
            Assert.Null(userParams.MaxAge);
        }

        [Fact]
        public void MinAge_InvalidInput_Ignored()
        {
            // Arrange
            UserParams userParams = new()
            {
                // Act
                MinAge = SystemPolicy.UsersMinimumAge - 1
            };

            // Assert
            Assert.Equal(SystemPolicy.UsersMinimumAge, userParams.MinAge);
        }
    }
}
