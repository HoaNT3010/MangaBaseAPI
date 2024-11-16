using AutoMapper;
using MangaBaseAPI.Contracts.LanguageCodes.GetAll;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.LanguageCodes.Queries.GetAll
{
    public class GetAllLanguageCodesMappingConfiguration : Profile
    {
        public GetAllLanguageCodesMappingConfiguration()
        {
            CreateMap<LanguageCode, LanguageCodeResponse>();
        }
    }
}
