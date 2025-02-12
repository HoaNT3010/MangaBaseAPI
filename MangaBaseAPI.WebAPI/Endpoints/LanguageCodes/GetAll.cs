using Carter;
using MangaBaseAPI.Application.LanguageCodes.Queries.GetAll;
using MangaBaseAPI.WebAPI.Common;
using MediatR;

namespace MangaBaseAPI.WebAPI.Endpoints.LanguageCodes
{
    public class GetAll : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("languages", HandleGetAllLanguageCodes)
                .WithTags("Language Codes")
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Get all language codes",
                    Description = "Return a collection contains all language codes listed in the system"
                })
                .MapToApiVersion(1);
        }

        private static async Task<IResult> HandleGetAllLanguageCodes(
            HttpContext context,
            ISender sender,
            CancellationToken cancellationToken)
        {
            var query = new GetAllLanguageCodesQuery();

            var result = await sender.Send(query, cancellationToken);

            return result.IsSuccess ? Results.Ok(result) : ResultExtensions.HandleFailure(result, context);
        }
    }
}
