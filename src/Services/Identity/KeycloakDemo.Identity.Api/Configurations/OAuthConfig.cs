#nullable disable
namespace KeycloakDemo.Identity.Api.Configurations;

public record class OAuthConfig
{
    public string MetadataUrl { get; init; }

    public string ClientId { get; init; }

    public string ClientSecret { get; init; }

    public string[] Scopes { get; init; }
}
