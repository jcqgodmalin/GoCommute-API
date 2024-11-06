using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using GoCommute.DTOs;

namespace GoCommute.Helpers;

public static class SecurityHelper
{

    private const int saltSize = 16;
    private const int hashSize = 32;
    private const int Iterations = 100000;
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    public static string PasswordHash(string password){
        byte[] salt = RandomNumberGenerator.GetBytes(saltSize);
        byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password,salt,Iterations,Algorithm,hashSize);

        return $"{Convert.ToHexString(hash)}:{Convert.ToHexString(salt)}";
    }

    public static bool PasswordVerify(string password, string passwordhash){
        var passwordparts = passwordhash.Split(":");

        if(passwordparts.Length != 2){
            return false;
        }

        byte[] hash = Convert.FromHexString(passwordparts[0]);
        byte[] salt = Convert.FromHexString(passwordparts[1]);

        byte[] toCompare = Rfc2898DeriveBytes.Pbkdf2(password,salt,Iterations,Algorithm,hashSize);

        return CryptographicOperations.FixedTimeEquals(hash, toCompare);
    }

    public static string GenerateAppID()
    {
        return Guid.NewGuid().ToString();
    }

    public static string GenerateSecretKey(int length = 32)
    {
        const string validChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        StringBuilder secretKey = new StringBuilder();
        using (var rng = RandomNumberGenerator.Create())
        {
            byte[] buffer = new byte[1];
            while (secretKey.Length < length)
            {
                rng.GetBytes(buffer);
                char randomChar = validChars[buffer[0] % validChars.Length];
                secretKey.Append(randomChar);
            }
        }
        return secretKey.ToString();
    }

    public static string GenerateToken(UserDto user){
        var TokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(user.SecretKey!);
        var tokenDescriptor = new SecurityTokenDescriptor{
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("AppID", user.AppID),
                new Claim(ClaimTypes.Role, user.Role!)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        Console.WriteLine($"Generating token for AppID: {user.AppID}"); // Debugging output
        var token = TokenHandler.CreateToken(tokenDescriptor);
        var tokenString = TokenHandler.WriteToken(token);
        Console.WriteLine($"Generated Token: {tokenString}");
        return tokenString;
    }

}
