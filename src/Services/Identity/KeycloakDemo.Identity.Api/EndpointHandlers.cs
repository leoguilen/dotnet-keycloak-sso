namespace KeycloakDemo.Identity.Api;

internal static class EndpointHandlers
{
    public static readonly Func<SignInRequest, IIdentityService, CancellationToken, Task<IResult>> SignInHandler = HandleSignInAsync;

    private static async Task<IResult> HandleSignInAsync(
        [FromBody] SignInRequest request,
        [FromServices] IIdentityService identityService,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var authResponse = await identityService
                .AuthenticateAsync(request.Username, request.Password, cancellationToken);

            return Results.Ok(AuthenticationResponse.From(authResponse));
        }
        catch (Exception ex)
        {
            return Results.Problem(
                detail: ex.Message,
                title: ex.GetType().Name,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
