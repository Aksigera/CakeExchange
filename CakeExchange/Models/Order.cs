using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using CakeExchange.Attributes;

namespace CakeExchange.Models
{
    public abstract class Order
    {
        public int Id { get; set; }

        [Required]
        [Positive]
        [Column(TypeName = "decimal(10,2)")]
        public decimal Price { get; set; }

//        [Positive]
        [Required]
        public int Number { get; set; }

        public string Comment { get; set; }

        public DateTime Date { get; set; }

        protected Order()
        {
            Date = DateTime.UtcNow;
        }
    }
}