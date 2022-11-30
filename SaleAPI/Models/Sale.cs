using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SaleAPI.Models
{
    /// <summary>
    /// Класс <c>Sale</c> – Акт продажи.
    /// </summary>
    public class Sale : EntityModel
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Time { get; set; }

        [Required]
        public int SalesPointId { get; set; }

        [Required, AllowNull]
        public int? BuyerId { get; set; }

        [Required, NotMapped]
        public IEnumerable<SalesData> SalesData { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }
    }
}
