using System.Security.Cryptography;
using System.Text;
using Konscious.Security.Cryptography;

namespace Coder.Todo.Auth.Services.User;

public static class PasswordUtils
{
    private const int PasswordHashSize = 32;
    private const int SaltSize = 16;
    
    public static byte[] HashPassword(string password,
        byte[] salt,
        int memoryCost = 65536,
        int iterations = 4,
        int degreeOfParallelism = 2)
    {
        using var hasher = new Argon2id(Encoding.UTF8.GetBytes(password));
        hasher.Salt = salt;
        hasher.MemorySize = memoryCost; // In KB
        hasher.Iterations = iterations;
        hasher.DegreeOfParallelism = degreeOfParallelism;

        var hashBytes = hasher.GetBytes(PasswordHashSize); // Length of hash in bytes
        return hashBytes;
    }

    public static bool VerifyPassword(string password,
        byte[] hashedPassword,
        byte[] salt,
        int memoryCost = 65536,
        int iterations = 4,
        int degreeOfParallelism = 2)
    {
        var newHash = HashPassword(password, salt, memoryCost, iterations, degreeOfParallelism);
        return newHash.SequenceEqual(hashedPassword);
    }
    
    public static byte[] GenerateSalt()
    {
        var salt = new byte[SaltSize];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(salt);
        return salt;
    }
}