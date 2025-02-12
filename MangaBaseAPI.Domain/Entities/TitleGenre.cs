﻿namespace MangaBaseAPI.Domain.Entities
{
    public class TitleGenre
    {
        public Guid TitleId { get; set; }
        public Title Title { get; set; } = default!;

        public int GenreId { get; set; }
        public Genre Genre { get; set; } = default!;

        public TitleGenre()
        {
        }

        public TitleGenre(Guid titleId, int genreId)
        {
            TitleId = titleId;
            GenreId = genreId;
        }
    }
}
