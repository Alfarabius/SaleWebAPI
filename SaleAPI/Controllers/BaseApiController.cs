using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleAPI.DataAccess;
using SaleAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SaleAPI.Controllers
{
    [Route("SaleAPI/[controller]")]
    [ApiController]
    [Authorize]
    public abstract class BaseApiController<TEntity> : Controller where TEntity : class
    {
        protected abstract DbSet<TEntity> DbTable { get; }

        protected abstract string Name { get; }

        protected SaleAPIDataContext context { get; }

        public BaseApiController(SaleAPIDataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("GetBy/{id}")]
        public IActionResult GetEntityById(int id)
        {

            var Entity = this.EntityById(id);

            if (Entity == null)
            {
                return NotFound($"{Name} with id - {id} not found");
            }

            return Ok(Entity);
        }

        [HttpGet]
        [Route("GetList")]
        public IActionResult GetDbTable([FromQuery] int number)
        {
            var list = this.DbTable.Take(number);
            return Ok(list);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> AddEntity([FromBody] TEntity newEntity)
        {
            this.DbTable.Add(newEntity);
            await this.context.SaveChangesAsync();
            return Ok(newEntity);
        }

        [HttpDelete]
        [Route("DeleteBy/{id}")]
        public async Task<IActionResult> DeleteEntityById(int id)
        {
            var Entity = this.EntityById(id);

            if (Entity == null)
            {
                return BadRequest($"{Name} {id} doesn't exist");
            }

            DbTable.Remove(Entity);
            await this.context.SaveChangesAsync();
            return Ok(Entity);

        }

        public abstract Task<IActionResult> UpdateEntityById(int id, TEntity Entity);

        [ApiExplorerSettings(IgnoreApi = true)]
        public abstract TEntity EntityById(int id);
    }
}
