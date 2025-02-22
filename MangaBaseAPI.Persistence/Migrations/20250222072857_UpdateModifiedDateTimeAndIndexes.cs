using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaBaseAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModifiedDateTimeAndIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedDateTime",
                table: "Users",
                type: "datetimeoffset",
                nullable: true,
                defaultValueSql: "SYSDATETIMEOFFSET()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedDateTime",
                table: "Titles",
                type: "datetimeoffset",
                nullable: true,
                defaultValueSql: "SYSDATETIMEOFFSET()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedDateTime",
                table: "PasswordHistories",
                type: "datetimeoffset",
                nullable: true,
                defaultValueSql: "SYSDATETIMEOFFSET()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedDateTime",
                table: "Creators",
                type: "datetimeoffset",
                nullable: true,
                defaultValueSql: "SYSDATETIMEOFFSET()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedDateTime",
                table: "Chapters",
                type: "datetimeoffset",
                nullable: true,
                defaultValueSql: "SYSDATETIMEOFFSET()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "ConcurrencyStamp",
                value: "89900295-fc1c-43b2-b370-1a32e2e9552f");

            migrationBuilder.CreateIndex(
                name: "IX_Titles_PublishedDate",
                table: "Titles",
                column: "PublishedDate");

            migrationBuilder.CreateIndex(
                name: "IX_Titles_Type_Status",
                table: "Titles",
                columns: new[] { "Type", "Status" });

            migrationBuilder.Sql(@"
                CREATE INDEX IX_Title_IsDeleted
                ON Titles (IsDeleted)
                WHERE IsDeleted = 0;
            ");

            migrationBuilder.Sql(@"
                CREATE INDEX IX_Title_IsHidden
                ON Titles (IsHidden)
                WHERE IsHidden = 0;
            ");

            migrationBuilder.Sql(@"
                CREATE INDEX IX_Chapter_IsDeleted
                ON Chapters (IsDeleted)
                WHERE IsDeleted = 0;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Titles_PublishedDate",
                table: "Titles");

            migrationBuilder.DropIndex(
                name: "IX_Titles_Type_Status",
                table: "Titles");

            migrationBuilder.Sql(@"
                DROP INDEX IX_Title_IsDeleted ON Titles;
            ");

            migrationBuilder.Sql(@"
                DROP INDEX IX_Title_IsHidden ON Titles;
            ");

            migrationBuilder.Sql(@"
                DROP INDEX IX_Chapter_IsDeleted ON Chapters;
            ");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedDateTime",
                table: "Users",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldDefaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedDateTime",
                table: "Titles",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldDefaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedDateTime",
                table: "PasswordHistories",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldDefaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedDateTime",
                table: "Creators",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldDefaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ModifiedDateTime",
                table: "Chapters",
                type: "datetimeoffset",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldNullable: true,
                oldDefaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "ConcurrencyStamp",
                value: "414cf2b9-5497-4b72-a5d4-893489bea542");
        }
    }
}
