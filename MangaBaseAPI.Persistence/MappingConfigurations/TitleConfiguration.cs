﻿using MangaBaseAPI.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MangaBaseAPI.Persistence.MappingConfigurations
{
    public class TitleConfiguration : IEntityTypeConfiguration<Title>
    {
        public void Configure(EntityTypeBuilder<Title> builder)
        {
            builder.ToTable("Titles");

            builder.Property(x => x.Name)
                .HasMaxLength(250);

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.Property(x => x.CreatedDateTime)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.Property(x => x.ModifiedDateTime)
                .HasDefaultValueSql("SYSDATETIMEOFFSET()");

            builder.HasOne(x => x.Uploader)
                .WithMany(u => u.UploadedTitles)
                .HasForeignKey(x => x.UploaderId)
                .OnDelete(DeleteBehavior.SetNull);

            builder.Property(x => x.RowVersion)
                .IsRowVersion();

            builder.HasIndex(x => x.Name)
                .IsUnique();
            builder.HasIndex(x => new { x.Type, x.Status });
            builder.HasIndex(x => x.PublishedDate);
        }
    }
}
