﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Restaurant.API.Data;

#nullable disable

namespace Restaurant.API.Data.Migrations
{
    [DbContext(typeof(RestaurantDbContext))]
    partial class RestaurantDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Restaurant.Domain.Customer", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("gen_random_uuid()");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<bool>("IsVerified")
                        .HasColumnType("boolean")
                        .HasColumnName("is_verified");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.HasKey("Id")
                        .HasName("pk_customers");

                    b.ToTable("customers", (string)null);
                });

            modelBuilder.Entity("Restaurant.Domain.Order", b =>
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

                    b.Property<Guid>("CustomerId")
                        .HasColumnType("uuid")
                        .HasColumnName("customer_id");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("varchar")
                        .HasColumnName("status");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp")
                        .HasColumnName("updated_at")
                        .HasDefaultValueSql("now()");

                    b.HasKey("Id")
                        .HasName("pk_orders");

                    b.HasIndex("CustomerId")
                        .HasDatabaseName("ix_orders_customer_id");

                    b.ToTable("orders", (string)null);
                });

            modelBuilder.Entity("Restaurant.Domain.OrderLineItem", b =>
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

            modelBuilder.Entity("Restaurant.Domain.Payment", b =>
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

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uuid")
                        .HasColumnName("order_id");

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

                    b.HasIndex("OrderId")
                        .IsUnique()
                        .HasDatabaseName("ix_payments_order_id");

                    b.ToTable("payments", (string)null);
                });

            modelBuilder.Entity("Restaurant.Domain.Product", b =>
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

            modelBuilder.Entity("Restaurant.Domain.ProductCategory", b =>
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

                    b.HasData(
                        new
                        {
                            Id = new Guid("d7f98551-3bb8-4f42-8d04-b520e024f638"),
                            Name = "Seafood"
                        },
                        new
                        {
                            Id = new Guid("55d15628-f5c8-4540-bf06-5bac43914b14"),
                            Name = "Steaks"
                        },
                        new
                        {
                            Id = new Guid("a191e59f-16d1-4c91-ac7d-c755085cecaa"),
                            Name = "Sushi"
                        },
                        new
                        {
                            Id = new Guid("d3deabee-8072-471f-a693-75eaeb46d272"),
                            Name = "Barbecue"
                        },
                        new
                        {
                            Id = new Guid("ec46b9b2-472e-49fd-a6e8-4134ced9612a"),
                            Name = "Hot Dogs"
                        },
                        new
                        {
                            Id = new Guid("baddbe0b-89b6-49ce-b48f-86ee476842d0"),
                            Name = "Pizzas"
                        },
                        new
                        {
                            Id = new Guid("1bc7481a-3c30-4505-bf03-f23a12ad1ed9"),
                            Name = "Drinks"
                        },
                        new
                        {
                            Id = new Guid("a9c107b7-8ee8-4446-9ec0-823081a72c69"),
                            Name = "Coffee"
                        },
                        new
                        {
                            Id = new Guid("a4563e90-9d5d-41c5-a97f-50a86b5d29b0"),
                            Name = "Fast Food"
                        },
                        new
                        {
                            Id = new Guid("4517150a-a5eb-4798-a3dc-8904dbc73edf"),
                            Name = "Desserts"
                        });
                });

            modelBuilder.Entity("Restaurant.Domain.Order", b =>
                {
                    b.HasOne("Restaurant.Domain.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_orders_customers_customer_id");

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("Restaurant.Domain.OrderLineItem", b =>
                {
                    b.HasOne("Restaurant.Domain.Order", null)
                        .WithMany("Items")
                        .HasForeignKey("OrderId")
                        .HasConstraintName("fk_order_line_items_orders_order_id");

                    b.HasOne("Restaurant.Domain.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_order_line_items_products_product_id");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("Restaurant.Domain.Payment", b =>
                {
                    b.HasOne("Restaurant.Domain.Order", "Order")
                        .WithOne("Payment")
                        .HasForeignKey("Restaurant.Domain.Payment", "OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_payments_orders_order_id");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("Restaurant.Domain.Product", b =>
                {
                    b.HasOne("Restaurant.Domain.ProductCategory", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_products_product_categories_category_id");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Restaurant.Domain.Order", b =>
                {
                    b.Navigation("Items");

                    b.Navigation("Payment");
                });
#pragma warning restore 612, 618
        }
    }
}
