var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddEndpointsApiExplorer()
    .AddSwaggerGen(options =>
    {
        options.AddSecurityDefinition(
            name: JwtBearerDefaults.AuthenticationScheme,
            securityScheme: new()
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 1safsfsdfdfd\"",
            });

        options.AddSecurityRequirement(new OpenApiSecurityRequirement
        {
            {
                  new OpenApiSecurityScheme
                  {
                      Reference = new OpenApiReference
                      {
                          Type = ReferenceType.SecurityScheme,
                          Id = "Bearer"
                      }
                  },
                  Array.Empty<string>()
            }
        });
    })
    .AddKeycloakAuth(builder.Configuration)
    .AddHttpContextAccessor()
    .AddHttpClient("OperationApiClient", (provider, client) =>
    {
        var httpContextAccessor = provider.GetRequiredService<IHttpContextAccessor>();

        var authAccessToken = httpContextAccessor.HttpContext!.Request.Headers.Authorization;
        var authAccessTokenFormatted = authAccessToken.ToString().Remove(0, 7);

        client.BaseAddress = builder.Configuration.GetValue<Uri>("OperationApi:BaseUrl");
        //client.Timeout = TimeSpan.FromMilliseconds(builder.Configuration.GetValue("OperationApi:TimeoutInMilliseconds", int.MaxValue));
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", authAccessTokenFormatted);
    })
    .Services.AddScoped<IOperationApiService, OperationApiService>();

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
    .UseHttpLogging()
    .UseAuthentication()
    .UseAuthorization();

var apiGroup = app
    .MapGroup("/api")
    .RequireAuthorization("reportApiScope");
{
    apiGroup
        .MapGet("/operations/report", GetOperationsReportHandler)
        .RequireAuthorization(policy => policy.RequireRole("report-viewer"))
        .WithName("GetOperationsReport")
        .WithTags("Report");
}

await app.RunAsync();