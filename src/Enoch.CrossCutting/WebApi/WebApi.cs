using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace Enoch.CrossCutting.WebApi
{
    public class WebApi : IWebApi
    {
        private static HttpClient _httpClient;

        public WebApi(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public HttpResponseMessage Get(string server, string route, string parameters = null,
            string token = null)
        {
            var _baseUrl = $"{server}/{route}";

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = _httpClient.GetAsync($"{_baseUrl}?{parameters}");

            return response.Result;
        }

        public HttpResponseMessage Post(string server, string route, string token, string url = null, object obj = null)
        {
            var baseUrl = $"{server}/{route}";

            var json = JsonConvert.SerializeObject(obj);

            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = _httpClient.PostAsync($"{baseUrl}/{url}", stringContent);

            return response.Result;
        }

        public HttpResponseMessage Post(string server, string route, string token, object obj = null)
        {
            var baseUrl = $"{server}/{route}";

            var json = JsonConvert.SerializeObject(obj);

            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = _httpClient.PostAsync($"{baseUrl}", stringContent);

            return response.Result;
        }

        public HttpResponseMessage Post(string server, string route, object obj = null)
        {
            var baseUrl = $"{server}/{route}";

            var json = JsonConvert.SerializeObject(obj);

            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            var response = _httpClient.PostAsync($"{baseUrl}", stringContent);

            return response.Result;
        }

        public HttpResponseMessage Put(string server, string route, string token, object obj)
        {
            var baseUrl = $"{server}/{route}";

            var json = JsonConvert.SerializeObject(obj);

            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = _httpClient.PutAsync(baseUrl, stringContent);

            return response.Result;
        }

        public HttpResponseMessage Patch(string server, string route, string token, object obj)
        {
            var baseUrl = $"{server}/{route}";

            var json = JsonConvert.SerializeObject(obj);

            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");

            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");

            var response = _httpClient.PatchAsync(baseUrl, stringContent);

            return response.Result;
        }
    }
}
