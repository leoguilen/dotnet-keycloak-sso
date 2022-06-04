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
    .AddSingleton<IOperationRepository, InMemoryOperationRepository>()
    .AddScoped<IOperationService, OperationService>();

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
    .RequireAuthorization("operationApiScope");
{
    apiGroup
        .MapGet("/operations", GetOperationsHandler)
        .RequireAuthorization(policy => policy.RequireRole("viewer", "operator"))
        .WithName("GetOperations")
        .WithTags("Operation")
        .Produces<OperationResponse[]>(StatusCodes.Status200OK);

    apiGroup
        .MapGet("/operations/{operationId:Guid}", GetOperationHandler)
        .RequireAuthorization(policy => policy.RequireRole("viewer", "operator"))
        .WithName("GetOperation")
        .WithTags("Operation")
        .Produces<OperationResponse>(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status404NotFound);

    apiGroup
        .MapPost("/operations", CreateOperationHandler)
        .RequireAuthorization(policy => policy.RequireRole("operator"))
        .WithName("PostOperation")
        .WithTags("Operation")
        .Accepts<CreateOperationRequest>("application/json")
        .Produces(StatusCodes.Status201Created);
}

await app.RunAsync();