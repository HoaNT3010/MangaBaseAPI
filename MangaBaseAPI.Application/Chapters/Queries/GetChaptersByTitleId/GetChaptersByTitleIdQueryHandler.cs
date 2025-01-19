using AutoMapper;
using MangaBaseAPI.Application.Common.Utilities.Paging;
using MangaBaseAPI.Contracts.Chapters.GetChaptersByTitleId;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Title;
using MangaBaseAPI.Domain.Repositories;
using MediatR;

namespace MangaBaseAPI.Application.Chapters.Queries.GetChaptersByTitleId
{
    public class GetChaptersByTitleIdQueryHandler
        : IRequestHandler<
            GetChaptersByTitleIdQuery,
            Result<PagedList<GetChaptersByTitleIdResponse>>>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IMapper _mapper;

        public GetChaptersByTitleIdQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Result<PagedList<GetChaptersByTitleIdResponse>>> Handle(
            GetChaptersByTitleIdQuery request,
            CancellationToken cancellationToken)
        {
            var titleRepository = _unitOfWork.GetRepository<ITitleRepository>();

            if (!await titleRepository.IsTitleExists(request.Id, cancellationToken))
            {
                return Result.Failure<PagedList<GetChaptersByTitleIdResponse>>(
                    Error.ErrorWithValue(
                        TitleErrors.General_TitleNotFound,
                        request.Id));
            }

            if (await titleRepository.IsTitleHidden(request.Id, cancellationToken))
            {
                return Result.Failure<PagedList<GetChaptersByTitleIdResponse>>(
                    Error.ErrorWithValue(
                        TitleErrors.General_TitleHidden,
                        request.Id));
            }

            if (await titleRepository.IsTitleDeleted(request.Id, cancellationToken))
            {
                return Result.Failure<PagedList<GetChaptersByTitleIdResponse>>(
                    Error.ErrorWithValue(
                        TitleErrors.General_TitleDeleted,
                        request.Id));
            }

            var chapterRepository = _unitOfWork.GetRepository<IChapterRepository>();
            var query = chapterRepository.ApplySpecification(new GetChaptersByTitleIdSpecification(
                request.Id,
                request.DescendingIndexOrder));

            var chapters = await PagedList<Chapter>.CreateAsync(query, request.Page, request.PageSize, cancellationToken);

            return Result.SuccessNullError(_mapper.Map<PagedList<GetChaptersByTitleIdResponse>>(chapters));
        }
    }
}
