using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Storage;
using SaleAPI.DataAccess;
using SaleAPI.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SaleAPI.Controllers
{
    [Route("SaleAPI/[controller]")]
    [ApiController]
    [Authorize]
    public class SeedDbController : Controller
    {
        private readonly SaleAPIDataContext context;

        public SeedDbController(SaleAPIDataContext context)
        {
            this.context = context;
        }
        
        [HttpGet("[action]")]
        public async Task<IActionResult> RecreateDataBase()
        {
            await this.CleanDataBase();
            this.SeedMockDb(context);
            await this.context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> CleanDataBase() 
        {
            this.context.Database.EnsureDeleted();
            this.context.Database.EnsureCreated();
            await this.context.SaveChangesAsync();

            return Ok();
        }

        protected void SeedMockDb(SaleAPIDataContext context)
        {
            context.Buyers.AddRange(
                 new Buyer() { Name = "Ivan Fedorov", SalesIds = new List<int>() },
                 new Buyer() { Name = "David Mahov", SalesIds = new List<int>() },
                 new Buyer() { Name = "Maria Lungina", SalesIds = new List<int>() },
                 new Buyer() { Name = "Lily Maksacova", SalesIds = new List<int>() },
                 new Buyer() { Name = "Rulon Oboev", SalesIds = new List<int>() },
                 new Buyer() { Name = "Armen Pashinyan", SalesIds = new List<int>() },
                 new Buyer() { Name = "Olga Smetanina", SalesIds = new List<int>() },
                 new Buyer() { Name = "Farida Aleshkova", SalesIds = new List<int>() }
                 );

            context.Products.AddRange(
                new Product() { Name = "GK61-Keyboard", Price = 45.99m },
                new Product() { Name = "HHKb-Keyboard", Price = 78.99m },
                new Product() { Name = "jj41-Keyboard", Price = 25.45m },
                new Product() { Name = "GK68-Keyboard", Price = 50.00m },
                new Product() { Name = "BlackTKL", Price = 105.95m },
                new Product() { Name = "BM16-Keypad", Price = 15.99m },
                new Product() { Name = "GMKCherrySpace Keycaps", Price = 25.55m },
                new Product() { Name = "Metal Plate", Price = 25.33m },
                new Product() { Name = "logimech", Price = 9.99m },
                new Product() { Name = "SuperGameingKeyboard", Price = 5.99m }
                );

            var P1 = new ProvidedProduct() { ProductId = 1, ProductQuantity = 10 };
            var P2 = new ProvidedProduct() { ProductId = 2, ProductQuantity = 6 };
            var P3 = new ProvidedProduct() { ProductId = 3, ProductQuantity = 15 };
            var P4 = new ProvidedProduct() { ProductId = 4, ProductQuantity = 3 };
            var P5 = new ProvidedProduct() { ProductId = 5, ProductQuantity = 4 };
            var P6 = new ProvidedProduct() { ProductId = 6, ProductQuantity = 12 };
            var P7 = new ProvidedProduct() { ProductId = 7, ProductQuantity = 12 };
            var P8 = new ProvidedProduct() { ProductId = 8, ProductQuantity = 5 };
            var P9 = new ProvidedProduct() { ProductId = 9, ProductQuantity = 62 };
            var P10 = new ProvidedProduct() { ProductId = 10, ProductQuantity = 55 };

            context.SalesPoints.AddRange(
                new SalesPoint() { Name = "KeyboardState", ProvidedProducts = new List<ProvidedProduct>() { P1, P2, P3 } },
                new SalesPoint() { Name = "LocalKeyboards", ProvidedProducts = new List<ProvidedProduct>() { P2, P4, P5, P6 } },
                new SalesPoint() { Name = "KeyboardGeeks", ProvidedProducts = new List<ProvidedProduct>() { P2, P5, P7, P8 } },
                new SalesPoint() { Name = "Keyboards for everyone", ProvidedProducts = new List<ProvidedProduct>() { P9, P10 } }
                );

            context.SaveChanges();
        }
    }
}
