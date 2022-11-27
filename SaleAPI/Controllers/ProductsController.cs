using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SaleAPI.DataAccess;
using SaleAPI.Models;
using System.Collections;
using System.Collections.Generic;

namespace SaleAPI.Controllers
{
    [Route("SaleAPI/Products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly SaleAPIDataContext context;

        public ProductsController(SaleAPIDataContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public IEnumerable<Product> GetAllProducts() 
        {
            return this.context.Products;
        }

        [HttpPost]
        public string Post() => "Hello, World!";
    }
}
