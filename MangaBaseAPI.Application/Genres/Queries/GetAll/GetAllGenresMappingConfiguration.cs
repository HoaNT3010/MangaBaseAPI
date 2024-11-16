using AutoMapper;
using MangaBaseAPI.Contracts.Genres.GetAll;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Genres.Queries.GetAll
{
    public class GetAllGenresMappingConfiguration : Profile
    {
        public GetAllGenresMappingConfiguration()
        {
            CreateMap<Genre, GenreResponse>();
        }
    }
}
