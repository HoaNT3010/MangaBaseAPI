using Microsoft.Extensions.Caching.Distributed;

namespace MangaBaseAPI.Domain.Constants.Caching
{
    public class CachingOptionConstants
    {
        /// <summary>
        /// Caching option for frequently changing resource, with Absolute expiration of <b>1 hour</b> and Sliding expiration of <b>15 minutes</b>.
        /// </summary>
        public static readonly DistributedCacheEntryOptions ShortCachingOption = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
            SlidingExpiration = TimeSpan.FromMinutes(15),
        };

        /// <summary>
        /// Caching option for infrequently changing resource, with Absolute expiration of <b>7 days</b> and Sliding expiration of <b>1 days</b>.
        /// </summary>
        public static readonly DistributedCacheEntryOptions LongCachingOption = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(7),
            SlidingExpiration = TimeSpan.FromDays(1),
        };

        /// <summary>
        /// Caching option for resource with not much changing, with Absolute expiration of <b>1 day</b> and Sliding expiration of <b>1 hour</b>.
        /// </summary>
        public static readonly DistributedCacheEntryOptions MediumCachingOption = new DistributedCacheEntryOptions()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1),
            SlidingExpiration = TimeSpan.FromHours(1),
        };
    }
}
