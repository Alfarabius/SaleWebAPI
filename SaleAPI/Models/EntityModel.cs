using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace SaleAPI.Models
{
    abstract public class EntityModel
    {
        public int Id { get; internal set; }

        public void SetId(int id) 
        {
            this.Id = id;            
        }
    }

    abstract public class NamedEntityModel : EntityModel 
    {
        [Required, NotNull]
        public string Name { get; }
    }
}
