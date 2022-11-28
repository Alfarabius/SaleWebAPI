using System.ComponentModel.DataAnnotations;

namespace SaleAPI.Models
{
    /// <summary>
    /// Класс <c>Product</c> – модель товара.
    /// </summary>
    public class Product : NamedEntityModel
    {
        [Required]
        public decimal Price { get; set; }
    }
}
