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
        [Required, NotMapped]
        public IEnumerable<ProvidedProducts> ProvidedProducts { get; set; }
    }
}
