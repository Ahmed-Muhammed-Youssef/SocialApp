using FluentValidation;

namespace API.Controllers.Users.Validators;

public class GetUsersRequestValidator : AbstractValidator<GetUsersRequest>
{
    public GetUsersRequestValidator()
    {
        RuleFor(x => x.UserParams.PageNumber)
            .GreaterThanOrEqualTo(1).WithMessage("PageNumber must be greater than or equal to 1.");

        RuleFor(x => x.UserParams.ItemsPerPage)
            .InclusiveBetween(1, 100).WithMessage("ItemsPerPage must be between 1 and 100.");

        RuleFor(x => x.UserParams.MinAge)
            .GreaterThanOrEqualTo(SystemPolicy.UsersMinimumAge).WithMessage($"MinAge must be greater than or equal to {SystemPolicy.UsersMinimumAge}.");

        RuleFor(x => x.UserParams.MaxAge)
            .GreaterThanOrEqualTo(x => x.UserParams.MinAge).WithMessage("MaxAge must be greater than or equal to MinAge.");
    }
}
