namespace KeycloakDemo.Operation.Api.Extensions;

internal static class SecurityExtensions
{
    public static IServiceCollection AddKeycloakAuth(
        this IServiceCollection services,
        IConfiguration configuration)
        => services
            .AddAuthentication(auth =>
            {
                auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(jwtOptions =>
            {
                jwtOptions.Authority = configuration["Authentication:Authority"];
                jwtOptions.Audience = configuration["Authentication:Audience"];
                jwtOptions.SaveToken = false;
                jwtOptions.IncludeErrorDetails = true;
                jwtOptions.RequireHttpsMetadata = false;
                jwtOptions.TokenValidationParameters = new TokenValidationKeycloak(configuration["Authentication:Authority"]!);
                jwtOptions.Events = new JwtBearerEvents
                {
                    OnMessageReceived = async ctx =>
                    {
                        var x = ctx;
                    },
                    OnAuthenticationFailed = async failedAuthContext =>
                    {
                        failedAuthContext.NoResult();
                        failedAuthContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
                        failedAuthContext.Response.ContentType = "text/plain";

                        var errorMessage = failedAuthContext.Exception switch
                        {
                            SecurityTokenExpiredException _ => "Expired token.",
                            SecurityTokenNotYetValidException _ => "Token not yet valid.",
                            SecurityTokenInvalidLifetimeException _ => "Invalid token lifetime.",
                            SecurityTokenNoExpirationException _ => "Missing token expiration.",
                            SecurityTokenSignatureKeyNotFoundException _ => "Invalid token. Key not found.",
                            _ => "An error occured processing your authentication."
                        };

                        await failedAuthContext.Response.WriteAsync(errorMessage);
                    },
                    OnTokenValidated = context =>
                    {
                        MapKeycloakRolesToRoleClaims(context);
                        return Task.CompletedTask;
                    }
                };
            })
            .Services
            .AddAuthorization(options =>
            {
                options.AddPolicy("operationApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireAssertion(ctx => ctx.User.HasClaim(c => c.Type is "scope" && c.Value.Contains("operation")));
                });
            });

    private static void MapKeycloakRolesToRoleClaims(TokenValidatedContext context)
    {
        var resourceAccess = JsonNode.Parse(context.Principal!.FindFirst("resource_access")!.Value);
        var clientResource = resourceAccess![context.Principal.FindFirstValue("aud")!];
        var clientRoles = clientResource!["roles"];

        if (context.Principal.Identity is not ClaimsIdentity claimsIdentity)
        {
            return;
        }

        var clientRolesClaims = clientRoles!
            .AsArray()
            .Select(clientRole => new Claim(ClaimTypes.Role, clientRole!.ToString()));

        claimsIdentity.AddClaims(clientRolesClaims);
    }
}
