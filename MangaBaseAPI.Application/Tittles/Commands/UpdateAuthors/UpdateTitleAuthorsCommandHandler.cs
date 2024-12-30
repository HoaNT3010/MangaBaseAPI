using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Title;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaBaseAPI.Application.Tittles.Commands.UpdateAuthors
{
    public class UpdateTitleAuthorsCommandHandler
        : IRequestHandler<UpdateTitleAuthorsCommand, Result>
    {
        readonly IUnitOfWork _unitOfWork;

        public UpdateTitleAuthorsCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateTitleAuthorsCommand request,
            CancellationToken cancellationToken)
        {
            var titleRepository = _unitOfWork.GetRepository<ITitleRepository>();
            var title = await titleRepository.FirstOrDefaultAsync(
                titleRepository.ApplySpecification(new UpdateTitleAuthorsSpecification(request.Id)));

            if (title == null)
            {
                return Result.Failure(TitleErrors.General_TitleNotFound);
            }

            var invalidAuthors = await FindInvalidNewAuthors(request.Authors);
            if (invalidAuthors.Any())
            {
                return Result.Failure(Error.Validation(
                    TitleErrors.Update_InvalidAuthor.Code,
                    TitleErrors.Update_InvalidAuthor.Description + string.Join(", ", invalidAuthors)));
            }

            title.TitleAuthors.Clear();
            foreach (var newAuthorId in request.Authors)
            {
                title.TitleAuthors.Add(new TitleAuthor(title.Id, newAuthorId));
            }

            titleRepository.UpdateAsync(title);
            var updateResult = await _unitOfWork.SaveChangeAsync();
            if (updateResult == 0)
            {
                return Result.Failure(TitleErrors.Update_UpdateAuthorFailed);
            }

            return Result.SuccessNullError();
        }

        private async Task<List<Guid>> FindInvalidNewAuthors(List<Guid> newAuthors)
        {
            if (newAuthors.Count == 0)
            {
                return newAuthors;
            }

            var authorRepository = _unitOfWork.GetRepository<ICreatorRepository>();
            var existingAuthors = await authorRepository.GetQueryableSet()
                .Select(x => x.Id)
                .Where(x => newAuthors.Contains(x))
                .ToListAsync();

            return newAuthors.Except(existingAuthors).ToList();
        }
    }
}
