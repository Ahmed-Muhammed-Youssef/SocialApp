using System.Security.Cryptography;
using System.Text;

namespace Domain
{
    public class PasswordGenerationService
    {
        public string GenerateRandomPassword(int length = 20)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^&*()_-+=<>?";
            using var rng = RandomNumberGenerator.Create();
            var bytes = new byte[length];
            rng.GetBytes(bytes);

            var result = new StringBuilder(length);
            bool hasLowercase = false;
            bool hasUppercase = false;
            bool hasNumber = false;
            bool hasSpecialChar = false;

            foreach (var b in bytes)
            {
                char character = validChars[b % validChars.Length];
                result.Append(character);

                if (char.IsLower(character))
                    hasLowercase = true;
                else if (char.IsUpper(character))
                    hasUppercase = true;
                else if (char.IsDigit(character))
                    hasNumber = true;
                else
                    hasSpecialChar = true;
            }

            // Ensure all required character types are present
            if (!hasLowercase || !hasUppercase || !hasNumber || !hasSpecialChar)
            {
                // Add missing characters to meet requirements
                if (!hasLowercase) result.Append(validChars[0]);
                if (!hasUppercase) result.Append(validChars[26]);
                if (!hasNumber) result.Append(validChars[52]);
                if (!hasSpecialChar) result.Append(validChars[62]);
            }
            return result.ToString();
        }
    }

}
