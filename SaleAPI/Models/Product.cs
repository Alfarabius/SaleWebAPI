using System.ComponentModel.DataAnnotations;

namespace SaleAPI.Models
{
    /// <summary>
    /// Класс <c>Product</c> – модель товара.
    /// </summary>
    public class Product
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}
