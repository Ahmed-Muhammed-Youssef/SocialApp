using API.Domain.Configuration;
using API.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace API.Infrastructure.Test.Services
{
    public class PictureServiceTest
    {
        private readonly PictureService _pictureService;
        public PictureServiceTest()
        {
            var configuration = new ConfigurationBuilder()
            .AddUserSecrets<PictureServiceTest>()
            .Build();
            CloudinarySettings cloudinarySettings = new CloudinarySettings() { 
                CloudName = configuration["Cloudinary:CloudName"],
                APIKey = configuration["Cloudinary:APIKey"],
                APISecret = configuration["Cloudinary:APISecret"]
            };
            _pictureService = new PictureService(Options.Create(cloudinarySettings));
        }
        [Fact]
        public void AddPictureAsync_Should_Throw_NullReferenceException_If_IForm_Is_Null()
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _pictureService.AddPictureAsync(null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void DeletePictureAsync_Should_Throw_Exception_If_ID_Is_Null_Or_Empty(string pictureId)
        {
            Assert.ThrowsAsync<NullReferenceException>(async () => await _pictureService.DeletePictureAsync(pictureId));
        }
    }
}