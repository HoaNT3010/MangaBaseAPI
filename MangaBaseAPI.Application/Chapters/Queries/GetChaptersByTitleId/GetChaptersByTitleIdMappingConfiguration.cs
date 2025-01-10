using AutoMapper;
using MangaBaseAPI.Contracts.Chapters.GetChaptersByTitleId;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Chapters.Queries.GetChaptersByTitleId
{
    public class GetChaptersByTitleIdMappingConfiguration : Profile
    {
        public GetChaptersByTitleIdMappingConfiguration()
        {
            CreateMap<Chapter, GetChaptersByTitleIdResponse>();
        }
    }
}
