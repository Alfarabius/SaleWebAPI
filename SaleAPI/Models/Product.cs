using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace SaleAPI.Models
{
    /// <summary>
    /// Класс <c>Product</c> – модель товара.
    /// </summary>
    public class Product : NamedEntityModel
    {
        [Required]
        [Column(TypeName = "decimal(20,2)")]
        public decimal Price { get; set; }
    }
}
