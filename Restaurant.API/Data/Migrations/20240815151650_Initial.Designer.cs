﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Restaurant.API.Data;

#nullable disable

namespace Restaurant.API.Data.Migrations
{
    [DbContext(typeof(RestaurantDbContext))]
    [Migration("20240815151650_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Restaurant.API.Entities.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .HasColumnName("name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .HasColumnName("password_hash");

                    b.HasKey("Id")
                        .HasName("pk_customers");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_customers_email");

                    b.ToTable("customers", (string)null);
                });

            modelBuilder.Entity("Restaurant.API.Entities.Desk", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_desks");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_desks_name");

                    b.ToTable("desks", (string)null);
                });

            modelBuilder.Entity("Restaurant.API.Entities.Employee", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .HasColumnName("name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .HasColumnName("password_hash");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid")
                        .HasColumnName("role_id");

                    b.HasKey("Id")
                        .HasName("pk_employees");

                    b.HasIndex("Email")
                        .IsUnique()
                        .HasDatabaseName("ix_employees_email");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_employees_role_id");

                    b.ToTable("employees", (string)null);
                });

            modelBuilder.Entity("Restaurant.API.Entities.EmployeeRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_employee_roles");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_employee_roles_name");

                    b.ToTable("employee_roles", (string)null);
                });

            modelBuilder.Entity("Restaurant.API.Entities.Order", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid?>("CustomerId")
                        .HasColumnType("uuid")
                        .HasColumnName("customer_id");

                    b.Property<Guid>("DeskId")
                        .HasColumnType("uuid")
                        .HasColumnName("desk_id");

                    b.Property<Guid?>("PaymentId")
                        .HasColumnType("uuid")
                        .HasColumnName("payment_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar")
                        .HasColumnName("status");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("now()");

                    b.Property<Guid>("WaiterId")
                        .HasColumnType("uuid")
                        .HasColumnName("waiter_id");

                    b.HasKey("Id")
                        .HasName("pk_orders");

                    b.HasIndex("CustomerId")
                        .HasDatabaseName("ix_orders_customer_id");

                    b.HasIndex("DeskId")
                        .HasDatabaseName("ix_orders_desk_id");

                    b.HasIndex("PaymentId")
                        .HasDatabaseName("ix_orders_payment_id");

                    b.HasIndex("WaiterId")
                        .HasDatabaseName("ix_orders_waiter_id");

                    b.ToTable("orders", (string)null);
                });

            modelBuilder.Entity("Restaurant.API.Entities.OrderLineItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<int>("Count")
                        .HasColumnType("integer")
                        .HasColumnName("count");

                    b.Property<Guid?>("OrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("order_id");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("uuid")
                        .HasColumnName("product_id");

                    b.HasKey("Id")
                        .HasName("pk_order_line_items");

                    b.HasIndex("OrderId")
                        .HasDatabaseName("ix_order_line_items_order_id");

                    b.HasIndex("ProductId")
                        .HasDatabaseName("ix_order_line_items_product_id");

                    b.ToTable("order_line_items", (string)null);
                });

            modelBuilder.Entity("Restaurant.API.Entities.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<decimal>("Bill")
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("bill");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasColumnName("created_at")
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar")
                        .HasColumnName("status");

                    b.Property<decimal?>("Tip")
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("tip");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("now()");

                    b.HasKey("Id")
                        .HasName("pk_payments");

                    b.ToTable("payments", (string)null);
                });

            modelBuilder.Entity("Restaurant.API.Entities.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<Guid>("CategoryId")
                        .HasColumnType("uuid")
                        .HasColumnName("category_id");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("ImageUrl")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .HasColumnName("image_url");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .HasColumnName("name");

                    b.Property<decimal?>("OldPrice")
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("old_price");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("price");

                    b.HasKey("Id")
                        .HasName("pk_products");

                    b.HasIndex("CategoryId")
                        .HasDatabaseName("ix_products_category_id");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_products_name");

                    b.ToTable("products", (string)null);
                });

            modelBuilder.Entity("Restaurant.API.Entities.ProductCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("varchar")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_product_categories");

                    b.HasIndex("Name")
                        .IsUnique()
                        .HasDatabaseName("ix_product_categories_name");

                    b.ToTable("product_categories", (string)null);
                });

            modelBuilder.Entity("Restaurant.API.Entities.Employee", b =>
                {
                    b.HasOne("Restaurant.API.Entities.EmployeeRole", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_employees_employee_roles_role_id");

                    b.Navigation("Role");
                });

            modelBuilder.Entity("Restaurant.API.Entities.Order", b =>
                {
                    b.HasOne("Restaurant.API.Entities.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .HasConstraintName("fk_orders_customers_customer_id");

                    b.HasOne("Restaurant.API.Entities.Desk", "Desk")
                        .WithMany()
                        .HasForeignKey("DeskId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_orders_desks_desk_id");

                    b.HasOne("Restaurant.API.Entities.Payment", "Payment")
                        .WithMany()
                        .HasForeignKey("PaymentId")
                        .HasConstraintName("fk_orders_payments_payment_id");

                    b.HasOne("Restaurant.API.Entities.Employee", "Waiter")
                        .WithMany()
                        .HasForeignKey("WaiterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_orders_employees_waiter_id");

                    b.Navigation("Customer");

                    b.Navigation("Desk");

                    b.Navigation("Payment");

                    b.Navigation("Waiter");
                });

            modelBuilder.Entity("Restaurant.API.Entities.OrderLineItem", b =>
                {
                    b.HasOne("Restaurant.API.Entities.Order", null)
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .HasConstraintName("fk_order_line_items_orders_order_id");

                    b.HasOne("Restaurant.API.Entities.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_order_line_items_products_product_id");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Restaurant.API.Entities.Product", b =>
                {
                    b.HasOne("Restaurant.API.Entities.ProductCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_products_product_categories_category_id");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Restaurant.API.Entities.Order", b =>
                {
                    b.Navigation("Items");
                });
#pragma warning restore 612, 618
        }
    }
}
