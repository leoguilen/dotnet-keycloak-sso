namespace KeycloakDemo.Identity.Api.Services;

public interface IIdentityService
{
    Task<TokenResponse> AuthenticateAsync(
        CancellationToken cancellationToken = default);

    Task<TokenResponse> AuthenticateAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default);
}
