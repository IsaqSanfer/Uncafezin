﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using UncafezinDAL;

#nullable disable

namespace UncafezinDAL.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20241003004910_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("UncafezinEntities.Category", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Code"));

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Code");

                    b.ToTable("Category");
                });

            modelBuilder.Entity("UncafezinEntities.Client", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Code"));

                    b.Property<string>("CNPJ_CPF")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Cellphone")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Code");

                    b.ToTable("Client");
                });

            modelBuilder.Entity("UncafezinEntities.Order", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Code"));

                    b.Property<int>("CodeClient")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("Total")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("Code");

                    b.HasIndex("CodeClient");

                    b.ToTable("Order");
                });

            modelBuilder.Entity("UncafezinEntities.OrderDetail", b =>
                {
                    b.Property<int>("CodeOrder")
                        .HasColumnType("int");

                    b.Property<int>("CodeProduct")
                        .HasColumnType("int");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.Property<decimal>("TotalValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("UnitValue")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("CodeOrder", "CodeProduct");

                    b.HasIndex("CodeProduct");

                    b.ToTable("OrderDetail");
                });

            modelBuilder.Entity("UncafezinEntities.Product", b =>
                {
                    b.Property<int>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Code"));

                    b.Property<int>("CodeCategory")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.Property<double>("Quantity")
                        .HasColumnType("float");

                    b.HasKey("Code");

                    b.HasIndex("CodeCategory");

                    b.ToTable("Product");
                });

            modelBuilder.Entity("UncafezinEntities.User", b =>
                {
                    b.Property<int?>("Code")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int?>("Code"));

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Code");

                    b.ToTable("User");
                });

            modelBuilder.Entity("UncafezinEntities.Order", b =>
                {
                    b.HasOne("UncafezinEntities.Client", "Client")
                        .WithMany("Orders")
                        .HasForeignKey("CodeClient")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Client");
                });

            modelBuilder.Entity("UncafezinEntities.OrderDetail", b =>
                {
                    b.HasOne("UncafezinEntities.Order", "Order")
                        .WithMany("Products")
                        .HasForeignKey("CodeOrder")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("UncafezinEntities.Product", "Product")
                        .WithMany("Orders")
                        .HasForeignKey("CodeProduct")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Order");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("UncafezinEntities.Product", b =>
                {
                    b.HasOne("UncafezinEntities.Category", "Category")
                        .WithMany("Products")
                        .HasForeignKey("CodeCategory")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("UncafezinEntities.Category", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("UncafezinEntities.Client", b =>
                {
                    b.Navigation("Orders");
                });

            modelBuilder.Entity("UncafezinEntities.Order", b =>
                {
                    b.Navigation("Products");
                });

            modelBuilder.Entity("UncafezinEntities.Product", b =>
                {
                    b.Navigation("Orders");
                });
#pragma warning restore 612, 618
        }
    }
}
