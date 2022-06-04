namespace KeycloakDemo.Report.Api.Security;

internal class TokenValidationKeycloak : TokenValidationParameters
{
    public TokenValidationKeycloak(string issuer)
    {
        ValidateAudience = true;
        ValidateIssuerSigningKey = true;
        ValidateIssuer = true;
        ValidIssuer = issuer;
        ValidateLifetime = true;
    }
}
