using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SaleAPI.Models
{
    /// <summary>
    /// Класс <c>Sale</c> – Акт продажи.
    /// </summary>
    public class Sale
    {
        public int Id { get; set; }

        [Required]
        public string Date { get; set; }

        [Required]
        public string Time { get; set; }

        [Required]
        public int SalesPointId { get; set; }

        [Required]
        public int BuyerId { get; set; }

        [Required]
        public List<SalesData> SalesData { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }
    }
}
