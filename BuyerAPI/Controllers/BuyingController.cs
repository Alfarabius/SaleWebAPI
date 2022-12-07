using BuyerAPI.Models;
using BuyerAPI.HttpUtils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace BuyerAPI.Controllers
{
    [Route("BuyAPI/[controller]")]
    [ApiController]
    public class BuyingController : Controller
    {
        private readonly HttpUtils.HttpUtils httpUtils;

        public BuyingController(HttpUtils.HttpUtils httpUtils)
        {
            this.httpUtils = httpUtils;
        }

        [HttpPost("[action]")]
        public IActionResult Sale(
            [FromQuery] int BuyerId,
            [FromQuery, Required] int salesPointId,
            [FromBody, Required] Dictionary<string, int> products)
        {
            if (products.Values.Any(p => p < 0) || products.Values.All(p => p == 0))
                return BadRequest("product quntity problem");

            using (var httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization = 
                    new AuthenticationHeaderValue(Config.type, Config.key);

                // Get SalesPoint
                
                var getSalesPointResponse = httpUtils.HttpRequest(
                httpClient,
                Config.saleApiURL + Config.salesPoints + Config.getBy + salesPointId.ToString(),
                new Dictionary<string, string>() { },
                HttpMethod.Get).Result;

                if (getSalesPointResponse.StatusCode != HttpStatusCode.OK)
                    return NotFound(getSalesPointResponse.ResponseData);

                // Deserialize to SalesPoint Model

                var salesPoint = JsonConvert.DeserializeObject<SalesPoint>(getSalesPointResponse.ResponseData);
                
                // Check required products in this SalesPoint

                Dictionary<int, decimal> totalPrices = new Dictionary<int, decimal>(); // Dict for SalesData list

                foreach (var product in products)
                {
                    var providedProduct = salesPoint.ProvidedProducts
                        .FirstOrDefault(p => p.ProductId.ToString() == product.Key);
                    
                    if (providedProduct == null)
                        return NotFound($"Product - {product.Key} did not found in the {salesPoint.Name}");

                    // Get current product price

                    var getProductResponse = httpUtils.HttpRequest(
                        httpClient,
                        Config.saleApiURL + Config.products + Config.getBy + providedProduct.ProductId,
                        new Dictionary<string, string>(),
                        HttpMethod.Get
                        ).Result;

                    if (getProductResponse.StatusCode != HttpStatusCode.OK)
                        return NotFound("Error: Unregistred product!");

                    var productObj = JsonConvert.DeserializeObject<Product>(getProductResponse.ResponseData);

                    // Add total in totalPrices dictionary <productId, total> for SalesData list

                    totalPrices.Add(productObj.Id, productObj.Price * product.Value);

                    // Check quantity of products in SalesPoint and change product quantity in SalesPoint Model

                    if (product.Value <= providedProduct.ProductQuantity)
                        providedProduct.ProductQuantity -= product.Value;
                    else
                        return BadRequest($"Not enought {product.Key} in the {salesPoint.Name}");
                }

                // Fill SalesData list
                
                List<SalesData> salesDataLst = new List<SalesData>();

                foreach (var product in products)
                {
                    if (product.Value == 0)
                        continue;

                    salesDataLst.Add(new SalesData()
                    {
                        ProductId = Int32.Parse(product.Key),
                        ProductQuantity = product.Value,
                        ProductIdAmount = totalPrices[Int32.Parse(product.Key)]
                    });
                }

                // Create Sale model

                var Sale = new Sale()
                {
                    Id = 0,
                    Date = DateTime.Now.ToShortDateString(),
                    Time = DateTime.Now.ToString("HH:mm"),
                    SalesPointId = salesPointId,
                    BuyerId = BuyerId,
                    SalesData = salesDataLst,
                    TotalAmount = totalPrices.Values.Sum()
                };

                // Create Sale Entity !!!!(1)!!!!

                var saleCreationResponse = httpUtils.PostRequest(
                    httpClient,
                    Config.saleApiURL + Config.sales + Config.create,
                    Sale
                    ).Result;

                if (saleCreationResponse.StatusCode != HttpStatusCode.OK)
                    return BadRequest("Sale instanse did not created" + saleCreationResponse.ResponseData);

                var saleFromDb = JsonConvert.DeserializeObject<Sale>(saleCreationResponse.ResponseData);

                // Update SalesPoint Entity by SalesPoint model !!!!(2)!!!!

                var json = JsonConvert.SerializeObject(salesPoint);

                var putSalesPointResponse = httpUtils.PutRequest(
                    httpClient,
                    Config.saleApiURL + Config.salesPoints + Config.updateBy + salesPointId.ToString(),
                    salesPoint
                    ).Result;

                if (putSalesPointResponse.StatusCode != HttpStatusCode.OK)
                {
                    Rollback(1, saleFromDb.Id, salesPoint, httpClient);
                    return BadRequest($"Failure update {salesPoint.Name}");
                }                    

                // Check Buyer are authorized

                if (BuyerId == 0)
                    return Ok("Succsess");

                // Get Buyer

                var getBuyerResponse = httpUtils.HttpRequest(
                    httpClient,
                    Config.saleApiURL + Config.buyers + Config.getBy + BuyerId.ToString(),
                    new Dictionary<string, string>(),
                    HttpMethod.Get
                    ).Result;

                var oldSalesPoint = JsonConvert.DeserializeObject<SalesPoint>(getSalesPointResponse.ResponseData);

                if (getBuyerResponse.StatusCode != HttpStatusCode.OK)
                {                    
                    Rollback(2, saleFromDb.Id, oldSalesPoint, httpClient);
                    return BadRequest(getBuyerResponse.ResponseData);
                }

                var buyer = JsonConvert.DeserializeObject<Buyer>(getBuyerResponse.ResponseData);                

                // Add SalesId to Buyer model

                buyer.SalesIds.Add(saleFromDb.Id);

                // Update Buyer entity !!!!(3)!!!!

                var updateBuyerResponse = httpUtils.PutRequest(
                    httpClient,
                    Config.saleApiURL + Config.buyers + Config.updateBy + BuyerId.ToString(),
                    buyer
                    ).Result;

                if (updateBuyerResponse.StatusCode != HttpStatusCode.OK)
                {
                    Rollback(2, saleFromDb.Id, oldSalesPoint, httpClient);
                    return BadRequest(updateBuyerResponse.ResponseData);
                }

            return Ok("Succsess");
            }                   
        }
        
        private async void Rollback(int step, int saleId, SalesPoint salesPoint, HttpClient httpClient)
        {
            await httpUtils.HttpRequest(
                httpClient,
                Config.saleApiURL + Config.sales + Config.deleteBy + saleId.ToString(),
                new Dictionary<string, string>(),
                HttpMethod.Delete
                ); 
            
            if (step == 2) 
            {
                await httpUtils.PutRequest(
                    httpClient,
                    Config.saleApiURL + Config.salesPoints + Config.updateBy + salesPoint.Id.ToString(),
                    salesPoint
                    );
            }

        }
    }
}
