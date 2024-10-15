using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Application.DTOs.User;
using Application.DTOs.Pagination;
using AutoMapper;

namespace Infrastructure.Repositories
{
    public class ApplicationUserRepository(DataContext _dataContext, IMapper mapper) : IApplicationUserRepository // using the repository design pattern to isolate the contollers further more from the entity framework. (it may not be neccesary)
    {
        public void Delete(ApplicationUser user)
        {
            _dataContext.Remove(user);
        }

        public async Task AddAsync(ApplicationUser user)
        {
            await _dataContext.ApplicationUsers.AddAsync(user);
        }

        public async Task<bool> IdExistsAsync(int id)
        {
            return await _dataContext.ApplicationUsers
                .AsNoTracking()
                .AnyAsync(e => e.Id == id);
        }
        public void Update(ApplicationUser appUser)
        {
            _dataContext.ChangeTracker.Clear();
            _dataContext.Entry(appUser).State = EntityState.Modified;
        }
        public async Task<PagedList<UserDTO>> GetUsersDTOAsync(int userId, UserParams userParams)
        {
            IEnumerable<UserDTO> users = [];

            using (var connection = new SqlConnection(_dataContext.Database.GetDbConnection().ConnectionString))
            {
                var parameters = new DynamicParameters();
                parameters.Add("@userId", userId);
                parameters.Add("@minAge", userParams.MinAge);
                parameters.Add("@maxAge", userParams.MaxAge);
                parameters.Add("@orderBy", (int)userParams.OrderBy);
                parameters.Add("@pageNumber", userParams.PageNumber);
                parameters.Add("@pageSize", userParams.ItemsPerPage);

                users = await connection.QueryAsync<UserDTO>("GetUsersDtos", parameters, commandType: CommandType.StoredProcedure);
            }
            return new PagedList<UserDTO>(users.ToList(), users.Count(), userParams.PageNumber, userParams.ItemsPerPage);
        }
        public async Task<ApplicationUser> GetByIdentity(string identity)
        {
            return await _dataContext.ApplicationUsers.FirstOrDefaultAsync(u => u.IdentityId == identity);
        }
        
        public async Task<ApplicationUser> GetByIdAsync(int id)
        {
            var connectionString = _dataContext.Database.GetConnectionString();
            using IDbConnection db = new SqlConnection(connectionString);
            return (await db.QueryAsync<ApplicationUser>("GetUserById", new { Id = id }, commandType: CommandType.StoredProcedure)).FirstOrDefault();
        }

        public async Task<UserDTO> GetDtoByIdentityId(string identityId)
        {
            var appUser = await _dataContext.ApplicationUsers.FirstOrDefaultAsync(u => u.IdentityId == identityId);

            return mapper.Map<UserDTO>(appUser);
        }

        public async Task<UserDTO> GetDtoByIdAsync(int id)
        {
            var connectionString = _dataContext.Database.GetConnectionString();
            using IDbConnection db = new SqlConnection(connectionString);
            return (await db.QueryAsync<UserDTO>("GetUserDtoById", new { Id = id }, commandType: CommandType.StoredProcedure)).FirstOrDefault();
        }
    }
}
