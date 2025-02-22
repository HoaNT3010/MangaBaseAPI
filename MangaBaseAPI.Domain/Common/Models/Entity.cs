using System.ComponentModel.DataAnnotations;

namespace MangaBaseAPI.Domain.Common.Models
{
    public abstract class Entity<TKey> : IHasKey<TKey>, ITrackable
    {
        public TKey Id { get; set; } = default!;

        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;
        public DateTimeOffset CreatedDateTime { get; set; }
        public DateTimeOffset? ModifiedDateTime { get; set; }

        public virtual void SetModifyDateTime()
        {
            ModifiedDateTime = DateTimeOffset.UtcNow;
        }
    }
}
