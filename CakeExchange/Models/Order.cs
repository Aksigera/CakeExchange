using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CakeExchange.Attributes;

namespace CakeExchange.Models
{
    public abstract class Order
    {
        public int Id { get; set; }

        [Required]
        [Positive(ErrorMessage = "Цена должна быть не ниже 0")]
        [Column(TypeName = "decimal(10,2)")]
        [Decimal]
        [Display(Name = "Цена")]
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

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public DateTime Date { get; set; }

        public bool IsActive { get; set; }

        protected Order()
        {
            Date = DateTime.UtcNow;
            IsActive = true;
        }
    }
}