using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Features.Pictures;
using Application.Common.Interfaces.Repositories;
using Application.Common.Mappings;

namespace Infrastructure.Repositories;

public class PictureRepository(DataContext dataContext) : RepositoryBase<Picture>(dataContext), IPictureRepository
{
    public async Task<List<Picture>> GetUserPictureAsync(int id)
    {
        var result = dataContext.Pictures
           .AsNoTracking()
           .Where(p => p.AppUserId == id)
           .OrderBy(p => p.Created);
        return await result.ToListAsync();
    }

    public async Task<List<PictureDTO>> GetUserPictureDTOsAsync(int id)
    {
        var result = await dataContext.Pictures
            .AsNoTracking()
            .Where(p => p.AppUserId == id)
            .Select(p => PictureMappings.ToDto(p))
            .ToListAsync();

        return result;
    }
}
