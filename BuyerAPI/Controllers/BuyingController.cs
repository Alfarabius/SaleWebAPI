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

        [HttpPost("[action]inSalePointProduct")]
        public IActionResult Buy(
            [FromQuery] int BuyerId,
            [FromQuery, Required] int salesPointId,
            [FromBody, Required] Dictionary<string, int> products)
        {
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

                var salesPointString = getSalesPointResponse.ResponseData;

                if (getSalesPointResponse.StatusCode != HttpStatusCode.OK)
                    return NotFound(salesPointString);

                var salesPoint = JsonConvert.DeserializeObject<SalesPoint>(salesPointString);

                Dictionary<int, decimal> totalPrices = new Dictionary<int, decimal>(); // Dict for SalesData list
                
                // Check required products in this SalesPoint
                
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

                    // Create total prices for SalesData list

                    totalPrices.Add(productObj.Id, productObj.Price * product.Value);

                    // Change product quanity in SalesPoint Model

                    if (product.Value <= providedProduct.ProductQuantity)
                        providedProduct.ProductQuantity -= product.Value;
                    else
                        return BadRequest($"Not enought {product.Key} in the {salesPoint.Name}");
                }

                var json = JsonConvert.SerializeObject(salesPoint);
                
                // Update SalesPoint Entity by SalesPoint model

                var putSalesPointResponse = httpUtils.PutRequest(
                    httpClient,
                    Config.saleApiURL + Config.salesPoints + Config.updateBy + salesPointId.ToString(),
                    salesPoint
                    ).Result;

                if (putSalesPointResponse.StatusCode != HttpStatusCode.OK)
                    return BadRequest($"Failure update {salesPoint.Name}");

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

                // Create Sale Entity

                var saleCreationResponse = httpUtils.PostRequest(
                    httpClient,
                    Config.saleApiURL + Config.sales + Config.create,
                    Sale
                    ).Result;

                if (saleCreationResponse.StatusCode != HttpStatusCode.OK)
                    return BadRequest("Sale instanse did not created" + saleCreationResponse.ResponseData);

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

                if (getBuyerResponse.StatusCode != HttpStatusCode.OK)
                    return BadRequest(getBuyerResponse.ResponseData);

                var buyer = JsonConvert.DeserializeObject<Buyer>(getBuyerResponse.ResponseData);
                var sale = JsonConvert.DeserializeObject<Sale>(saleCreationResponse.ResponseData);

                // Create SalesId to Buyer model

                buyer.SalesIds.Add(sale.Id);

                // Update Buyer entity

                var updateBuyerResponse = httpUtils.PutRequest(
                    httpClient,
                    Config.saleApiURL + Config.buyers + Config.updateBy + BuyerId.ToString(),
                    buyer
                    ).Result;

                if (updateBuyerResponse.StatusCode != HttpStatusCode.OK)
                    return BadRequest(updateBuyerResponse.ResponseData);

            return Ok("Succsess");
            }                   
        }        
    }
}
