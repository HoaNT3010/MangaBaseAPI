using AutoMapper;
using MangaBaseAPI.Contracts.Chapters.GetTitleChaptersList;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Chapters.Queries.GetTitleChaptersList
{
    public class GetTitleChaptersListMappingConfiguration : Profile
    {
        public GetTitleChaptersListMappingConfiguration()
        {
            CreateMap<Chapter, GetTitleChaptersListResponse>();
        }
    }
}
