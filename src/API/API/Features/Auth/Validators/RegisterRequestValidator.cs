namespace API.Features.Auth.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.FirstName)
            .NotEmpty().WithMessage("FirstName is required.")
            .MaximumLength(255).WithMessage("FirstName must not exceed 255 characters.");

        RuleFor(x => x.LastName)
            .NotEmpty().WithMessage("LastName is required.")
            .MaximumLength(255).WithMessage("LastName must not exceed 255 characters.");

        RuleFor(x => x.Gender)
            .IsInEnum().WithMessage("Gender is required.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("A valid email address is required.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

        RuleFor(x => x.DateOfBirth)
            .NotEmpty().WithMessage("DateOfBirth is required.")
            .Must(d => d <= DateTime.UtcNow.AddYears(-SystemPolicy.UsersMinimumAge))
            .WithMessage($"User must be at least {SystemPolicy.UsersMinimumAge} years old.");

        RuleFor(x => x.CityId)
            .GreaterThan(0).WithMessage("CityId must be a positive integer.");
    }
}
