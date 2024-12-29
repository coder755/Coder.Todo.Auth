namespace Coder.Todo.Auth.Model.Auth;

public class JwtOptions
{
    public required string PrivateKeyPath { get; init; }
    public required string PublicKeyPath { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public required int ExpirationSeconds { get; init; }
}
    