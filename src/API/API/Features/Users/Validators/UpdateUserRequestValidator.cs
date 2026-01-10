namespace API.Features.Users.Validators;

public class UpdateUserRequestValidator : AbstractValidator<UpdateUserRequest>
{
    public UpdateUserRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .MaximumLength(255).WithMessage("FirstName must not exceed 255 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required.")
            .MaximumLength(255).WithMessage("LastName must not exceed 255 characters.");

        RuleFor(x => x.CityId)
            .GreaterThan(0).WithMessage("CityId must be a positive integer.");
    }
}
