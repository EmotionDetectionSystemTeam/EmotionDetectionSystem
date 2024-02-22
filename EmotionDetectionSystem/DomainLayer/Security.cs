using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace EmotionDetectionSystem.DomainLayer;

public class Security
{
    public string HashPassword(string password)
    {
        byte[] salt = GenerateSalt();
        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, salt, 10_000);
        byte[] hash = pbkdf2.GetBytes(24);

        byte[] hashBytes = new byte[36];
        Array.Copy(salt, 0, hashBytes, 0, 12);
        Array.Copy(hash, 0, hashBytes, 12, 24);

        string hashedPassword = Convert.ToBase64String(hashBytes);
        return hashedPassword;
    }

    public bool VerifyPassword(string password, string hashedPassword)
    {
        byte[] hashBytes = Convert.FromBase64String(hashedPassword);
        byte[] salt = new byte[12];
        Array.Copy(hashBytes, 0, salt, 0, 12);

        byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

        Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(passwordBytes, salt, 10_000);
        byte[] hash = pbkdf2.GetBytes(24);

        bool passwordMatch = hash.SequenceEqual(hashBytes.Skip(12));

        return passwordMatch;
    }

    private static byte[] GenerateSalt()
    {
        byte[] salt = new byte[12];
        using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }
    
    public bool IsValidPassword(string password)
    {
        // Check for minimum length
        if (password.Length < 8)
            return false;

        // Check for at least one uppercase letter, one lowercase letter, and one digit
        if (!password.Any(char.IsUpper) || !password.Any(char.IsLower) || !password.Any(char.IsDigit))
            return false;

        // Check for at least one special character using a regular expression
        if (!Regex.IsMatch(password, @"[!@#$%^&*()_+=\[{\]};:<>|./?,-]"))
            return false;

        // Additional checks can be added as needed

        return true;
    }
}