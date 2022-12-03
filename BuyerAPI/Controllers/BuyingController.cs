using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace BuyerAPI.Controllers
{
    [Route("BuyAPI/[controller]")]
    [ApiController]
    public class BuyingController : Controller
    {
        public const string saleApiURL = "https://localhost:5001/SaleAPI";
        
        public const string buyers = "/Buyers";
        public const string products = "/Products";
        public const string salesPoints = "/SalesPoints";

        public const string getBy = "/GetBy/";
        public const string getList = "/GetList";
        public const string create = "/Create";
        public const string deleteBy = "/DeleteBy/";
        public const string updateBy = "/UpdateBy/";


        [HttpGet("[action]inSalePointProduct")]
        public IActionResult Buy([FromQuery] int salesPoitId, [FromQuery] int productId) 
        {            
            return Ok();           
        }

        static async Task<HttpResponseMessage> HttpRequest(
            string uri, 
            Dictionary<string, string> Parameters, 
            List<string> Header,
            HttpMethod Method) 
        {
            using (var httpClient = new HttpClient()) 
            {
                Uri _uri = new Uri(uri);
                FormUrlEncodedContent content = new FormUrlEncodedContent(Parameters);
                httpClient.DefaultRequestHeaders.Add(Header[0], Header[1]);

                using (var requestMessage = new HttpRequestMessage(Method, _uri))
                {
                    requestMessage.Content = content;
                    var response = await httpClient.SendAsync(requestMessage);
                    return response;
                }
            }
        }
    }
}
