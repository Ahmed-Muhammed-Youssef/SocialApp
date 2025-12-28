using Application.Common.Interfaces;
using Application.Features.Users;
using Domain.ApplicationUserAggregate;
using Microsoft.AspNetCore.Identity;
using NSubstitute;
using Shared.Constants;

namespace Application.Test.Helpers;

public static class TestHelpers
{
    public static ApplicationUser CreateTestUser(int id = 1, string identityId = "identity-1")
    {
        var user = new ApplicationUser("John", "Doe", new DateTime(1990, 1, 1), Gender.Male, 1);
        user.GetType().GetProperty("Id")!.SetValue(user, id);
        user.AssociateWithIdentity(identityId);
        return user;
    }

    public static UserDTO CreateTestUserDto(int id = 1)
    {
        return new UserDTO
        {
            Id = id,
            FirstName = "John",
            LastName = "Doe",
            Gender = Gender.Male,
            Age = 34,
            Created = DateTime.UtcNow,
            LastActive = DateTime.UtcNow,
            Bio = "Test bio"
        };
    }

    public static IUnitOfWork CreateMockUnitOfWork()
    {
        return Substitute.For<IUnitOfWork>();
    }

    public static ICurrentUserService CreateMockCurrentUserService(int userId = 1, string email = "test@test.com")
    {
        var service = Substitute.For<ICurrentUserService>();
        service.GetPublicId().Returns(userId);
        service.GetEmail().Returns(email);
        service.GetRoles().Returns([RolesNameValues.User]);
        return service;
    }

    public static IdentityUser CreateTestIdentityUser(string id = "identity-1", string email = "test@test.com")
    {
        return new IdentityUser
        {
            Id = id,
            UserName = email,
            Email = email,
            EmailConfirmed = true
        };
    }
}

