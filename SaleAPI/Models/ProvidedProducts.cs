using System.ComponentModel.DataAnnotations;

namespace SaleAPI.Models
{
    public class ProvidedProducts
    {
        [Required]
        public int ProductId { get; set; }
        
        [Required]
        public decimal ProductQuantity { get; set; }
    }
}
