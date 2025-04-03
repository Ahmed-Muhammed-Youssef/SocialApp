using Application.DTOs.Pagination;
using Domain.Constants;

namespace Application.Test.DTOs.Pagination
{
    public class PaginationParamsTests
    {
        [Fact]
        public void DefaultValues()
        {
            // Arrange & Act
            PaginationParams paginationParams = new();

            // Assert
            Assert.Equal(0, paginationParams.SkipValue);
            Assert.Equal(1, paginationParams.PageNumber);
            Assert.Equal(10, paginationParams.ItemsPerPage);
        }

        [Fact]
        public void Invalid_ItemsPerPage_Uses_PageSizeMax()
        {
            // Arrange
            PaginationParams paginationParams = new()
            {
                // Act
                ItemsPerPage = -1
            };

            // Assert
            Assert.Equal(SystemPolicy.MaxPageSize, paginationParams.ItemsPerPage);
        }

        [Fact]
        public void ItemsPerPage_Bigger_Than_PageSizeMax_Uses_PageSizeMax()
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
        public void Invalid_PageNumber_Ignored()
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
