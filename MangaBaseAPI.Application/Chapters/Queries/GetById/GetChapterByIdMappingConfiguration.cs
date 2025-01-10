using AutoMapper;
using MangaBaseAPI.Contracts.Chapters.GetById;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Chapters.Queries.GetById
{
    public class GetChapterByIdMappingConfiguration : Profile
    {
        public GetChapterByIdMappingConfiguration()
        {
            CreateMap<Chapter, GetChapterByIdResponse>()
                .ForMember(dest => dest.Images, opt => opt.MapFrom(src => src.ChapterImages))
                .ForMember(dest => dest.TotalPages, opt => opt.MapFrom(src => src.ChapterImages.Count));

            CreateMap<ChapterImage, ChapterImageResponse>();

            CreateMap<Title, ChapterTitleResponse>();
        }
    }
}
