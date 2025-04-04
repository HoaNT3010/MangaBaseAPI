﻿using FluentValidation;

namespace MangaBaseAPI.Application.Titles.Commands.UpdateGenres
{
    public class UpdateTitleGenresCommandValidator : AbstractValidator<UpdateTitleGenresCommand>
    {
        public UpdateTitleGenresCommandValidator()
        {
            ClassLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.Genres)
                .NotNull().WithMessage("Title's genres cannot be null")
                .Must(x => x == null || x.Distinct().Count() == x.Count).WithMessage("Title's genres cannot contains duplicate(s)")
                .Must(x => x == null || x.Count <= 50).WithMessage("Title cannot have more than 50 genres");

            RuleForEach(x => x.Genres)
                .GreaterThan(0).WithMessage("Genre Id must be greater than 0");
        }
    }
}
