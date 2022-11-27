using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Required]
        public List<ProvidedProducts> ProvidedProducts { get; set; }
    }
}
