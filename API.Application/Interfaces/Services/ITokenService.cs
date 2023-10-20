using API.Domain.Entities;
using System.Threading.Tasks;

namespace API.Application.Interfaces.Services
{
    public interface ITokenService
    {
        public Task<string> CreateTokenAsync(AppUser user);
    }
}
