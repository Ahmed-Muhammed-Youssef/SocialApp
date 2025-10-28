using Mediator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shared.Results;

namespace Application.Features.Roles.List;

public class GetRolesHandler(RoleManager<IdentityRole> roleManager) : IQueryHandler<GetRolesQuery, Result<List<string>>>
{
    public async ValueTask<Result<List<string>>> Handle(GetRolesQuery query, CancellationToken cancellationToken)
    {
        List<string> result = await roleManager.Roles
            .Where(r => r.Name != null)
            .Select(r => r.Name!)
            .ToListAsync(cancellationToken: cancellationToken);

        return Result<List<string>>.Success(result);
    }
}
