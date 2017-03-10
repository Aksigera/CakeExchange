using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using CakeExchange.Data;

namespace CakeExchange.Models
{
    public sealed class Transaction
    {

        public Transaction()
        {
            State = TransactionStates.NotConfirmedByAdmin;
        }

        public int Id { get; set; }

        [Required]
        public Buy Buy { get; set; }

        [Required]
        public Sell Sell { get; set; }

        public int Size { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

        public DateTime Date { get; set; }

        public TransactionStates State { get; set; }

        public Transaction(Buy buy, Sell sell)
        {
            Buy = buy;
            Sell = sell;
            Date = DateTime.UtcNow;
        }

        public static void Try()
        {
            using (var dbContext = new ExchangeContext())
            {
                Buy buyHighest = Buy.QueryFree(dbContext)
                    .FirstOrDefault() as Buy;

                Sell sellLowest = Sell.QueryFree(dbContext)
                    .FirstOrDefault() as Sell;

                if (buyHighest == null || sellLowest == null)
                    return;

                Transaction transaction = new Transaction(buyHighest, sellLowest);

                if (transaction.IsAvailable())
                    transaction.Make(dbContext);
            }
        }

        private bool IsAvailable()
        {
            return Buy.Price >= Sell.Price;
        }

        private void Make(ExchangeContext dbContext)
        {
            var deal = new List<Order> {Buy, Sell};

            Price = deal.OrderBy(o => o.Date).First().Price;
            Size = deal.Select(o => o.Number).Min();

            foreach (Order order in deal)
            {
                order.Number -= Size;
            }

            dbContext.Add(this);
            dbContext.SaveChanges();

            Try();
        }
    }

    public enum TransactionStates
    {
        NotConfirmedByAdmin,
        ConfirmedInProgress,
        HasDone
    }
}