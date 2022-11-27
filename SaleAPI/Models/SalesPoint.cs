using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleAPI.Models
{
    /// <summary>
    /// Класс <c>SalesPoint</c> – точка продажи товаров.
    /// </summary>
    public class SalesPoint
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required, NotMapped]
        public IEnumerable<ProvidedProducts> ProvidedProducts { get; set; }
    }
}
