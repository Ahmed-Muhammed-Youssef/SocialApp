using Shared.Extensions;


namespace Shared.Validators
{
    /// <summary>
    /// Specifies the minimum age required for a DateTime property.
    /// Validates that the date provided results in an age equal to or greater than the specified minimum age.
    /// </summary>
    public class MinimumAgeAttribute : ValidationAttribute
    {
        private readonly int _minAge;

        public MinimumAgeAttribute(int minAge)
        {
            _minAge = minAge;
        }
        public override bool IsValid(object? value)
        {
            if (value is DateTime dateOfBirth)
            {
                var age = dateOfBirth.CalculateAge();

                return age >= _minAge;
            }
            return true;
        }
    }
}
