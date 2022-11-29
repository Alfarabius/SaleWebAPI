using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaleAPI.DataAccess;
using SaleAPI.Models;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SaleAPI.Controllers
{
    [Route("SaleAPI/[controller]")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly SaleAPIDataContext context;

        public ProductsController(SaleAPIDataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        [Route("GetBy/{id}")]
        public IActionResult GetProductById(int id) 
        {

            var product = this.ProductById(id);

            if (product == null) 
            {
                return NotFound($"Product {id} not found");
            }

            return Ok(product);
        }

        [HttpGet]
        [Route("List")]
        public IActionResult GetProducts([FromQuery] int num)
        {
            var products = this.context.Products.Take(num);
            return Ok(products);
        }

        [HttpPost]
        [Route("Create")]
        public async Task<Product> AddProduct([FromBody] Product newProduct) 
        {
            this.context.Products.Add(newProduct);
            await this.context.SaveChangesAsync();
            return newProduct;
        }

        [Authorize]
        [HttpDelete]
        [Route("DeleteBy/{id}")]
        public async Task<IActionResult> DeleteProductById(int id)
        {
            var product = this.ProductById(id);
            
            if (product == null)
            {
                return BadRequest($"Product {id} doesn't exist");            
            }

            this.context.Products.Remove(product);
            await this.context.SaveChangesAsync();
            return Ok(product);

        }

        [HttpPut]
        [Route("UpdateBy/{id}")]
        public async Task<IActionResult> UpdateProductById(int id, [FromBody] Product product)        
        {
            var oldProduct = this.ProductById(id);

            if (oldProduct == null)
            {
                return BadRequest($"Product {id} doesn't exist");
            }

            this.context.Products.Update(product);
            await this.context.SaveChangesAsync();
            return Ok(product);
        }

        private Product ProductById(int id) => this.context.Products.FirstOrDefault(p => p.Id == id);

    }
}
