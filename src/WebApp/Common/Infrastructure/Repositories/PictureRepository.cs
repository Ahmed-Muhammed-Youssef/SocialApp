using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Features.Pictures;
using Application.Common.Interfaces.Repositories;

namespace Infrastructure.Repositories;

public class PictureRepository(DataContext _dataContext) : IPictureRepository
{
    public async Task<Picture> AddPictureAsync(Picture picture)
    {
        await _dataContext.Pictures.AddAsync(picture);
        return picture;
    }

    public void DeletePicture(Picture picture)
    {
        _dataContext.Pictures.Remove(picture);
    }

    public async Task<List<Picture>> GetUserPictureAsync(int id)
    {
        var result = _dataContext.Pictures
           .AsNoTracking()
           .Where(p => p.AppUserId == id)
           .OrderBy(p => p.Created);
        return await result.ToListAsync();
    }

    public async Task<List<PictureDTO>> GetUserPictureDTOsAsync(int id)
    {
        var result = await _dataContext.Pictures
            .AsNoTracking()
            .Where(p => p.AppUserId == id)
            .Select(p => new PictureDTO(p.Id, p.Url))
            .ToListAsync();

        return result;
    }
    public void UpdatePicture(Picture picture)
    {
        _dataContext.Entry(picture).State = EntityState.Modified;
    }
}
