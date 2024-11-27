using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MangaBaseAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateEntityTitle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsHidden",
                table: "Titles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "ConcurrencyStamp",
                value: "23d171cb-3b05-49ac-8427-e386af2aadf4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsHidden",
                table: "Titles");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "ConcurrencyStamp",
                value: "81cde581-93af-472e-8636-8556166cc6ce");
        }
    }
}
