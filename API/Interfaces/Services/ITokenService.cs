using API.Entities;
using System.Threading.Tasks;

namespace API.Interfaces.Services
{
    public interface ITokenService
    {
        public Task<string> CreateTokenAsync(AppUser user);
    }
}
