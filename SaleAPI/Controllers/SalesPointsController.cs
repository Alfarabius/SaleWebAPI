using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleAPI.DataAccess;
using SaleAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SaleAPI.Controllers
{
    public class SalesPointsController : BaseApiController<SalesPoint>
    {
        protected override DbSet<SalesPoint> DbTable => base.context.SalesPoints;

        protected override string Name => "SalesPoint";

        public SalesPointsController(SaleAPIDataContext context) : base(context) { }

        [HttpPut]
        [Route("UpdateBy/{id}")]
        public override async Task<IActionResult> UpdateEntityById(int id, [FromBody] SalesPoint Entity)
        {
            var oldSalesPoint = this.EntityById(id);

            if (oldSalesPoint == null)
                return BadRequest($"{Name} {id} doesn't exist");

            oldSalesPoint.Name = Entity.Name;
            oldSalesPoint.ProvidedProducts = Entity.ProvidedProducts;
            
            await this.context.SaveChangesAsync();
            return Ok(oldSalesPoint);
        }

        public override SalesPoint EntityById(int id)
        {
            return DbTable.FirstOrDefault(s => s.Id == id);
        }
    }
}
