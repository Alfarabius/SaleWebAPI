using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleAPI.Models
{
    public class ProvidedProduct : EntityModel
    {
        [Required, ForeignKey(nameof(Product))]
        public int ProductId { get; set; }
        
        [Required]
        public int ProductQuantity { get; set; }
    }
}
