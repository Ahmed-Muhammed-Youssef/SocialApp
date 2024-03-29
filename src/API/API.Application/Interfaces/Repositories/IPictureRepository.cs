﻿using API.Application.DTOs.Picture;
using API.Domain.Entities;

namespace API.Application.Interfaces.Repositories
{
    public interface IPictureRepository
    {
        public void DeletePicture(Picture picture);
        public Task<Picture> AddPictureAsync(Picture picture);
        public Task<IEnumerable<Picture>> GetUserPictureAsync(int id);
        public Task<IEnumerable<PictureDTO>> GetUserPictureDTOsAsync(int id);
        public void UpdatePicture(Picture picture);
    }
}
