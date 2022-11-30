﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaleAPI.Models
{
    /// <summary>
    /// Класс <c>Buyer</c> – покупатель, лицо, осуществляющее покупку товара или услуги в одной из точек продаж.
    /// </summary>
    public class Buyer : NamedEntityModel
    {
        public ICollection<SaleIdE> SalesIds { get; set; }
    }

    public class SaleIdE : EntityModel
    {
        [ForeignKey(nameof(Sale))]
        public int SaleId { get; set; }
    };
}
