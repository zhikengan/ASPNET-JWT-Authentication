namespace JwtAuth.Services.Helpers
{
    public static class CryptoHelper
    {
        public static string GenerateSalt(int length = 32)
        {
            using var rng = System.Security.Cryptography.RandomNumberGenerator.Create();
            var salt = new byte[length];
            rng.GetBytes(salt);
            return Convert.ToBase64String(salt);
        }

        public static string HashPassword(string password, string salt)
        {
            // Use SHA-256 to hash the password with the salt
            using var sha256 = System.Security.Cryptography.SHA256.Create();

            // Combine password and salt
            var saltedPassword = System.Text.Encoding.UTF8.GetBytes(password + salt);

            // Compute the hash
            var hash = sha256.ComputeHash(saltedPassword);

            return Convert.ToBase64String(hash);
        }
        public static bool VerifyPassword(string password, string hashedPassword, string salt)
        {
            var hashedInput = HashPassword(password, salt);
            return hashedInput == hashedPassword;
        }
    }
}
