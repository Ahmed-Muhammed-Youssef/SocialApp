using Application.DTOs.Pagination;
using Domain.Constants;

namespace Application.Test.DTOs.Pagination
{
    public class PaginationParamsTests
    {
        [Fact]
        public void Constructor_DefaultInput_SetsPropertiesCorrectly()
        {
            // Arrange & Act
            PaginationParams paginationParams = new();

            // Assert
            Assert.Equal(0, paginationParams.SkipValue);
            Assert.Equal(1, paginationParams.PageNumber);
            Assert.Equal(10, paginationParams.ItemsPerPage);
        }

        [Theory]
        [InlineData(2, 10)]
        public void SkipValue_Calculation_SetsValueCorrectly(int pageNumber, int itemsPerPage)
        {
            // Arrange
            PaginationParams paginationParams = new()
            {
                // Act
                PageNumber = pageNumber,
                ItemsPerPage = itemsPerPage
            };

            // Assert
            Assert.Equal(10, paginationParams.SkipValue);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(0)]
        public void ItemsPerPage_InvalidInput_UsesPageSizeMax(int itemsPerPage)
        {
            // Arrange
            PaginationParams paginationParams = new()
            {
                // Act
                ItemsPerPage = itemsPerPage
            };

            // Assert
            Assert.Equal(SystemPolicy.MaxPageSize, paginationParams.ItemsPerPage);
        }

        [Fact]
        public void ItemsPerPage_InputBiggerThanPageSizeMax_UsesPageSizeMax()
        {
            // Arrange
            PaginationParams paginationParams = new()
            {
                // Act
                ItemsPerPage = SystemPolicy.MaxPageSize + 1
            };

            // Assert
            Assert.Equal(SystemPolicy.MaxPageSize, paginationParams.ItemsPerPage);
        }

        [Fact]
        public void PageNumber_InvalidInput_IgnoreNewValue()
        {
            // Arrange
            PaginationParams paginationParams = new()
            {
                // Act
                PageNumber = -1
            };

            // Assert
            Assert.Equal(1, paginationParams.PageNumber);
        }
    }
}
