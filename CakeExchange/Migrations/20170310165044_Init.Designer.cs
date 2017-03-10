using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CakeExchange.Data;
using CakeExchange.Models;

namespace CakeExchange.Migrations
{
    [DbContext(typeof(ExchangeContext))]
    [Migration("20170310165044_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CakeExchange.Models.Buy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<bool>("IsActive");

                    b.Property<int>("Number");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("Id");

                    b.ToTable("BuyOrders");
                });

            modelBuilder.Entity("CakeExchange.Models.Sell", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<bool>("IsActive");

                    b.Property<int>("Number");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10,2)");

                    b.HasKey("Id");

                    b.ToTable("SellOrders");
                });

            modelBuilder.Entity("CakeExchange.Models.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("BuyId");

                    b.Property<DateTime>("Date");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(10,2)");

                    b.Property<int>("SellId");

                    b.Property<int>("Size");

                    b.Property<int>("State");

                    b.HasKey("Id");

                    b.HasIndex("BuyId");

                    b.HasIndex("SellId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("CakeExchange.Models.Transaction", b =>
                {
                    b.HasOne("CakeExchange.Models.Buy", "Buy")
                        .WithMany()
                        .HasForeignKey("BuyId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("CakeExchange.Models.Sell", "Sell")
                        .WithMany()
                        .HasForeignKey("SellId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
