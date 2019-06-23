using IntegrantFirstTask.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace IntegrantFirstTask.Clients
{
    class APISManager : IAPISManager
    {
        public async Task<T> GetAsync<T>(string uri)
        {
            var HttpClient = new HttpClient();
            HttpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpClient.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");

            HttpResponseMessage response = await HttpClient.GetAsync(uri);
            string serialized = await response.Content.ReadAsStringAsync();

            var result = await Task.Run(() => JsonConvert.DeserializeObject<T>(serialized));

            return result;

        }

        public async Task<TResult> PostAsync<T, TResult>(string uri, T Data)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("ZUMO-API-VERSION", "2.0.0");

            var content = new StringContent(JsonConvert.SerializeObject(Data));
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            HttpResponseMessage response = await httpClient.PostAsync(uri, content);

            string serialized = await response.Content.ReadAsStringAsync();

            TResult result = await Task.Run(() =>
                JsonConvert.DeserializeObject<TResult>(serialized));

            return result;
        }
    }
}
