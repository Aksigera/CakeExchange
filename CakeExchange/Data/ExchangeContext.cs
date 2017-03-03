using CakeExchange.Models;
using EntityFrameworkCore.Triggers;
using Microsoft.EntityFrameworkCore;

namespace CakeExchange.Data
{
    public class ExchangeContext : DbContextWithTriggers
    {
        public DbSet<Buy> BuyOrders { get; set; }
        public DbSet<Sell> SellOrders { get; set; }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=exchange_db;Trusted_Connection=True;");
        }
    }
}