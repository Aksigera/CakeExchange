﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using CakeExchange.Data;
using Microsoft.EntityFrameworkCore;

namespace CakeExchange.Models
{
    public sealed class Transaction
    {
        public int Id { get; set; }

        [Required]
        public Buy Buy { get; set; }

        [Required]
        public Sell Sell { get; set; }

        public int Size { get; set; }

        public DateTime Date { get; set; }

        public Transaction(Buy buy, Sell sell)
        {
            Buy = buy;
            Sell = sell;
            Date = DateTime.UtcNow;
        }

        public static void Try()
        {
            using (ExchangeContext dbContext = new ExchangeContext())
            {
                Buy buyHighest = dbContext.BuyOrders.OrderBy(o => o.Price).FirstOrDefault();
                Sell sellLowest = dbContext.SellOrders.OrderBy(o => o.Price).FirstOrDefault();

                if (buyHighest == null || sellLowest == null)
                    return;

                Transaction transaction = new Transaction(buyHighest, sellLowest);

                if (transaction.IsAvailable())
                    transaction.Make(dbContext);
            }
        }

        private bool IsAvailable()
        {
            return Buy.Price <= Sell.Price;
        }

        private void Make(DbContext dbContext)
        {
            var deal = new List<Order> {Buy, Sell};

            Size = deal.Select(o => o.Number).Min();
            foreach (Order order in deal)
            {
                order.Number -= Size;
            }

            dbContext.Add(this);
            dbContext.SaveChanges();

            Transaction.Try();
        }
    }
}