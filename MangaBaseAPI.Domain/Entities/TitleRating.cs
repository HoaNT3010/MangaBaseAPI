using System.ComponentModel.DataAnnotations;

namespace MangaBaseAPI.Domain.Entities
{
    public class TitleRating
    {
        public Guid TitleId { get; set; }
        public Title Title { get; set; } = default!;

        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        [Range(1, 10)]
        public int Rating { get; set; }

        public TitleRating(
            Guid titleId,
            Guid userId,
            int rating)
        {
            TitleId = titleId;
            UserId = userId;
            Rating = rating;
        }
    }
}
