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
        private readonly ExchangeContext _dbContext;

        public Transaction()
        {
            Date = DateTime.UtcNow;
            State = TransactionStates.NotConfirmedByAdmin;
        }

        public Transaction(ExchangeContext dbContext)
        {
            _dbContext = dbContext;
            Date = DateTime.UtcNow;
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

        public void Try()
        {
            Buy = Buy.QueryFree(_dbContext)
                .FirstOrDefault() as Buy;

            Sell = Sell.QueryFree(_dbContext)
                .FirstOrDefault() as Sell;

            if (Buy == null || Sell == null)
                return;

            if (IsAvailable())
                Make(_dbContext);
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

            new Transaction(_dbContext).Try();
        }
    }

    public enum TransactionStates
    {
        NotConfirmedByAdmin,
        ConfirmedInProgress,
        HasDone
    }
}