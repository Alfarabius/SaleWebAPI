using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleAPI.DataAccess;
using SaleAPI.Models;
using System.Linq;
using System.Threading.Tasks;

namespace SaleAPI.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public abstract class BaseApiController<TEntity> : Controller where TEntity : class
    {
        protected abstract DbSet<TEntity> DbTable { get; }

        protected abstract IQueryable<TEntity> QueryTable { get; }

        protected abstract string Name { get; }

        private readonly SaleAPIDataContext context;

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
        [Route("List")]
        public IActionResult GetDbTable([FromQuery] int number)
        {
            var list = this.QueryTable.Take(number);
            return Ok(list);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<TEntity> AddEntity([FromBody] TEntity newEntity)
        {
            this.DbTable.Add(newEntity);
            await this.context.SaveChangesAsync();
            return newEntity;
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteBy/{id}")]
        public async Task<IActionResult> DeleteEntityById(int id)
        {
            var Entity = this.EntityById(id);

            if (Entity == null)
            {
                return BadRequest($"Entity {id} doesn't exist");
            }

            DbTable.Remove(Entity);
            await this.context.SaveChangesAsync();
            return Ok(Entity);

        }

        [HttpPut]
        [Route("UpdateBy/{id}")]
        public async Task<IActionResult> UpdateEntityById(int id, [FromBody] TEntity Entity)
        {
            var oldEntity = this.EntityById(id);

            if (oldEntity == null)
            {
                return BadRequest($"Entity {id} doesn't exist");
            }

            DbTable.Update(Entity);
            await this.context.SaveChangesAsync();
            return Ok(Entity);
        }

        public abstract TEntity EntityById(int id);
    }
}
