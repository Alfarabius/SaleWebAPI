using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleAPI.Models
{
    /// <summary>
    /// Класс <c>SalesPoint</c> – точка продажи товаров.
    /// </summary>
    public class SalesPoint : NamedEntityModel
    {
        [Required]        
        public ICollection<ProvidedProduct> ProvidedProducts { get; set; }
    }

    public class ProvidedProduct
    {
        public int ProductId { get; set; }

        public int ProductQuantity { get; set; }
    }
}
