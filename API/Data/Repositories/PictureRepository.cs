using API.DTOs;
using API.Entities;
using API.Interfaces.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data.Repositories
{
    public class PictureRepository : IPictureRepository
    {
        private readonly DataContext _dataContext;
        private readonly IMapper _mapper;

        public PictureRepository(DataContext dataContext, IMapper mapper)
        {
            _dataContext = dataContext;
            _mapper = mapper;
        }
        public async Task<Picture> AddPictureAsync(Picture picture)
        {
            await _dataContext.Pictures.AddAsync(picture);
            return picture;
        }

        public void DeletePicture(Picture picture)
        {
            _dataContext.Pictures.Remove(picture);
        }

        public async Task<IEnumerable<Picture>> GetUserPictureAsync(int id)
        {
            var result = _dataContext.Pictures
               .AsNoTracking()
               .Where(p => p.AppUserId == id)
               .OrderBy(p => p.Created);
            return await result.ToListAsync();
        }

        public async Task<IEnumerable<PictureDTO>> GetUserPictureDTOsAsync(int id)
        {
            var result = await _dataContext.Pictures
                .AsNoTracking()
                .Where(p => p.AppUserId == id)
                .ProjectTo<PictureDTO>(_mapper.ConfigurationProvider)
                .ToListAsync();

            return result;
        }
        public void UpdatePicture(Picture picture)
        {
            _dataContext.Entry(picture).State = EntityState.Modified;
        }
    }
}
