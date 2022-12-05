using Newtonsoft.Json;
using System.Collections.Generic;

namespace BuyerAPI.Models
{
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
