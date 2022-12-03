using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleAPI.DataAccess;
using SaleAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SaleAPI.Controllers
{
    public class ProductsController : BaseApiController<Product>
    {
        protected override DbSet<Product> DbTable => base.context.Products;

        protected override string Name => "Product";

        public ProductsController(SaleAPIDataContext context) : base(context) { }

        public override Product EntityById(int id)
        {
            return DbTable.FirstOrDefault(p => p.Id == id);
        }

        [HttpPut]
        [Route("UpdateBy{id}")]
        public override async Task<IActionResult> UpdateEntityById(int id, [FromBody] Product Entity)
        {
            var oldProduct = this.EntityById(id);

            if (oldProduct == null)
                return BadRequest($"{Name} {id} doesn't exist");

            oldProduct.Name = Entity.Name;
            oldProduct.Price = Entity.Price;

            await this.context.SaveChangesAsync();
            return Ok(oldProduct);
        }
    }
}
