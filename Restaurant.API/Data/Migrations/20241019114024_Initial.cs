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
                    customer_id = table.Column<Guid>(type: "uuid", nullable: false),
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
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
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
                    { new Guid("74341376-1fd5-495b-9944-5f31e991ab91"), "manager" },
                    { new Guid("e810e4d5-1887-4ac7-85a1-fa62e216e4e1"), "waiter" }
                });

            migrationBuilder.InsertData(
                table: "product_categories",
                columns: new[] { "id", "name" },
                values: new object[,]
                {
                    { new Guid("04b8c2cd-a9bb-4698-a30e-8ba3d9c170f1"), "Steaks" },
                    { new Guid("1e6b5e24-ee39-441a-bebc-564f8514b616"), "Seafood" },
                    { new Guid("214691df-e1c5-4524-b865-389626eac3af"), "Desserts" },
                    { new Guid("2426ca78-6c8e-42f9-8573-9ab5dac81583"), "Drinks" },
                    { new Guid("486be5f5-c861-430f-8f60-03f9af73b170"), "Sushi" },
                    { new Guid("63102f40-3ae1-4814-848f-64a5b3f21120"), "Pizzas" },
                    { new Guid("99b529fc-18cc-46dc-b2c2-5a69e88834a3"), "Fast Food" },
                    { new Guid("c51130c9-057d-4046-a8d8-6257a6efdb7e"), "Barbecue" },
                    { new Guid("c728710f-7d21-43f8-af7e-edae192fcc5b"), "Hot Dogs" },
                    { new Guid("f2e2837e-c945-43d6-a124-9aea8466c288"), "Coffee" }
                });

            migrationBuilder.InsertData(
                table: "users",
                columns: new[] { "id", "email", "name", "password_hash", "role" },
                values: new object[] { new Guid("cd0e5dd9-5078-471b-ab7e-493d26669ce4"), "victor_samoylov@gmail.com", "Victor Samoylov", "EAABBF23E856BEB7C7CB30704B5DAE880D0E9BDA4C6A23C4D1F1888393AD991C-E67C6986FC4D8B2386090650D4EA513F", "employee" });

            migrationBuilder.InsertData(
                table: "employees",
                columns: new[] { "id", "role_id", "user_id" },
                values: new object[] { new Guid("b0fda4dc-97ec-43f8-b43c-b7c7b3eaf137"), new Guid("74341376-1fd5-495b-9944-5f31e991ab91"), new Guid("cd0e5dd9-5078-471b-ab7e-493d26669ce4") });

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
