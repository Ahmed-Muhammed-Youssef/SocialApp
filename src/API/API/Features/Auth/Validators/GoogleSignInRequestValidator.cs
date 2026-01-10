namespace API.Features.Auth.Validators;

public class GoogleSignInRequestValidator : AbstractValidator<GoogleSignInRequest>
{
    public GoogleSignInRequestValidator()
    {
        RuleFor(x => x.Credential)
            .NotEmpty().WithMessage("Credential is required.");
    }
}
