using Microsoft.EntityFrameworkCore;
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
        public string Date { get; set; }

        [Required]
        public string Time { get; set; }

        [Required]
        public int SalesPointId { get; set; }

        [Required, AllowNull]
        public int? BuyerId { get; set; }

        [Required]
        public ICollection<SalesData> SalesData { get; set; }

        [Required]
        [Column(TypeName = "decimal(20,2)")]
        public decimal TotalAmount { get; set; }
    }

    /// <summary>
    /// Класс <c>SalesData</c> – Данные о количестве и общей цене купленных товаров одного вида.
    /// </summary>
    public class SalesData
    {
        public int ProductId { get; set; }

        public int ProductQuantity { get; set; }

        public decimal ProductIdAmount { get; set; }
    }
}
