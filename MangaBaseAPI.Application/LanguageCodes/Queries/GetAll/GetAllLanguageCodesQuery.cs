using MangaBaseAPI.Contracts.LanguageCodes.GetAll;
using MangaBaseAPI.Domain.Abstractions;
using MediatR;

namespace MangaBaseAPI.Application.LanguageCodes.Queries.GetAll
{
    public record GetAllLanguageCodesQuery()
        : IRequest<Result<List<LanguageCodeResponse>>>;
}
