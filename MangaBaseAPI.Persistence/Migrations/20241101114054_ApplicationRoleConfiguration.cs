using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MangaBaseAPI.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationRoleConfiguration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("123e4567-e89b-12d3-a456-426614174000"), null, "Administrator", "administrator" },
                    { new Guid("123e4567-e89b-12d3-a456-426614174001"), null, "Member", "member" }
                });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "ConcurrencyStamp",
                value: "a8f66ad5-60dc-45fd-ae04-e679b3cc0161");

            migrationBuilder.InsertData(
                table: "RoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "AccessLevel", "Admin", new Guid("123e4567-e89b-12d3-a456-426614174000") },
                    { 2, "CanManageUsers", "True", new Guid("123e4567-e89b-12d3-a456-426614174000") },
                    { 3, "CanManageContent", "True", new Guid("123e4567-e89b-12d3-a456-426614174000") },
                    { 4, "AccessLevel", "Member", new Guid("123e4567-e89b-12d3-a456-426614174001") },
                    { 5, "CanManageUsers", "False", new Guid("123e4567-e89b-12d3-a456-426614174001") },
                    { 6, "CanManageContent", "False", new Guid("123e4567-e89b-12d3-a456-426614174001") }
                });

            migrationBuilder.InsertData(
                table: "UserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("123e4567-e89b-12d3-a456-426614174000"), new Guid("11111111-1111-1111-1111-111111111111") });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "UserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("123e4567-e89b-12d3-a456-426614174000"), new Guid("11111111-1111-1111-1111-111111111111") });

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("123e4567-e89b-12d3-a456-426614174000"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("123e4567-e89b-12d3-a456-426614174001"));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"),
                column: "ConcurrencyStamp",
                value: "dfd83104-6bf1-4bf4-b48d-15f35be1391e");
        }
    }
}
