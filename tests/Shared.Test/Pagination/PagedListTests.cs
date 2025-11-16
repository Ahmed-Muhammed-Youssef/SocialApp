using Shared.Pagination;

namespace Shared.Test.Pagination;

public class PagedListTests
{
    [Fact]
    public void Constructor_ValidInputs_SetsPropertiesCorrectly()
    {
        // Arrange
        var items = new List<int> { 1, 2 };

        // Act
        var pagedList = new PagedList<int>(items, 10, 1, 5);

        // Assert
        Assert.Equal(items, pagedList.Items); // Same reference
        Assert.Equal(10, pagedList.Count);
        Assert.Equal(1, pagedList.CurrentPage);
        Assert.Equal(5, pagedList.ItemsPerPage);
        Assert.Equal(2, pagedList.TotalPages); // 10 / 5 = 2 pages
    }

    [Fact]
    public void Constructor_NullItems_InitializesEmptyList()
    {
        // Act
        var pagedList = new PagedList<int>(null!, 0, 1, 10);

        // Assert
        Assert.NotNull(pagedList.Items);
        Assert.Empty(pagedList.Items);
        Assert.Equal(0, pagedList.Count);
        Assert.Equal(1, pagedList.CurrentPage);
        Assert.Equal(10, pagedList.ItemsPerPage);
        Assert.Equal(0, pagedList.TotalPages); // 0 / 10 = 0 pages
    }

    [Fact]
    public void Constructor_EmptyItems_SetsEmptyList()
    {
        // Arrange
        var items = new List<int>();

        // Act
        var pagedList = new PagedList<int>(items, 0, 1, 10);

        // Assert
        Assert.Equal(items, pagedList.Items); // Same reference
        Assert.Empty(pagedList.Items);
        Assert.Equal(0, pagedList.Count);
        Assert.Equal(1, pagedList.CurrentPage);
        Assert.Equal(10, pagedList.ItemsPerPage);
        Assert.Equal(0, pagedList.TotalPages);
    }

    [Fact]
    public void TotalPages_Calculation_RoundsUpCorrectly()
    {
        // Arrange & Act
        var pagedList = new PagedList<int>([1], 3, 1, 2);

        // Assert
        Assert.Equal(2, pagedList.TotalPages); // 3 / 2 = 1.5, rounds up to 2
    }

    [Fact]
    public void Constructor_CountLessThanItemsPerPage_SetsTotalPagesToOne()
    {
        // Arrange & Act
        var pagedList = new PagedList<int>([1], 1, 1, 10);

        // Assert
        Assert.Equal(1, pagedList.TotalPages); // 1 / 10 = 0.1, rounds up to 1
    }
}
