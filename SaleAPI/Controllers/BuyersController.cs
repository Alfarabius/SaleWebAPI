using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleAPI.DataAccess;
using SaleAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SaleAPI.Controllers
{
    [Authorize]
    public class BuyersController : BaseApiController<Buyer>
    {
        protected override DbSet<Buyer> DbTable => base.context.Buyers;

        protected override string Name => "Buyer";

        public BuyersController(SaleAPIDataContext context) : base(context) { }

        public override Buyer EntityById(int id)
        {
            return DbTable.FirstOrDefault(b => b.Id == id);
        }

        [HttpPut]
        [Route("UpdateBy/{id}")]
        public override async Task<IActionResult> UpdateEntityById(int id, [FromBody] Buyer Entity)
        {
            var oldBuyer = this.EntityById(id);

            if (oldBuyer == null)
                return NotFound($"{Name} {id} doesn't exist");
            
            oldBuyer.Name = Entity.Name;
            oldBuyer.SalesIds = Entity.SalesIds;            

            await this.context.SaveChangesAsync();
            return Ok(oldBuyer);
        }
    }
}
