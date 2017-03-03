using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using CakeExchange.Data;

namespace CakeExchange.Migrations
{
    [DbContext(typeof(ExchangeContext))]
    partial class ExchangeContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CakeExchange.Models.Buy", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Comment");

                    b.Property<DateTime>("Date");

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

                    b.Property<string>("Comment");

                    b.Property<DateTime>("Date");

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

                    b.Property<int>("SellId");

                    b.Property<int>("Size");

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
