using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Title;
using MangaBaseAPI.Domain.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MangaBaseAPI.Application.Titles.Commands.UpdateArtists
{
    public class UpdateTitleArtistsCommandHandler
        : IRequestHandler<UpdateTitleArtistsCommand, Result>
    {
        readonly IUnitOfWork _unitOfWork;

        public UpdateTitleArtistsCommandHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            UpdateTitleArtistsCommand request,
            CancellationToken cancellationToken)
        {
            var titleRepository = _unitOfWork.GetRepository<ITitleRepository>();
            var title = await titleRepository.FirstOrDefaultAsync(
                titleRepository.ApplySpecification(new UpdateTitleArtistsSpecification(request.Id)));

            if (title == null)
            {
                return Result.Failure(TitleErrors.General_TitleNotFound);
            }

            var invalidArtists = await FindInvalidNewArtists(request.Artists);
            if (invalidArtists.Any())
            {
                return Result.Failure(Error.Validation(
                    TitleErrors.Update_InvalidArtist.Code,
                    TitleErrors.Update_InvalidArtist.Description,
                    invalidArtists));
            }

            title.TitleArtists.Clear();
            foreach (var newArtistId in request.Artists)
            {
                title.TitleArtists.Add(new TitleArtist(title.Id, newArtistId));
            }

            titleRepository.UpdateAsync(title);
            var updateResult = await _unitOfWork.SaveChangeAsync();
            if (updateResult == 0)
            {
                return Result.Failure(TitleErrors.Update_UpdateArtistFailed);
            }

            return Result.SuccessNullError();
        }

        private async Task<List<Guid>> FindInvalidNewArtists(List<Guid> newArtists)
        {
            if (newArtists.Count == 0)
            {
                return newArtists;
            }

            var artistRepository = _unitOfWork.GetRepository<ICreatorRepository>();
            var existingArtist = await artistRepository.GetQueryableSet()
                .Select(x => x.Id)
                .Where(x => newArtists.Contains(x))
                .ToListAsync();

            return newArtists.Except(existingArtist).ToList();
        }
    }
}
