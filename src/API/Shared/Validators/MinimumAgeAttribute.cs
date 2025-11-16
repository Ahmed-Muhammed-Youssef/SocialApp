using Shared.Extensions;


namespace Shared.Validators;

/// <summary>
/// Specifies the minimum age required for a DateTime property.
/// Validates that the date provided results in an age equal to or greater than the specified minimum age.
/// </summary>
public class MinimumAgeAttribute(int minAge) : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is DateTime dateOfBirth)
        {
            var age = dateOfBirth.CalculateAge();

            return age >= minAge;
        }
        return true;
    }
}
