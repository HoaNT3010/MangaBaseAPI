using Carter;
using MangaBaseAPI.Application.Authentication.Commands.Register;
using MangaBaseAPI.Contracts.Authentication.Register;
using MangaBaseAPI.WebAPI.Common;
using MediatR;

namespace MangaBaseAPI.WebAPI.Endpoints.Authentication
{
    public class Register : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("auth/register", HandleRegister)
                .WithTags("Authentication")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "User can create new account",
                    Description = "Endpoint for user to create new account in system with role 'Member'"
                })
                .MapToApiVersion(1);
        }

        private static async Task<IResult> HandleRegister(
            RegisterRequest registerRequest,
            HttpContext context,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var command = new RegisterCommand(
                registerRequest.UserName,
                registerRequest.Email,
                registerRequest.Password,
                registerRequest.ConfirmedPassword);

            var result = await sender.Send(command, cancellationToken);

            return result.IsSuccess ? Results.Created() : ResultExtensions.HandleFailure(result, context);
        }
    }
}
