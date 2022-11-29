using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

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
