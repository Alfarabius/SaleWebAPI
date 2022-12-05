using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleAPI.DataAccess;
using SaleAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SaleAPI.Controllers
{
    public class SalesController : BaseApiController<Sale>
    {
        protected override DbSet<Sale> DbTable => base.context.Sales;

        protected override string Name => "Sale";

        public SalesController(SaleAPIDataContext context) : base(context) { }

        [HttpPut]
        [Route("UpdateBy/{id}")]
        public override async Task<IActionResult> UpdateEntityById(int id, [FromBody] Sale Entity)
        {
            var oldSale = this.EntityById(id);

            if (oldSale == null)
                return NotFound($"{Name} {id} doesn't exist");
            
            oldSale.SalesData = Entity.SalesData;
            oldSale.Date = Entity.Date;
            oldSale.Time = Entity.Time;
            oldSale.BuyerId = Entity.BuyerId;
            oldSale.SalesPointId = Entity.SalesPointId;
            oldSale.TotalAmount = Entity.TotalAmount;

            await this.context.SaveChangesAsync();
            return Ok(oldSale);
        }

        public override Sale EntityById(int id)
        {
            return DbTable.FirstOrDefault(s => s.Id == id);
        }
    }
}

