using System.Net;

namespace BuyerAPI.Models
{    public class ResponseViewModel
    {
        public string ResponseData { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
