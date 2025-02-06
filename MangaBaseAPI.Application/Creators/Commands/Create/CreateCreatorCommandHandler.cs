using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.Creator;
using MangaBaseAPI.Domain.Repositories;
using MediatR;

namespace MangaBaseAPI.Application.Creators.Commands.Create
{
    public class CreateCreatorCommandHandler
        : IRequestHandler<CreateCreatorCommand, Result>
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateCreatorCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            CreateCreatorCommand request,
            CancellationToken cancellationToken)
        {
            var creatorRepository = _unitOfWork.GetRepository<ICreatorRepository>();
            // Check if creator with same name already exists
            if (await creatorRepository.IsCreatorNameExist(request.Name, cancellationToken))
            {
                return Result.Failure(CreatorErrors.Create_ExistedCreatorName);
            }

            Creator newCreator = new Creator(request.Name, request.Biography);
            await creatorRepository.AddAsync(newCreator);
            var result = await _unitOfWork.SaveChangeAsync(cancellationToken);

            if (result <= 0)
            {
                return Result.Failure(CreatorErrors.Create_CreateCreatorFailed);
            }

            return Result.SuccessNullError();
        }
    }
}
