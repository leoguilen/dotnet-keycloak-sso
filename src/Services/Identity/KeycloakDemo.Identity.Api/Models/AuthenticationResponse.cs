namespace KeycloakDemo.Identity.Api.Models;

public readonly record struct AuthenticationResponse(string AccessToken, DateTime IssuedAt, DateTime ExpiresIn)
{
    public static AuthenticationResponse From(TokenResponse tokenResponse)
        => new(tokenResponse.AccessToken, DateTime.UtcNow, DateTime.UtcNow.AddMinutes(tokenResponse.ExpiresIn));
}
