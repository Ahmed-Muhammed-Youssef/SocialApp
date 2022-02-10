using API.Entities;

namespace API.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(AppUser user);
    }
}
