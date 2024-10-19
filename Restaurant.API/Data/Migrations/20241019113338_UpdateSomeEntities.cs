using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurant.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateSomeEntities : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_orders_customers_customer_id",
                table: "orders");

            migrationBuilder.DeleteData(
                table: "employee_roles",
                keyColumn: "id",
                keyValue: new Guid("abe471a9-b706-4794-abd7-0c2d518d2abe"));

            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "id",
                keyValue: new Guid("10092dfa-fe46-4b04-bba5-5da74eec3807"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("08847cf3-2d2b-4c19-8aac-93f1cbcb04a5"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("1f958f7e-f235-49ef-9f9e-9d2387eecb02"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("25b37daa-5f3f-4f96-a863-092c3e8b202f"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("2cdf4bf5-1b80-4ebe-a016-b07825c438f2"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("4ac43dc6-f2b3-4310-92ed-867fce5c76a4"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("6784cb9b-7208-48b8-9c37-afef9d20388b"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("7e990a71-fbab-4378-95bd-3b0dffa13a47"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("9130a129-41fa-4465-b229-20bc4eedba4d"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("9c050383-d937-403d-8ae5-a50fb28eacd2"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("a8c40386-b940-4602-aeb7-f9542737ffbd"));

            migrationBuilder.DeleteData(
                table: "employee_roles",
                keyColumn: "id",
                keyValue: new Guid("98d6964a-e88d-44af-86e4-0c58240bd371"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("fd92af47-10be-4689-b28f-796cad40a350"));

            migrationBuilder.AlterColumn<Guid>(
                name: "customer_id",
                table: "orders",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "employee_roles",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("9363619a-20af-4458-9053-feb5c685d296"), "manager" },
                    { new Guid("adfa83aa-152f-4819-bb73-bc1f75e2135a"), "waiter" }
                });

            migrationBuilder.InsertData(
                table: "product_categories",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("32d96677-1a00-492e-b9f9-0eae18575836"), "Sushi" },
                    { new Guid("a2357c65-24d9-4a74-aed5-a69aa9bb7b8c"), "Hot Dogs" },
                    { new Guid("aac5232d-dd06-4604-ad43-2de5d1fbe6e9"), "Fast Food" },
                    { new Guid("b70e2cc9-0993-45a4-b068-62f17b1f8598"), "Barbecue" },
                    { new Guid("bde472f6-5e7e-4541-bfe4-65ea68477e1f"), "Desserts" },
                    { new Guid("d444b910-4c55-4fa2-8a65-35792863c4e9"), "Steaks" },
                    { new Guid("dba64614-22e3-4261-825f-5f397f0d00b3"), "Seafood" },
                    { new Guid("e45823e0-3eda-41c7-9179-99cc73ec81ac"), "Pizzas" },
                    { new Guid("e858e449-5f08-4421-aac3-39f39c884c8e"), "Drinks" },
                    { new Guid("f236f911-d9ec-4ae0-8f65-14d68a0d157f"), "Coffee" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "name", "password_hash", "role" },
                values: new object[] { new Guid("6ae550e5-a6f7-44e9-9e1e-e3ad1e687a6b"), "victor_samoylov@gmail.com", "Victor Samoylov", "10E8011FBE7A140C8D13D65B8154DD7A7CE55C071414106D8FB4DC4A263530C4-7079F6E8EE80788B417F1DCD3EF95B13", "employee" });

            migrationBuilder.InsertData(
                table: "employees",
                columns: new[] { "id", "role_id", "user_id" },
                values: new object[] { new Guid("83e3c44a-3547-4718-b747-4224b8119488"), new Guid("9363619a-20af-4458-9053-feb5c685d296"), new Guid("6ae550e5-a6f7-44e9-9e1e-e3ad1e687a6b") });

            migrationBuilder.AddForeignKey(
                name: "fk_orders_customers_customer_id",
                table: "orders",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_orders_customers_customer_id",
                table: "orders");

            migrationBuilder.DeleteData(
                table: "employee_roles",
                keyColumn: "id",
                keyValue: new Guid("adfa83aa-152f-4819-bb73-bc1f75e2135a"));

            migrationBuilder.DeleteData(
                table: "employees",
                keyColumn: "id",
                keyValue: new Guid("83e3c44a-3547-4718-b747-4224b8119488"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("32d96677-1a00-492e-b9f9-0eae18575836"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("a2357c65-24d9-4a74-aed5-a69aa9bb7b8c"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("aac5232d-dd06-4604-ad43-2de5d1fbe6e9"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("b70e2cc9-0993-45a4-b068-62f17b1f8598"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("bde472f6-5e7e-4541-bfe4-65ea68477e1f"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("d444b910-4c55-4fa2-8a65-35792863c4e9"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("dba64614-22e3-4261-825f-5f397f0d00b3"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("e45823e0-3eda-41c7-9179-99cc73ec81ac"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("e858e449-5f08-4421-aac3-39f39c884c8e"));

            migrationBuilder.DeleteData(
                table: "product_categories",
                keyColumn: "id",
                keyValue: new Guid("f236f911-d9ec-4ae0-8f65-14d68a0d157f"));

            migrationBuilder.DeleteData(
                table: "employee_roles",
                keyColumn: "id",
                keyValue: new Guid("9363619a-20af-4458-9053-feb5c685d296"));

            migrationBuilder.DeleteData(
                table: "users",
                keyColumn: "id",
                keyValue: new Guid("6ae550e5-a6f7-44e9-9e1e-e3ad1e687a6b"));

            migrationBuilder.AlterColumn<Guid>(
                name: "customer_id",
                table: "orders",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.InsertData(
                table: "employee_roles",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("98d6964a-e88d-44af-86e4-0c58240bd371"), "manager" },
                    { new Guid("abe471a9-b706-4794-abd7-0c2d518d2abe"), "waiter" }
                });

            migrationBuilder.InsertData(
                table: "product_categories",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("08847cf3-2d2b-4c19-8aac-93f1cbcb04a5"), "Sushi" },
                    { new Guid("1f958f7e-f235-49ef-9f9e-9d2387eecb02"), "Barbecue" },
                    { new Guid("25b37daa-5f3f-4f96-a863-092c3e8b202f"), "Fast Food" },
                    { new Guid("2cdf4bf5-1b80-4ebe-a016-b07825c438f2"), "Hot Dogs" },
                    { new Guid("4ac43dc6-f2b3-4310-92ed-867fce5c76a4"), "Seafood" },
                    { new Guid("6784cb9b-7208-48b8-9c37-afef9d20388b"), "Desserts" },
                    { new Guid("7e990a71-fbab-4378-95bd-3b0dffa13a47"), "Pizzas" },
                    { new Guid("9130a129-41fa-4465-b229-20bc4eedba4d"), "Steaks" },
                    { new Guid("9c050383-d937-403d-8ae5-a50fb28eacd2"), "Coffee" },
                    { new Guid("a8c40386-b940-4602-aeb7-f9542737ffbd"), "Drinks" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "name", "password_hash", "role" },
                values: new object[] { new Guid("fd92af47-10be-4689-b28f-796cad40a350"), "victor_samoylov@gmail.com", "Victor Samoylov", "A253E48651FC84595B0EB5A498205E9C43E791AB3C21B5A5AEA02E56C3309FB8-862C21CDB4435E527F321B9FC0507B43", "employee" });

            migrationBuilder.InsertData(
                table: "employees",
                columns: new[] { "id", "role_id", "user_id" },
                values: new object[] { new Guid("10092dfa-fe46-4b04-bba5-5da74eec3807"), new Guid("98d6964a-e88d-44af-86e4-0c58240bd371"), new Guid("fd92af47-10be-4689-b28f-796cad40a350") });

            migrationBuilder.AddForeignKey(
                name: "fk_orders_customers_customer_id",
                table: "orders",
                column: "customer_id",
                principalTable: "customers",
                principalColumn: "id");
        }
    }
}
