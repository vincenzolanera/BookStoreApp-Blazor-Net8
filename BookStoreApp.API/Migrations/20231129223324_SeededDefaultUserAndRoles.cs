using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BookStoreApp.API.Migrations
{
    /// <inheritdoc />
    public partial class SeededDefaultUserAndRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "722bccad-d175-4e0f-b12b-482687e1a2a5", null, "Administrator", "ADMINISTRATOR" },
                    { "ff0da9be-f14b-4d64-bc28-6019c3a0faba", null, "User", "USER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[,]
                {
                    { "86028e29-d15c-48c6-ac1f-6fc99bbf5118", 0, "3ecdac06-e796-4d92-a2b1-bdc0dcc41cc7", "user@bookstore.com", false, "System", "User", false, null, "USER@BOOKSTORE.COM", "USER@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEKjXD3WzKXFpzax8wfxWdVn7yNFVpD1PQdGuT06hpRNT5EOrqHRtru+TjwHvvOiIIg==", null, false, "5b6a2da0-88b8-4f08-8c53-2f3fbe8ce12b", false, "user@bookstore.com" },
                    { "96268aac-8802-45a0-afa0-9545e97abfcb", 0, "8a7493aa-6bbd-4fd5-a429-d9c08b35581b", "admin@bookstore.com", false, "System", "Admin", false, null, "ADMIN@BOOKSTORE.COM", "ADMIN@BOOKSTORE.COM", "AQAAAAIAAYagAAAAEOlLwbyOtMkrijS82ZQWUytsn/gXqGD1OM25f6m9HRskaEKhuO8lQbnachrql/PavA==", null, false, "9cfeb987-67c2-4338-82bb-36e3a4ca03b7", false, "admin@bookstore.com" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "ff0da9be-f14b-4d64-bc28-6019c3a0faba", "86028e29-d15c-48c6-ac1f-6fc99bbf5118" },
                    { "722bccad-d175-4e0f-b12b-482687e1a2a5", "96268aac-8802-45a0-afa0-9545e97abfcb" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "ff0da9be-f14b-4d64-bc28-6019c3a0faba", "86028e29-d15c-48c6-ac1f-6fc99bbf5118" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "722bccad-d175-4e0f-b12b-482687e1a2a5", "96268aac-8802-45a0-afa0-9545e97abfcb" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "722bccad-d175-4e0f-b12b-482687e1a2a5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ff0da9be-f14b-4d64-bc28-6019c3a0faba");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "86028e29-d15c-48c6-ac1f-6fc99bbf5118");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "96268aac-8802-45a0-afa0-9545e97abfcb");
        }
    }
}
