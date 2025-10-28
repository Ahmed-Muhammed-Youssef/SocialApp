namespace Application.Common.Interfaces;

public interface ICurrentUserService
{
    int GetPublicId();
    string GetEmail();
    IEnumerable<string> GetRoles();
}
