using MangaBaseAPI.Domain.Common.Models;
using MangaBaseAPI.Domain.Enums;

namespace MangaBaseAPI.Domain.Entities
{
    public class Title : Entity<Guid>
    {
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
        public TitleType Type { get; set; }
        public TitleStatus Status { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTimeOffset? PublishedDate { get; set; }
        public float AverageRating { get; set; } = 0;
        public string? CoverImageUrl { get; set; }
        public bool IsHidden { get; set; } = true;

        // UserId and User
        public Guid? UploaderId { get; set; }
        public User Uploader { get; set; } = default!;

        public ICollection<TitleGenre> TitleGenres { get; set; } = default!;
        public ICollection<TitleRating> TitleRatings { get; set; } = default!;
        public ICollection<AlternativeName> AlternativeNames { get; set; } = default!;
        public ICollection<Chapter> Chapters { get; set; } = default!;
        public ICollection<TitleAuthor> TitleAuthors { get; set; } = default!;
        public ICollection<TitleArtist> TitleArtists { get; set; } = default!;

        public Title()
        {
        }

        public Title(
            Guid id,
            string name,
            string? description,
            int titleType,
            int titleStatus,
            DateTimeOffset? publishedDate,
            Guid uploaderId)
        {
            Id = id;
            Name = name;
            Description = description;
            Type = (TitleType)titleType;
            Status = (TitleStatus)titleStatus;
            PublishedDate = publishedDate;
            UploaderId = uploaderId;

            // Should newly created title be hidden?
            IsHidden = false;
            IsDeleted = false;
            AverageRating = 0;
        }
    }
}
