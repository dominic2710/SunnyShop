﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using SellManagement.Api.Entities;

namespace SellManagement.Api.Migrations
{
    [DbContext(typeof(SellManagementContext))]
    [Migration("20220312091506_NEW_INIT")]
    partial class NEW_INIT
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 63)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            modelBuilder.Entity("SellManagement.Api.Entities.TblClassifyName", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Code")
                        .HasColumnType("integer");

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TblClassifiesName");
                });

            modelBuilder.Entity("SellManagement.Api.Entities.TblCustomer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Address1")
                        .HasColumnType("text");

                    b.Property<string>("Address2")
                        .HasColumnType("text");

                    b.Property<int>("CategoryCd")
                        .HasColumnType("integer");

                    b.Property<string>("CustomerCd")
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Facebook")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TblCustomers");
                });

            modelBuilder.Entity("SellManagement.Api.Entities.TblProduct", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Barcode")
                        .HasColumnType("text");

                    b.Property<int>("CategoryCd")
                        .HasColumnType("integer");

                    b.Property<int>("CostPrice")
                        .HasColumnType("integer");

                    b.Property<string>("Detail")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("OriginCd")
                        .HasColumnType("integer");

                    b.Property<string>("ProductCd")
                        .HasColumnType("text");

                    b.Property<int>("SoldPrice")
                        .HasColumnType("integer");

                    b.Property<int>("TradeMarkCd")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("TblProducts");
                });

            modelBuilder.Entity("SellManagement.Api.Entities.TblProductCombo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("ProductCd")
                        .HasColumnType("text");

                    b.Property<string>("ProductComboCd")
                        .HasColumnType("text");

                    b.Property<int>("Quatity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("TblProductCombos");
                });

            modelBuilder.Entity("SellManagement.Api.Entities.TblProductInventory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("AvailabilityInStock")
                        .HasColumnType("integer");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("text");

                    b.Property<int>("InStock")
                        .HasColumnType("integer");

                    b.Property<int>("PlannedInpStock")
                        .HasColumnType("integer");

                    b.Property<int>("PlannedOutStock")
                        .HasColumnType("integer");

                    b.Property<string>("ProductCd")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UpdateUserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TblProductInventories");
                });

            modelBuilder.Entity("SellManagement.Api.Entities.TblPurchaseOrderDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("Cost")
                        .HasColumnType("integer");

                    b.Property<int>("CostPrice")
                        .HasColumnType("integer");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<string>("ProductCd")
                        .HasColumnType("text");

                    b.Property<string>("PurchaseOrderNo")
                        .HasColumnType("text");

                    b.Property<int>("Quantity")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("TblPurchaseOrderDetails");
                });

            modelBuilder.Entity("SellManagement.Api.Entities.TblPurchaseOrderHead", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("text");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<int>("PaidCost")
                        .HasColumnType("integer");

                    b.Property<DateTime>("PlannedImportDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("PurchaseCost")
                        .HasColumnType("integer");

                    b.Property<DateTime>("PurchaseOrderDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("PurchaseOrderNo")
                        .HasColumnType("text");

                    b.Property<int>("SaleOffCost")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<int>("SummaryCost")
                        .HasColumnType("integer");

                    b.Property<string>("SupplierCd")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UpdateUserId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TblPurchaseOrderHeads");
                });

            modelBuilder.Entity("SellManagement.Api.Entities.TblSupplier", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Address1")
                        .HasColumnType("text");

                    b.Property<string>("Address2")
                        .HasColumnType("text");

                    b.Property<int>("CategoryCd")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .HasColumnType("text");

                    b.Property<string>("Facebook")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Note")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<string>("SupplierCd")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TblSuppliers");
                });

            modelBuilder.Entity("SellManagement.Api.Entities.TblUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("LoginId")
                        .HasColumnType("text");

                    b.Property<string>("Password")
                        .HasColumnType("text");

                    b.Property<string>("UserName")
                        .HasColumnType("text");

                    b.Property<string>("UserRole")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TblUsers");
                });

            modelBuilder.Entity("SellManagement.Api.Entities.TblVoucherNoManagement", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<int>("CategoryCd")
                        .HasColumnType("integer");

                    b.Property<string>("CategoryName")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreateDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("CreateUserId")
                        .HasColumnType("text");

                    b.Property<DateTime>("UpdateDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("UpdateUserId")
                        .HasColumnType("text");

                    b.Property<int>("VoucherNo")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("TblVoucherNoManagements");
                });
#pragma warning restore 612, 618
        }
    }
}
