namespace API.Features.Users.Validators;

public class GetUsersRequestValidator : AbstractValidator<GetUsersRequest>
{
    public GetUsersRequestValidator()
    {
        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be greater than or equal to 1.");

        RuleFor(x => x.ItemsPerPage)
            .InclusiveBetween(1, SystemPolicy.MaxPageSize).WithMessage($"ItemsPerPage must be between 1 and {SystemPolicy.MaxPageSize}.");

        RuleFor(x => x.MinAge)
            .GreaterThanOrEqualTo(SystemPolicy.UsersMinimumAge).WithMessage($"MinAge must be greater than or equal to {SystemPolicy.UsersMinimumAge}.");

        RuleFor(x => x.MaxAge)
            .GreaterThanOrEqualTo(x => x.MinAge).WithMessage("MaxAge must be greater than or equal to MinAge.");
    }
}
