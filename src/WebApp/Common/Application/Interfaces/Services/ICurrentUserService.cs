namespace Application.Interfaces.Services;

public interface ICurrentUserService
{
    int GetPublicId();
    string GetEmail();
    IEnumerable<string> GetRoles();
}
