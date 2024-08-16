using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Application.Interfaces.Repositories;
using Microsoft.Data.SqlClient;
using System.Data;
using Dapper;
using Application.DTOs.User;
using Application.DTOs.Pagination;

namespace Infrastructure.Repositories
{
    public class UserRepository(DataContext _dataContext) : IUserRepository // using the repository design pattern to isolate the contollers further more from the entity framework. (it may not be neccesary)
    {
        public void DeleteUser(AppUser user)
        {
            _dataContext.Remove(user);
        }
        public async Task<bool> UserExistsAsync(int id)
        {
            return await _dataContext.Users
                .AsNoTracking()
                .AnyAsync(e => e.Id == id);
        }
        public void Update(AppUser appUser)
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
                parameters.Add("@sex", (int)userParams.Sex);
                parameters.Add("@minAge", userParams.MinAge);
                parameters.Add("@maxAge", userParams.MaxAge);
                parameters.Add("@orderBy", (int)userParams.OrderBy);
                parameters.Add("@pageNumber", userParams.PageNumber);
                parameters.Add("@pageSize", userParams.ItemsPerPage);

                users = await connection.QueryAsync<UserDTO>("GetUsersDtos", parameters, commandType: CommandType.StoredProcedure);
            }
            return new PagedList<UserDTO>(users.ToList(), users.Count(), userParams.PageNumber, userParams.ItemsPerPage);
        }
        public async Task<AppUser> GetUserByIdAsync(int id)
        {
            var connectionString = _dataContext.Database.GetConnectionString();
            using IDbConnection db = new SqlConnection(connectionString);
            return (await db.QueryAsync<AppUser>("GetUserById", new { Id = id }, commandType: CommandType.StoredProcedure)).FirstOrDefault();
        }
        public async Task<UserDTO> GetUserDTOByIdAsync(int id)
        {
            var connectionString = _dataContext.Database.GetConnectionString();
            using IDbConnection db = new SqlConnection(connectionString);
            return (await db.QueryAsync<UserDTO>("GetUserDtoById", new { Id = id }, commandType: CommandType.StoredProcedure)).FirstOrDefault();
        }
    }
}
