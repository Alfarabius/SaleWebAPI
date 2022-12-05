using BuyerAPI.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System;
using System.Net.Http.Json;

namespace BuyerAPI.HttpUtils
{
    public class HttpUtils
    {
        public async Task<ResponseViewModel> HttpRequest(
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

        public async Task<ResponseViewModel> PutRequest(HttpClient httpClient, string uri, object body)
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

        public async Task<ResponseViewModel> PostRequest(HttpClient httpClient, string uri, object body)
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
}
