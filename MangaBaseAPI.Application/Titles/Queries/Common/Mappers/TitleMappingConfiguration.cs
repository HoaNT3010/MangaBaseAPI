using AutoMapper;
using MangaBaseAPI.Contracts.Titles.Common;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Titles.Queries.Common.Mappers
{
    public class TitleMappingConfiguration : Profile
    {
        public TitleMappingConfiguration()
        {
            CreateMap<Title, ShortTitleResponse>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.LatestChapter, opt => opt.MapFrom(src => src.Chapters.OrderByDescending(c => c.Index).FirstOrDefault()));
        }
    }
}
