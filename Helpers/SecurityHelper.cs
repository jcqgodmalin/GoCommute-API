using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using System.Text;
using GoCommute.DTOs;
using Microsoft.AspNetCore.Identity;

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

    public static string GenerateToken(AppDto appDto){

        var TokenHandler = new JwtSecurityTokenHandler();
        
        var key = Encoding.ASCII.GetBytes(appDto.SecretKey);

        var claims = new List<Claim> {
            new Claim("AppID", appDto.AppID)
        };

        foreach(var role in appDto.Role){
            claims.Add(new Claim(ClaimTypes.Role,role));
        }
        
        var tokenDescriptor = new SecurityTokenDescriptor{
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
        };

        Console.WriteLine($"Generating token for AppID: {appDto.AppID}"); // Debugging output
        var token = TokenHandler.CreateToken(tokenDescriptor);
        var tokenString = TokenHandler.WriteToken(token);
        Console.WriteLine($"Generated Token: {tokenString}");
        return tokenString;
    }

    public static ClaimsPrincipal? GetPrincipalFromExpiredToken(string token, string key)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);

        if (securityToken is JwtSecurityToken jwtSecurityToken &&
            jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
            return principal;
        }

        return null;
    }

}
