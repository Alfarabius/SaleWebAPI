using Microsoft.EntityFrameworkCore;
using SaleAPI.DataAccess;
using SaleAPI.Models;
using System.Linq;

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
    }
}
