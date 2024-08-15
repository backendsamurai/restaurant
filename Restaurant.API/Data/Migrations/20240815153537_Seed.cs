using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurant.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class Seed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "employee_roles",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("9415be67-c498-4cc1-b172-8940cc150b52"), "manager" },
                    { new Guid("e282dd51-115c-4c8d-9628-f73fd824285d"), "waiter" }
                });

            migrationBuilder.InsertData(
                table: "product_categories",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("0ad39dbc-a9f0-4bc4-8e1e-0fe3f095ac5f"), "Fast Food" },
                    { new Guid("4f5cba22-854f-4f37-94dc-d2311aca31a0"), "Desserts" },
                    { new Guid("500a450c-2b8b-4ff0-a6a0-cde8a0ee1303"), "Seafood" },
                    { new Guid("69e358cd-f772-48d1-8138-b6e01bc34d82"), "Drinks" },
                    { new Guid("76ba8479-f89a-43f4-a6a5-9a08d23b1851"), "Pizzas" },
                    { new Guid("7838ad9c-f81f-4474-8657-879e8c29b78f"), "Hot Dogs" },
                    { new Guid("87761332-9a41-4f64-8dc3-119e0256fa7a"), "Barbecue" },
                    { new Guid("da155e36-7d92-4db0-907c-59443b67ec87"), "Sushi" },
                    { new Guid("f456c7f3-cf5e-486c-a649-42fbcc8bab44"), "Coffee" },
                    { new Guid("f9f7c3a7-f9ed-434f-81ee-dbbfed0c1a51"), "Steaks" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "employee_roles",
                keyColumn: "id",
                keyValue: new Guid("9415be67-c498-4cc1-b172-8940cc150b52"));

            migrationBuilder.DeleteData(
                table: "employee_roles",
                keyColumn: "id",
                keyValue: new Guid("e282dd51-115c-4c8d-9628-f73fd824285d"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("0ad39dbc-a9f0-4bc4-8e1e-0fe3f095ac5f"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("4f5cba22-854f-4f37-94dc-d2311aca31a0"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("500a450c-2b8b-4ff0-a6a0-cde8a0ee1303"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("69e358cd-f772-48d1-8138-b6e01bc34d82"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("76ba8479-f89a-43f4-a6a5-9a08d23b1851"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("7838ad9c-f81f-4474-8657-879e8c29b78f"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("87761332-9a41-4f64-8dc3-119e0256fa7a"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("da155e36-7d92-4db0-907c-59443b67ec87"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("f456c7f3-cf5e-486c-a649-42fbcc8bab44"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("f9f7c3a7-f9ed-434f-81ee-dbbfed0c1a51"));
        }
    }
}
