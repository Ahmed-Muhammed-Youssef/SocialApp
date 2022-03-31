using API.DTOs;
using API.Entities;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Data
{
    public class LikesRepository : ILikesRepository
    {
        private readonly DataContext dataContext;
        private readonly IMapper mapper;

        public LikesRepository(DataContext dataContext, IMapper mapper)
        {
            this.dataContext = dataContext;
            this.mapper = mapper;
        }

        public async Task<bool> LikeAsync(int likerId, int likedId)
        {
            await dataContext.Likes.AddAsync(new Like() { 
                LikerId = likerId,
                LikedId = likedId
            });
            // check if the liked id likes him or not if it does, it's a match
            bool isMatch = await dataContext.Likes.AnyAsync(l => l.LikerId == likedId
            && l.LikedId == likerId);
            if (isMatch)
            {
                await dataContext.Matches.AddAsync(new Match { UserId = likerId, MatchedId = likedId });
                await dataContext.Matches.AddAsync(new Match { UserId = likedId, MatchedId = likerId });
            }
            return isMatch;
        }
        public async Task<PagedList<UserDTO>> GetMatchesAsync(int id, PaginationParams paginationParams)
        {
            var matchesId = await dataContext.Matches.Where(u => u.UserId == id)
                .Select(u => u.MatchedId).ToListAsync();

            var queryDto = dataContext.Users.Where(user => matchesId.Contains(user.Id))
                .ProjectTo<UserDTO>(mapper.ConfigurationProvider).AsNoTracking();

            queryDto = queryDto.OrderByDescending(u => u.LastActive);

            var pagedResult = await PagedList<UserDTO>.CreatePageAsync(queryDto, paginationParams.PageNumber, paginationParams.ItemsPerPage);
            pagedResult.ForEach(u => u.Photos.OrderBy(p => p.Order));

            return pagedResult;
        }
        public async Task<bool> IsMacth(int userId, int matchedId)
        {
            return await dataContext.Matches.AnyAsync(u => u.UserId == userId && u.MatchedId == matchedId);
        }
        public async Task<Like> GetLikeAsync(int likerId, int likedId)
        {
            return await dataContext.Likes.Where(l => l.LikedId == likedId && l.LikerId == likerId).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserDTO>> GetLikedUsersDTOAsync(int likerId)
        {
            var users = dataContext.Likes.Where(l => l.LikerId == likerId).Select(l => l.Liked).ProjectTo<UserDTO>(mapper.ConfigurationProvider);
            return await users.ToListAsync();
        }

        public async Task<IEnumerable<int>> GetLikedUsersIdAsync(int likerId)
        {
            var users = dataContext.Likes.Where(l => l.LikerId == likerId).Select(l => l.LikedId);
            return await users.ToListAsync();

        }
    }
}
