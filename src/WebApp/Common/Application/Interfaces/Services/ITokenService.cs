using Domain.Entities;
using System.Threading.Tasks;

namespace Application.Interfaces.Services
{
    public interface ITokenService
    {
        public Task<string> CreateTokenAsync(AppUser user);
    }
}
