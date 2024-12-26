using AutoMapper;
using MangaBaseAPI.Application.Common.Utilities.Paging;
using MangaBaseAPI.Contracts.Creators.Search;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Repositories;
using MediatR;

namespace MangaBaseAPI.Application.Creators.Queries.SearchByName
{
    public class SearchCreatorByNameQueryHandler : IRequestHandler<SearchCreatorByNameQuery, Result<PagedList<SearchCreatorByNameResponse>>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SearchCreatorByNameQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<SearchCreatorByNameResponse>>> Handle(
            SearchCreatorByNameQuery request,
            CancellationToken cancellationToken)
        {
            var creatorRepository = _unitOfWork.GetRepository<ICreatorRepository>();
            var query = creatorRepository.ApplySpecification(new SearchCreatorByNameSpecification(request.Keyword))
                .Select(x => new Creator
                {
                    Id = x.Id,
                    Name = x.Name
                });
            var creators = await PagedList<Creator>.CreateAsync(query, request.Page, request.PageSize);

            return Result.SuccessNullError(_mapper.Map<PagedList<SearchCreatorByNameResponse>>(creators));
        }
    }
}
