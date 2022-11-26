using System.ComponentModel.DataAnnotations;

namespace SaleAPI.Models
{
    /// <summary>
    /// Класс <c>SalesData</c> – Данные о количестве и общей цене купленных товаров одного вида.
    /// </summary>
    public class SalesData
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public decimal ProductQuantity { get; set; }

        [Required]
        public decimal ProductIdAmount { get; set; }
    }
}
