using AutoMapper;
using MangaBaseAPI.Contracts.Users.GetById;
using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Application.Users.Queries.GetById
{
    public class GetUserByIdMappingConfiguration : Profile
    {
        public GetUserByIdMappingConfiguration()
        {
            CreateMap<User, GetUserByIdResponse>()
                .ForMember(dest => dest.IsEmailConfirmed, opt => opt.MapFrom(src => src.EmailConfirmed))
                .ForMember(dest => dest.IsPhoneNumberConfirmed, opt => opt.MapFrom(src => src.PhoneNumberConfirmed))
                .ForMember(dest => dest.IsLockoutEnabled, opt => opt.MapFrom(src => src.LockoutEnabled));
        }
    }
}
