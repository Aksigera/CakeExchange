using CakeExchange.Models;
using Microsoft.EntityFrameworkCore;

namespace CakeExchange.Data
{
    public class ExchangeContext : DbContext
    {
        public DbSet<Buy> BuyOrders { get; set; }
        public DbSet<Sell> SellOrders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=exchange_db;Trusted_Connection=True;");
        }
    }
}