using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Coder.Todo.Auth.Model.Auth;
using Microsoft.IdentityModel.Tokens;

namespace Coder.Todo.Auth.Services.Authorization.Jwt;

public class JwtService(JwtOptions jwtOptions) : IJwtService
{
    public string GenerateUserToken(Guid userId)
    {
        var privateKeyPem = File.ReadAllText(jwtOptions.PrivateKeyPath);
        var ecdsa = ECDsa.Create();
        ecdsa.ImportFromPem(privateKeyPem);

        var signingCredentials = new SigningCredentials(new ECDsaSecurityKey(ecdsa), SecurityAlgorithms.EcdsaSha256);
        var claim = new Claim("sub", userId.ToString());
        var expires = DateTime.Now.AddSeconds(jwtOptions.ExpirationSeconds);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new List<Claim>{claim}),
            Expires = expires,
            Issuer = jwtOptions.Issuer,
            Audience = jwtOptions.Audience,
            SigningCredentials = signingCredentials
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
    
    public string? GetSubjectFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(token)) return null;
        var jwtToken = handler.ReadJwtToken(token);
        var subject = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;
        return subject;
    }
}