using Shared.Results;

namespace Shared.Test.Results;

public class ResultTests
{
    [Fact]
    public void Success_ReturnsOkAndIsSuccessTrue()
    {
        // Act
        var result = Result<int>.Success(42);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.Ok, result.Status);
        Assert.Equal(42, result.Value);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Success_WithMessage_SetsSuccessMessage()
    {
        // Act
        var result = Result<int>.Success(42, "All good");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal("All good", result.SuccessMessage);
    }

    [Fact]
    public void Created_ReturnsCreatedAndIsSuccessTrue()
    {
        // Act
        var result = Result<int>.Created(1, "/api/things/1");

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.Created, result.Status);
        Assert.Equal(1, result.Value);
        Assert.Equal("/api/things/1", result.Location);
    }

    [Fact]
    public void NoContent_ReturnsNoContentAndIsSuccessTrue()
    {
        // Act
        var result = Result<object?>.NoContent();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(ResultStatus.NoContent, result.Status);
    }

    [Fact]
    public void Error_ReturnsErrorAndIsSuccessFalse()
    {
        // Act
        var result = Result<int>.Error("Something went wrong");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.Error, result.Status);
        Assert.Contains("Something went wrong", result.Errors);
    }

    [Fact]
    public void NotFound_WithoutMessages_ReturnsNotFoundAndIsSuccessFalse()
    {
        // Act
        var result = Result<int>.NotFound();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void NotFound_WithMessages_SetsErrors()
    {
        // Act
        var result = Result<int>.NotFound("Not found", "Try again");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.NotFound, result.Status);
        Assert.Equal(["Not found", "Try again"], result.Errors);
    }

    [Fact]
    public void Forbidden_ReturnsForbiddenAndIsSuccessFalse()
    {
        // Act
        var result = Result<int>.Forbidden("Not allowed");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.Forbidden, result.Status);
        Assert.Contains("Not allowed", result.Errors);
    }

    [Fact]
    public void Unauthorized_ReturnsUnauthorizedAndIsSuccessFalse()
    {
        // Act
        var result = Result<int>.Unauthorized("Not signed in");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.Unauthorized, result.Status);
        Assert.Contains("Not signed in", result.Errors);
    }

    [Fact]
    public void Conflict_ReturnsConflictAndIsSuccessFalse()
    {
        // Act
        var result = Result<int>.Conflict("Already exists");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.Conflict, result.Status);
        Assert.Contains("Already exists", result.Errors);
    }

    [Fact]
    public void CriticalError_ReturnsCriticalErrorAndIsSuccessFalse()
    {
        // Act
        var result = Result<int>.CriticalError("Unrecoverable");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.CriticalError, result.Status);
        Assert.Contains("Unrecoverable", result.Errors);
    }

    [Fact]
    public void Unavailable_ReturnsUnavailableAndIsSuccessFalse()
    {
        // Act
        var result = Result<int>.Unavailable("Try later");

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal(ResultStatus.Unavailable, result.Status);
        Assert.Contains("Try later", result.Errors);
    }

    [Fact]
    public void IsSuccess_ForOk_ReturnsTrue()
    {
        Assert.True(Result<int>.Success(1).IsSuccess);
    }

    [Fact]
    public void IsSuccess_ForCreated_ReturnsTrue()
    {
        Assert.True(Result<int>.Created(1).IsSuccess);
    }

    [Fact]
    public void IsSuccess_ForNoContent_ReturnsTrue()
    {
        Assert.True(Result<object?>.NoContent().IsSuccess);
    }

    [Theory]
    [InlineData(ResultStatus.Error)]
    [InlineData(ResultStatus.NotFound)]
    [InlineData(ResultStatus.Forbidden)]
    [InlineData(ResultStatus.Unauthorized)]
    [InlineData(ResultStatus.Conflict)]
    [InlineData(ResultStatus.CriticalError)]
    [InlineData(ResultStatus.Unavailable)]
    public void IsSuccess_ForFailureStatuses_ReturnsFalse(ResultStatus status)
    {
        // Arrange
        Result<int> result = status switch
        {
            ResultStatus.Error => Result<int>.Error("e"),
            ResultStatus.NotFound => Result<int>.NotFound(),
            ResultStatus.Forbidden => Result<int>.Forbidden(),
            ResultStatus.Unauthorized => Result<int>.Unauthorized(),
            ResultStatus.Conflict => Result<int>.Conflict(),
            ResultStatus.CriticalError => Result<int>.CriticalError(),
            ResultStatus.Unavailable => Result<int>.Unavailable(),
            _ => throw new ArgumentOutOfRangeException(nameof(status))
        };

        // Assert
        Assert.False(result.IsSuccess);
    }
}
