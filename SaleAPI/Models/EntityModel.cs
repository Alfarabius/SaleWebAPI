using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SaleAPI.Models
{
    /// <summary>
    /// Класс <c>EntityModel</c> – модель сущности БД.
    /// </summary>
    abstract public class EntityModel
    {
        public int Id { get; set; }
    }

    /// <summary>
    /// Класс <c>EntityModel</c> – именованная модель сущности БД.
    /// </summary>
    abstract public class NamedEntityModel : EntityModel 
    {
        [Required, NotNull]
        public string Name { get; set; }
    }
}
