using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleAPI.DataAccess;
using SaleAPI.Models;
using System;
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

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public override async Task<IActionResult> AddEntity([FromBody] Sale newEntity) 
        {
            try
            {
                if (newEntity.BuyerId == 0)
                    newEntity.BuyerId = null;

                this.DbTable.Add(newEntity);
                await this.context.SaveChangesAsync();
                return Ok(newEntity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public override Sale EntityById(int id)
        {
            return DbTable.FirstOrDefault(s => s.Id == id);
        }
    }
}

