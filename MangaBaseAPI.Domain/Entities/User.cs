﻿using MangaBaseAPI.Domain.Common.Models;
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
        public ICollection<Title> UploadedTitles { get; set; } = default!;
        public ICollection<TitleRating> TitleRatings { get; set; } = default!;
        public ICollection<Chapter> UploadedChapters { get; set; } = default!;

        public User()
        {
        }

        public User(Guid id,
            string userName,
            string email)
        {
            Id = id;
            UserName = userName;
            NormalizedUserName = userName.ToUpper();
            Email = email;
            NormalizedEmail = email.ToUpper();
            LockoutEnabled = true;
            SecurityStamp = Guid.NewGuid().ToString();
            FirstName = null;
            LastName = null;
        }

        public void SetInitialPassword(string hashedPassword)
        {
            PasswordHash = hashedPassword;
            PasswordHistories = new List<PasswordHistory>()
            {
                new PasswordHistory {
                    UserId = Id,
                    PasswordHash = hashedPassword,
                }
            };
        }

        public void SetModifyDateTime()
        {
            ModifiedDateTime = DateTimeOffset.Now;
        }

        public bool IsUserCurrentlyLockedOut()
        {
            return LockoutEnabled && LockoutEnd.HasValue && LockoutEnd.Value.ToUniversalTime() > DateTimeOffset.UtcNow.ToUniversalTime();
        }
    }
}
