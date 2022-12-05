using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleAPI.DataAccess;
using SaleAPI.Models;
using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SaleAPI.Controllers
{
    [Authorize]
    [Route("SaleAPI/[controller]")]
    [ApiController]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public IActionResult GetDbTable([FromQuery] int number)
        {
            var list = this.DbTable.Take(number);
            return Ok(list);
        }

        [HttpPost]
        [Route("Create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddEntity([FromBody] TEntity newEntity)
        {
            try
            {
                this.DbTable.Add(newEntity);
                await this.context.SaveChangesAsync();
                return Ok(newEntity);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
        }

        [HttpDelete]
        [Route("DeleteBy/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> DeleteEntityById(int id)
        {
            var Entity = this.EntityById(id);

            if (Entity == null)
            {
                return NotFound($"{Name} {id} doesn't exist");
            }

            DbTable.Remove(Entity);
            await this.context.SaveChangesAsync();
            return Ok("Succsess");

        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public abstract Task<IActionResult> UpdateEntityById(int id, TEntity Entity);

        [ApiExplorerSettings(IgnoreApi = true)]
        public abstract TEntity EntityById(int id);
    }
}
