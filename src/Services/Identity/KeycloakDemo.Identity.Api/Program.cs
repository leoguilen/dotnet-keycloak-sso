var builder = WebApplication.CreateBuilder(args);

builder.WebHost
    .ConfigureAppConfiguration(webBuilder => webBuilder.AddEnvironmentVariables());

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen()
    .AddOptions()
    .AddLogging()
    .Configure<OAuthConfig>(builder.Configuration.GetSection("OAuth"))
    .AddHttpClient<KeycloakIdentityService>("keycloak")
    .Services.AddScoped<IIdentityService, KeycloakIdentityService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.MapGet("/", () => Results
        .LocalRedirect("~/swagger"))
        .ExcludeFromDescription();
}

app
    .UseHttpLogging();

app
    .MapPost("/api/auth/signin", SignInHandler)
    .WithName("PostSignIn")
    .WithTags("Sso")
    .AllowAnonymous()
    .Produces<AuthenticationResponse>(StatusCodes.Status200OK)
    .Produces(StatusCodes.Status401Unauthorized, typeof(UnauthorizedResult))
    .ProducesProblem(StatusCodes.Status500InternalServerError);

await app.RunAsync();