using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Constants.Caching;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Title;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace MangaBaseAPI.Application.Titles.Commands.UpdateAuthors
{
    public class UpdateTitleAuthorsCommandHandler
        : IRequestHandler<UpdateTitleAuthorsCommand, Result>
    {
        readonly IUnitOfWork _unitOfWork;
        readonly IDistributedCache _cache;

        public UpdateTitleAuthorsCommandHandler(
            IUnitOfWork unitOfWork,
            IDistributedCache cache)
        {
            _unitOfWork = unitOfWork;
            _cache = cache;
        }

        public async Task<Result> Handle(
            UpdateTitleAuthorsCommand request,
            CancellationToken cancellationToken)
        {
            var titleRepository = _unitOfWork.GetRepository<ITitleRepository>();
            var title = await titleRepository.FirstOrDefaultAsync(
                titleRepository.ApplySpecification(new UpdateTitleAuthorsSpecification(request.Id)),
                cancellationToken);

            if (title == null)
            {
                return Result.Failure(TitleErrors.General_TitleNotFound);
            }

            var invalidAuthors = await FindInvalidNewAuthors(request.Authors, cancellationToken);
            if (invalidAuthors.Any())
            {
                return Result.Failure(Error.Validation(
                    TitleErrors.Update_InvalidAuthor.Code,
                    TitleErrors.Update_InvalidAuthor.Description,
                    invalidAuthors));
            }

            title.TitleAuthors.Clear();
            foreach (var newAuthorId in request.Authors)
            {
                title.TitleAuthors.Add(new TitleAuthor(title.Id, newAuthorId));
            }

            titleRepository.Update(title);
            var updateResult = await _unitOfWork.SaveChangeAsync(cancellationToken);
            if (updateResult == 0)
            {
                return Result.Failure(TitleErrors.Update_UpdateAuthorFailed);
            }

            _ = _cache.RemoveAsync(ChapterCachingConstants.GetByIdKey + request.Id, cancellationToken);

            return Result.SuccessNullError();
        }

        private async Task<List<Guid>> FindInvalidNewAuthors(List<Guid> newAuthors, CancellationToken cancellationToken)
        {
            if (newAuthors.Count == 0)
            {
                return newAuthors;
            }

            var authorRepository = _unitOfWork.GetRepository<ICreatorRepository>();
            var existingAuthors = await authorRepository.GetQueryableSet()
                .Select(x => x.Id)
                .Where(x => newAuthors.Contains(x))
                .ToListAsync(cancellationToken);

            return newAuthors.Except(existingAuthors).ToList();
        }
    }
}
