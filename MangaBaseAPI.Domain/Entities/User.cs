using MangaBaseAPI.Domain.Common.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MangaBaseAPI.Domain.Entities
{
    public class User : IdentityUser<Guid>, ITrackable
    {
        [StringLength(50)]
        public string? FirstName { get; set; }
        [StringLength(50)]
        public string? LastName { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; } = default!;
        public DateTimeOffset CreatedDateTime { get; set; }
        public DateTimeOffset? ModifiedDateTime { get; set; }

        public ICollection<PasswordHistory> PasswordHistories { get; set; } = default!;
    }
}
