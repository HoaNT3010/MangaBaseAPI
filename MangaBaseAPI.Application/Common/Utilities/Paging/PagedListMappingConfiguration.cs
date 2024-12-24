using AutoMapper;

namespace MangaBaseAPI.Application.Common.Utilities.Paging
{
    public class PagedListMappingConfiguration : Profile
    {
        public PagedListMappingConfiguration()
        {
            CreateMap(typeof(PagedList<>), typeof(PagedList<>));
        }
    }
}
