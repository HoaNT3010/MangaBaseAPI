using AutoMapper;
using MangaBaseAPI.Contracts.Creators.Search;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Creators.Queries
{
    public class SearchCreatorByNameMappingConfiguration : Profile
    {
        public SearchCreatorByNameMappingConfiguration()
        {
            CreateMap<Creator, SearchCreatorByNameResponse>();
        }
    }
}
