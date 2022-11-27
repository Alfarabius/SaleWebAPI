using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleAPI.Models
{
    /// <summary>
    /// Класс <c>Buyer</c> – покупатель, лицо, осуществляющее покупку товара или услуги в одной из точек продаж.
    /// </summary>
    public class Buyer
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [NotMapped]
        public IEnumerable<int> SalesIds { get; set; }
    }
}
