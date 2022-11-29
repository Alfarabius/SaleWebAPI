using Microsoft.EntityFrameworkCore;
using SaleAPI.DataAccess;
using SaleAPI.Models;
using System.Linq;

namespace SaleAPI.Controllers
{
    public class SalesController : BaseApiController<Sale>
    {
        protected override DbSet<Sale> DbTable => base.context.Sales;

        protected override string Name => "Sale";

        public SalesController(SaleAPIDataContext context) : base(context) { }

        public override Sale EntityById(int id)
        {
            return DbTable.FirstOrDefault(s => s.Id == id);
        }
    }
}

