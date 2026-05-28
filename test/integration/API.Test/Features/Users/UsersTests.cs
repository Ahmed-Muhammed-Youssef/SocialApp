using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using API.Features.Auth.Responses;
using API.Test.Infrastructure;
using Application.Features.Users;

namespace API.Test.Features.Users;

public sealed class UsersTests(WebAppFactory webAppFactory) : IntegrationTestFixture(webAppFactory)
{
    [Fact]
    public async Task GetById_ShouldSucceed_WithAuthenticatedUser()
    {
        // Arrange
        HttpClient client = await CreateAuthenticatedClientAsync();
        UserDTO userDTO = await GetAuthenticatedUserDataAsync();
        // Act
        var response = await client.GetAsync(Routes.Users.GetById(userDTO.Id));
        response.EnsureSuccessStatusCode();
        UserDTO? getUserByIdResponse = await response.Content.ReadFromJsonAsync<UserDTO>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(getUserByIdResponse);

        Assert.Equal(userDTO.Id, getUserByIdResponse.Id);
        Assert.Equal(userDTO.FirstName, getUserByIdResponse.FirstName);
        Assert.Equal(userDTO.LastName, getUserByIdResponse.LastName);
        Assert.Equal(userDTO.Gender, getUserByIdResponse.Gender);
        Assert.Equal(userDTO.Created, getUserByIdResponse.Created);
        Assert.Equal(userDTO.Bio, getUserByIdResponse.Bio);
        Assert.Equal(userDTO.Age, getUserByIdResponse.Age);
    }
}
