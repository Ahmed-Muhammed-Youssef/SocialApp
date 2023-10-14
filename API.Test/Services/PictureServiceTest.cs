using API.Helpers;
using API.Services;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NuGet.Configuration;
using System.Configuration;

namespace API.Test.Models
{
    public class PictureServiceTest
    {
        private readonly PictureService _pictureService;
        public PictureServiceTest()
        {
        }
        [Fact]
        public void AddPictureAsync_Should_Throw_Exception_If_IForm_Is_Null()
        {
            
        }
    }
}