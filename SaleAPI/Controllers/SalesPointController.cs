using Microsoft.EntityFrameworkCore;
using SaleAPI.DataAccess;
using SaleAPI.Models;
using System.Linq;

namespace SaleAPI.Controllers
{
    public class SalesPointController : BaseApiController<SalesPoint>
    {
        protected override DbSet<SalesPoint> DbTable => base.context.SalesPoints;

        protected override string Name => "SalesPoint";

        public SalesPointController(SaleAPIDataContext context) : base(context) { }

        public override SalesPoint EntityById(int id)
        {
            return DbTable.FirstOrDefault(s => s.Id == id);
        }
    }
}
