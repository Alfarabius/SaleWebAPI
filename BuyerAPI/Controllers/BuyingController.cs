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

                var getSalesPointResponse = HttpRequest(
                httpClient,
                Config.saleApiURL + Config.salesPoints + Config.getBy + salesPointId.ToString(),
                new Dictionary<string, string>() { },
                HttpMethod.Get).Result;

                var salesPointString = getSalesPointResponse.ResponseData;

                if (getSalesPointResponse.StatusCode != HttpStatusCode.OK)
                    return NotFound(salesPointString);

                var salesPoint = JsonConvert.DeserializeObject<SalesPoint>(salesPointString);

                Dictionary<int, decimal> totalPrices = new Dictionary<int, decimal>();
                
                foreach (var product in products)
                {
                    var providedProduct = salesPoint.ProvidedProducts
                        .FirstOrDefault(p => p.ProductId.ToString() == product.Key);
                    
                    if (providedProduct == null)
                        return NotFound($"Product - {product.Key} did not found in the {salesPoint.Name}");

                    var getProductResponse = HttpRequest(
                        httpClient,
                        Config.saleApiURL + Config.products + Config.getBy + providedProduct.ProductId,
                        new Dictionary<string, string>(),
                        HttpMethod.Get
                        ).Result;

                    if (getProductResponse.StatusCode != HttpStatusCode.OK)
                        return NotFound("Error: Unregistred product!");

                    var productObj = JsonConvert.DeserializeObject<Product>(getProductResponse.ResponseData);

                    totalPrices.Add(productObj.Id, productObj.Price * product.Value);

                    if (product.Value <= providedProduct.ProductQuantity)
                        providedProduct.ProductQuantity -= product.Value;
                    else
                        return BadRequest($"Not enought {product.Key} in the {salesPoint.Name}");
                }

                var json = JsonConvert.SerializeObject(salesPoint);
                
                var putSalesPointResponse = PutRequest(
                    httpClient,
                    Config.saleApiURL + Config.salesPoints + Config.updateBy + salesPointId.ToString(),
                    salesPoint
                    ).Result;

                if (putSalesPointResponse.StatusCode != HttpStatusCode.OK)
                    return BadRequest($"Failure update {salesPoint.Name}");

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

                var saleCreationResponse = PostRequest(
                    httpClient,
                    Config.saleApiURL + Config.sales + Config.create,
                    Sale
                    ).Result;

                if (saleCreationResponse.StatusCode != HttpStatusCode.OK)
                    return BadRequest("Sale instanse did not created" + saleCreationResponse.ResponseData);

                if (BuyerId == 0)
                    return Ok("Succsess");

                var getBuyerResponse = HttpRequest(
                    httpClient,
                    Config.saleApiURL + Config.buyers + Config.getBy + BuyerId.ToString(),
                    new Dictionary<string, string>(),
                    HttpMethod.Get
                    ).Result;

                if (getBuyerResponse.StatusCode != HttpStatusCode.OK)
                    return BadRequest(getBuyerResponse.ResponseData);

                var buyer = JsonConvert.DeserializeObject<Buyer>(getBuyerResponse.ResponseData);
                var sale = JsonConvert.DeserializeObject<Sale>(saleCreationResponse.ResponseData);

                buyer.SalesIds.Add(sale.Id);

                var updateBuyerResponse = PutRequest(
                    httpClient,
                    Config.saleApiURL + Config.buyers + Config.updateBy + BuyerId.ToString(),
                    buyer
                    ).Result;

                if (updateBuyerResponse.StatusCode != HttpStatusCode.OK)
                    return BadRequest(updateBuyerResponse.ResponseData);

            return Ok("Succsess");
            }                   
        }

        private async Task<ResponseViewModel> HttpRequest(
            HttpClient httpClient,
            string uri, 
            Dictionary<string, string> Parameters, 
            HttpMethod Method) 
        {            
            try
            {
                Uri _uri = new Uri(uri);
                FormUrlEncodedContent content = new FormUrlEncodedContent(Parameters);                

                using (var requestMessage = new HttpRequestMessage(Method, _uri))
                {
                    requestMessage.Content = content;
                    

                    using (var response = httpClient.SendAsync(requestMessage).GetAwaiter().GetResult()) 
                    {
                        var status = response.StatusCode;
                        var responseString = await response.Content.ReadAsStringAsync();
                        
                        var goodResponse = new ResponseViewModel();
                        goodResponse.StatusCode = status;
                        goodResponse.ResponseData = responseString;
                        
                        return goodResponse;
                    }                    
                }
            }
            catch (Exception ex) 
            {
                var badResponse = new ResponseViewModel();
                badResponse.ResponseData = $"Error - {ex.Message}";
                badResponse.StatusCode = HttpStatusCode.InternalServerError;

                return badResponse; 
            }
        }

        private async Task<ResponseViewModel> PutRequest(HttpClient httpClient, string uri, object body)
        {
            try
            {
                Uri _uri = new Uri(uri);
                using (var response = httpClient.PutAsJsonAsync(_uri, body).GetAwaiter().GetResult())
                {
                    var status = response.StatusCode;
                    var responseString = await response.Content.ReadAsStringAsync();

                    var goodResponse = new ResponseViewModel();
                    goodResponse.StatusCode = status;
                    goodResponse.ResponseData = responseString;

                    return goodResponse;
                }

            }
            catch (Exception ex)
            {
                var badResponse = new ResponseViewModel();
                badResponse.ResponseData = $"Error - {ex.Message}";
                badResponse.StatusCode = HttpStatusCode.InternalServerError;

                return badResponse;
            }
        }

        private async Task<ResponseViewModel> PostRequest(HttpClient httpClient, string uri, object body)
        {
            try
            {
                Uri _uri = new Uri(uri);
                using (var response = httpClient.PostAsJsonAsync(_uri, body).GetAwaiter().GetResult())
                {
                    var status = response.StatusCode;
                    var responseString = await response.Content.ReadAsStringAsync();

                    var goodResponse = new ResponseViewModel();
                    goodResponse.StatusCode = status;
                    goodResponse.ResponseData = responseString;

                    return goodResponse;
                }
            }
            catch (Exception ex)
            {
                var badResponse = new ResponseViewModel();
                badResponse.ResponseData = $"Error - {ex.Message}";
                badResponse.StatusCode = HttpStatusCode.InternalServerError;

                return badResponse;
            }
        }
    }

    public class ResponseViewModel
    {
        public string ResponseData { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }

    public partial class Sale 
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("date")]
        public string Date { get; set; }

        [JsonProperty("time")]
        public string Time { get; set; }

        [JsonProperty("salesPointId")]
        public int SalesPointId { get; set; }

        [JsonProperty("buyerId")]
        public int BuyerId { get; set; }

        [JsonProperty("salesData")]
        public List<SalesData> SalesData { get; set; }

        [JsonProperty("totalAmount")]
        public decimal TotalAmount { get; set; }
    }

    public partial class SalesData
    {
        [JsonProperty("productId")]
        public int ProductId { get; set; }

        [JsonProperty("productQuantity")]
        public int ProductQuantity { get; set; }

        [JsonProperty("productIdAmount")]
        public decimal ProductIdAmount { get; set; }
    }

    public partial class SalesPoint
    {
        [JsonProperty("providedProducts")]
        public List<ProvidedProduct> ProvidedProducts { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }

    public partial class ProvidedProduct
    {
        [JsonProperty("productId")]
        public int ProductId { get; set; }

        [JsonProperty("productQuantity")]
        public int ProductQuantity { get; set; }
    }

    public partial class Product
    {
        [JsonProperty("price")]
        public decimal Price { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }

    public partial class Buyer
    {
        [JsonProperty("salesIds")]
        public List<int> SalesIds { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
