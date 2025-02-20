using AutoMapper;
using MangaBaseAPI.Contracts.Chapters.Common;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Chapters.Queries.Common.Mappers
{
    public class ChapterMappingConfiguration : Profile
    {
        public ChapterMappingConfiguration()
        {
            CreateMap<Chapter, ShortSingleChapterResponse>();
        }
    }
}
