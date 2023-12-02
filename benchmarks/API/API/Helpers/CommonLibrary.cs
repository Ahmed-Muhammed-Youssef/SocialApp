using API.Application.Interfaces;
using API.Application.Interfaces.Repositories;
using API.Infrastructure.Data;
using API.Infrastructure.MappingProfiles;
using API.Infrastructure.Repositories;
using API.Infrastructure.Repositories.CachedRepositories;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using System.Text.Json;

namespace API.Benchmark.Helpers
{
    public static class CommonLibrary
    {
        private static DataContext CreateDbContext() 
        {
            var options = new DbContextOptionsBuilder<DataContext>()
                            .UseSqlServer("Server=AHMED-YOUSSEF;Database=SocialApp;Trusted_Connection=True;TrustServerCertificate=True")
                            .Options;
            return new DataContext(options);
        }
        public static void PrintActionResult<T>(ActionResult<T> actionResult)
        {
            var resultProperty = actionResult.GetType().GetProperty("Result");
            if (resultProperty != null)
            {
                var resultValue = resultProperty.GetValue(actionResult);
                var valueProperty = resultValue?.GetType().GetProperty("Value");

                if (valueProperty != null)
                {
                    var content = valueProperty.GetValue(resultValue);
                    Console.WriteLine(JsonSerializer.Serialize(content));

                }
            }
            Console.WriteLine("Error");
        }
        public static IMapper CreateAutoMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                // profiles configured in the application
                cfg.AddProfile<AutoMapperProfiles>();
            });

           return configuration.CreateMapper();
        }
        public static UnitOfWork CreateUnitOfWork()
        {
            var dbContext = CreateDbContext();
            var mapper = CreateAutoMapper();
            var emptyCache = new MemoryCache(new MemoryCacheOptions());
            var userRepository = new CachedUserRepository(new UserRepository(dbContext), emptyCache);
            var pictureRepository = new PictureRepository(dbContext, mapper);
            var messageRepository = new MessageRepository(dbContext, mapper);
            var friendRequestsRepository = new FriendRequestsRepository(dbContext, mapper);
            return new UnitOfWork(dbContext, userRepository, pictureRepository, messageRepository, friendRequestsRepository);
        }
        public static DefaultHttpContext CreateControllerContext()
        {
            var token = "Bearer eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6ImFkbWluQHRlc3QiLCJuYW1laWQiOiIxIiwidW5pcXVlX25hbWUiOiJhZG1pbiIsInJvbGUiOlsiYWRtaW4iLCJtb2RlcmF0b3IiLCJ1c2VyIl0sIm5iZiI6MTcwMTQzNjU4NSwiZXhwIjoxNzAyMDQxMzg1LCJpYXQiOjE3MDE0MzY1ODV9.c_2MzXgD16zSgMw4GPrH5PMLB9H2B1-jNLi_ApSPSCWTcb7lLnwW9WMVIJCtJOkKUYv_VuGD8fcwvpnZkAY4Bg";
            DefaultHttpContext httpContext = new DefaultHttpContext();

            // Add authentication user to the context
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, "admin@test"),
                new Claim(ClaimTypes.Name, "admin"),
                new Claim(ClaimTypes.NameIdentifier, 1.ToString()),
                new Claim(ClaimTypes.Role, "admin"),
                new Claim(ClaimTypes.Role, "moderator"),
                new Claim(ClaimTypes.Role, "user")
            };
            var identity = new ClaimsIdentity(claims);
            httpContext.User = new ClaimsPrincipal(identity);
            httpContext.Request.Headers.Append("Authorization", token);
            return httpContext;
        }
    }
}
