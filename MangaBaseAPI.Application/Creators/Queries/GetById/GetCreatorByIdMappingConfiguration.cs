using AutoMapper;
using MangaBaseAPI.Contracts.Creators.GetById;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Creators.Queries.GetById
{
    public class GetCreatorByIdMappingConfiguration : Profile
    {
        public GetCreatorByIdMappingConfiguration()
        {
            CreateMap<Creator, GetCreatorByIdResponse>();
        }
    }
}
