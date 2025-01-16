using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaBaseAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTokenAndLoginExpiry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "TokenExpiry",
                table: "UserTokens",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "LoginExpiry",
                table: "UserLogins",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "ConcurrencyStamp",
                value: "414cf2b9-5497-4b72-a5d4-893489bea542");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TokenExpiry",
                table: "UserTokens");

            migrationBuilder.DropColumn(
                name: "LoginExpiry",
                table: "UserLogins");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "ConcurrencyStamp",
                value: "04482017-67cd-41ac-b96c-b26173c74118");
        }
    }
}
