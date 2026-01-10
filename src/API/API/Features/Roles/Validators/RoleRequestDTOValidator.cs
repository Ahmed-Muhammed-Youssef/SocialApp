namespace API.Features.Roles.Validators;

public class RoleRequestDTOValidator : AbstractValidator<RoleRequestDTO>
{
    public RoleRequestDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
    }
}
