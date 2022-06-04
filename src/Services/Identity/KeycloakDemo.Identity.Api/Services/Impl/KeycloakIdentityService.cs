namespace KeycloakDemo.Identity.Api.Services.Impl;

internal class KeycloakIdentityService : IIdentityService
{
    private readonly OAuthConfig _oauthConfig;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<KeycloakIdentityService> _logger;

    public KeycloakIdentityService(
        IOptions<OAuthConfig> oauthConfig,
        IHttpClientFactory httpClientFactory,
        ILogger<KeycloakIdentityService> logger)
    {
        _oauthConfig = oauthConfig.Value;
        _httpClientFactory = httpClientFactory;
        _logger = logger;

        _logger.LogDebug($"Loaded oauth configurations with {JsonSerializer.Serialize(oauthConfig.Value)}");
    }

    public async Task<TokenResponse> AuthenticateAsync(CancellationToken cancellationToken = default)
    {
        using var client = _httpClientFactory.CreateClient("keycloak");

        var discoveryDocumentResponse = await GetDiscoveryDocumentAsync(client, cancellationToken);
        var tokenResponse = await client.RequestClientCredentialsTokenAsync(
            request: new()
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                ClientId = _oauthConfig.ClientId,
                ClientSecret = _oauthConfig.ClientSecret,
                Scope = string.Join(' ', _oauthConfig.Scopes),
            },
            cancellationToken: cancellationToken);

        if (tokenResponse.IsError)
        {
            _logger.LogError(tokenResponse.Exception, tokenResponse.ErrorDescription);
            throw new HttpRequestException(
                tokenResponse.ErrorDescription,
                tokenResponse.Exception,
                tokenResponse.HttpStatusCode);
        }

        return tokenResponse;
    }

    public async Task<TokenResponse> AuthenticateAsync(
        string username,
        string password,
        CancellationToken cancellationToken = default)
    {
        using var client = _httpClientFactory.CreateClient("keycloak");

        var discoveryDocumentResponse = await GetDiscoveryDocumentAsync(client, cancellationToken);

        var tokenResponse = await client.RequestPasswordTokenAsync(
            request: new()
            {
                Address = discoveryDocumentResponse.TokenEndpoint,
                ClientId = _oauthConfig.ClientId,
                ClientSecret = _oauthConfig.ClientSecret,
                Scope = string.Join(' ', _oauthConfig.Scopes),
                UserName = username,
                Password = password,
            },
            cancellationToken: cancellationToken);

        if (tokenResponse.IsError)
        {
            _logger.LogError(tokenResponse.Exception, tokenResponse.ErrorDescription);
            throw new HttpRequestException(
                tokenResponse.ErrorDescription,
                tokenResponse.Exception,
                tokenResponse.HttpStatusCode);
        }

        return tokenResponse;
    }

    private async Task<DiscoveryDocumentResponse> GetDiscoveryDocumentAsync(HttpClient client, CancellationToken cancellationToken)
    {
        var response = await client
            .GetDiscoveryDocumentAsync(
                address: _oauthConfig.MetadataUrl,
                cancellationToken: cancellationToken);

        if (response.IsError)
        {
            _logger.LogError(response.Exception, response.Exception.Message);
            throw new HttpRequestException(
                response.Exception.Message,
                response.Exception,
                response.HttpStatusCode);
        }

        return response;
    }
}
