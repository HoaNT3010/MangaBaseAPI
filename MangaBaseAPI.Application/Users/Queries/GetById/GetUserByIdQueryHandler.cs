using MangaBaseAPI.Contracts.Users.GetById;
using MangaBaseAPI.Domain.Abstractions;
using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Errors.User;
using MediatR;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using MangaBaseAPI.Domain.Constants.Caching;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace MangaBaseAPI.Application.Users.Queries.GetById
{
    public class GetUserByIdQueryHandler(
        UserManager<User> userManager,
        IMapper mapper,
        IDistributedCache cache)
        : IRequestHandler<GetUserByIdQuery, Result<GetUserByIdResponse>>
    {
        public async Task<Result<GetUserByIdResponse>> Handle(
            GetUserByIdQuery request,
            CancellationToken cancellationToken)
        {
            var cachedData = await cache.GetStringAsync(
                UserCachingConstants.GetByIdKey + request.Id,
                cancellationToken);

            if (!string.IsNullOrWhiteSpace(cachedData))
            {
                return Result.SuccessNullError(JsonConvert.DeserializeObject<GetUserByIdResponse>(cachedData)!);
            }

            var user = await userManager.FindByIdAsync(request.Id.ToString());
            if (user == null)
            {
                return Result.Failure<GetUserByIdResponse>(Error.ErrorWithValue(
                    UserErrors.General_UserNotFound,
                    request.Id));
            }
            var userRoles = await userManager.GetRolesAsync(user);
            var response = mapper.Map<GetUserByIdResponse>(user);
            response.Roles = userRoles;
            await cache.SetStringAsync(
                UserCachingConstants.GetByIdKey + request.Id,
                JsonConvert.SerializeObject(response),
                CachingOptionConstants.DailyCachingOption,
                cancellationToken);
            return Result.SuccessNullError(response);
        }
    }
}
