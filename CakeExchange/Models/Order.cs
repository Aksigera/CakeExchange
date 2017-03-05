﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CakeExchange.Attributes;
using EntityFrameworkCore.Triggers;

namespace CakeExchange.Models
{
    public abstract class Order
    {
        public int Id { get; set; }

        [Required]
        [Positive(ErrorMessage = "Цена должна быть не ниже 0")]
        [Column(TypeName = "decimal(10,2)")]
        [Decimal]
        [Display(Name="Цена")]
        public decimal Price { get; set; }

        private int _number;

        [Positive(ErrorMessage = "Количество должно быть не ниже 0")]
        [Required]
        public int Number
        {
            get { return _number; }
            set
            {
                if (value == 0) IsActive = false;
                _number = value;
            }
        }

        public string Comment { get; set; }

        public DateTime Date { get; set; }

        public bool IsActive { get; set; }

        protected Order()
        {
            Date = DateTime.UtcNow;
            IsActive = true;

            Triggers<Order>.Inserted += entry => Transaction.Try();
        }
    }
}