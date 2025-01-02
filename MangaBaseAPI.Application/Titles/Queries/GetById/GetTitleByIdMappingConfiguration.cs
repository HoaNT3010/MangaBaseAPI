using AutoMapper;
using MangaBaseAPI.Contracts.Titles.GetById;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Titles.Queries.GetById
{
    public class GetTitleByIdMappingConfiguration : Profile
    {
        public GetTitleByIdMappingConfiguration()
        {
            CreateMap<Title, GetTitleByIdResponse>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Genres, opt => opt.MapFrom(src => src.TitleGenres.Select(x => x.Genre.Name).ToList()))
                .ForMember(dest => dest.AlternativeNames, opt => opt.MapFrom(src => src.AlternativeNames))
                .ForMember(dest => dest.Authors, opt => opt.MapFrom(src => src.TitleAuthors.Select(x => x.Author).ToList()))
                .ForMember(dest => dest.Artists, opt => opt.MapFrom(src => src.TitleArtists.Select(x => x.Artist).ToList()));

            CreateMap<AlternativeName, TitleAlternativeName>()
                .ForMember(dest => dest.LanguageEnglishName, opt => opt.MapFrom(src => src.LanguageCode.EnglishName));

            CreateMap<Creator, TitleCreator>();
        }
    }
}
