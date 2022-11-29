using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SaleAPI.DataAccess;
using SaleAPI.Models;
using System.Linq;

namespace SaleAPI.Controllers
{
    [Route("SaleAPI/[controller]")]
    [ApiController]
    public class BuyerController : BaseApiController<Buyer>
    {
        protected override DbSet<Buyer> DbTable => base.context.Buyers;

        protected override string Name => "Buyer";

        public BuyerController(SaleAPIDataContext context) : base(context) { }

        public override Buyer EntityById(int id)
        {
            return DbTable.FirstOrDefault(b => b.Id == id);
        }
    }
}
