namespace Application.Features.UserRoles.Get;

public class GetUserRolesHandler(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager) : IQueryHandler<GetUserRolesQuery, Result<List<string>>>
{
    public async ValueTask<Result<List<string>>> Handle(GetUserRolesQuery query, CancellationToken cancellationToken)
    {
        // get user identity id
        ApplicationUser? user = await unitOfWork.ApplicationUserRepository.GetByIdAsync(query.UserId, cancellationToken);

        if (user is null)
        {
            return Result<List<string>>.NotFound($"User with id {query.UserId} not found.");
        }

        var identityUser = await userManager.FindByIdAsync(user.IdentityId);

        if (identityUser == null)
        {
            return Result<List<string>>.NotFound($"User with id {query.UserId} not found.");
        }

        IList<string> result = await userManager.GetRolesAsync(identityUser);

        return Result<List<string>>.Success(result.ToList());
    }
}
