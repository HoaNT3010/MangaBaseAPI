using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaBaseAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PasswordHistoryConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDateTime",
                table: "PasswordHistories",
                type: "datetimeoffset",
                nullable: false,
                defaultValueSql: "SYSDATETIMEOFFSET()",
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset");

            migrationBuilder.InsertData(
                table: "PasswordHistories",
                columns: new[] { "Id", "ModifiedDateTime", "PasswordHash", "UserId" },
                values: new object[] { new Guid("22222222-1111-1111-1111-111111111111"), null, "AQAAAAIAAYagAAAAENX7BIlY1gy8Getg2rmVWj0zLEDmvNY8m7TEJETG6JYBfWbiKN41/MgUaiU8N03GRw==", new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName" },
                values: new object[] { "477a1c83-33ce-4c83-91f0-8aa5a76f8afb", "HOA41300@GMAIL.COM", "GIBLEBC2" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "PasswordHistories",
                keyColumn: "Id",
                keyValue: new Guid("22222222-1111-1111-1111-111111111111"));

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "CreatedDateTime",
                table: "PasswordHistories",
                type: "datetimeoffset",
                nullable: false,
                oldClrType: typeof(DateTimeOffset),
                oldType: "datetimeoffset",
                oldDefaultValueSql: "SYSDATETIMEOFFSET()");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                columns: new[] { "ConcurrencyStamp", "NormalizedEmail", "NormalizedUserName" },
                values: new object[] { "a8f66ad5-60dc-45fd-ae04-e679b3cc0161", "hoa41300@gmail.com", "giblebc2" });
        }
    }
}
