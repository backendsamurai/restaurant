using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Restaurant.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "desks",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "varchar", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_desks", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "employee_roles",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "varchar", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employee_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    status = table.Column<string>(type: "varchar", nullable: false),
                    bill = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    tip = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_payments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "product_categories",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "varchar", maxLength: 255, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_product_categories", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "varchar", maxLength: 255, nullable: false),
                    email = table.Column<string>(type: "varchar", maxLength: 255, nullable: false),
                    is_verified = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    password_hash = table.Column<string>(type: "varchar", maxLength: 255, nullable: false),
                    role = table.Column<string>(type: "varchar", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "products",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    name = table.Column<string>(type: "varchar", maxLength: 255, nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    image_url = table.Column<string>(type: "varchar", maxLength: 255, nullable: false),
                    old_price = table.Column<decimal>(type: "numeric(18,2)", nullable: true),
                    price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    category_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_products", x => x.id);
                    table.ForeignKey(
                        name: "fk_products_product_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "product_categories",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "customers",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_customers", x => x.id);
                    table.ForeignKey(
                        name: "fk_customers_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "employees",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    role_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_employees", x => x.id);
                    table.ForeignKey(
                        name: "fk_employees_employee_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "employee_roles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_employees_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    customer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    waiter_id = table.Column<Guid>(type: "uuid", nullable: false),
                    desk_id = table.Column<Guid>(type: "uuid", nullable: false),
                    status = table.Column<string>(type: "varchar", nullable: false),
                    payment_id = table.Column<Guid>(type: "uuid", nullable: true),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: false, defaultValueSql: "now()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_orders", x => x.id);
                    table.ForeignKey(
                        name: "fk_orders_customers_customer_id",
                        column: x => x.customer_id,
                        principalTable: "customers",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_orders_desks_desk_id",
                        column: x => x.desk_id,
                        principalTable: "desks",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_orders_employees_waiter_id",
                        column: x => x.waiter_id,
                        principalTable: "employees",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_orders_payments_payment_id",
                        column: x => x.payment_id,
                        principalTable: "payments",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "order_line_items",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false, defaultValueSql: "gen_random_uuid()"),
                    product_id = table.Column<Guid>(type: "uuid", nullable: false),
                    count = table.Column<int>(type: "integer", nullable: false),
                    order_id = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_order_line_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_order_line_items_orders_order_id",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_order_line_items_products_product_id",
                        column: x => x.product_id,
                        principalTable: "products",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "ix_customers_user_id",
                table: "customers",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_desks_name",
                table: "desks",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_employee_roles_name",
                table: "employee_roles",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_employees_role_id",
                table: "employees",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "ix_employees_user_id",
                table: "employees",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_line_items_order_id",
                table: "order_line_items",
                column: "order_id");

            migrationBuilder.CreateIndex(
                name: "ix_order_line_items_product_id",
                table: "order_line_items",
                column: "product_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_customer_id",
                table: "orders",
                column: "customer_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_desk_id",
                table: "orders",
                column: "desk_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_payment_id",
                table: "orders",
                column: "payment_id");

            migrationBuilder.CreateIndex(
                name: "ix_orders_waiter_id",
                table: "orders",
                column: "waiter_id");

            migrationBuilder.CreateIndex(
                name: "ix_product_categories_name",
                table: "product_categories",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_products_category_id",
                table: "products",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_products_name",
                table: "products",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_users_email",
                table: "users",
                column: "email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "order_line_items");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "products");

            migrationBuilder.DropTable(
                name: "customers");

            migrationBuilder.DropTable(
                name: "desks");

            migrationBuilder.DropTable(
                name: "employees");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "product_categories");

            migrationBuilder.DropTable(
                name: "employee_roles");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
