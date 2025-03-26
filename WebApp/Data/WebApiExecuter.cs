using System.Text.Json;
using WebApp.Models;

namespace WebApp.Data
{
    public class WebApiExecuter : IWebApiExecuter
    {
        public WebApiExecuter(IHttpClientFactory httpClientFactory)
        {
            HttpClientFactory = httpClientFactory;
        }
        private const string apiName = "ShirtsApi";
        public IHttpClientFactory HttpClientFactory { get; }

        public async Task<T?> InvokeGet<T>(string relativeUrl)
        {
            var httpClient = HttpClientFactory.CreateClient(apiName);
            var request = new HttpRequestMessage(HttpMethod.Get, relativeUrl);
            var response = await httpClient.SendAsync(request);
            await HandlePotentialError(response);
            
            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task<T?> InvokePost<T>(string relativeUrl, T data)
        {
            var httpClient = HttpClientFactory.CreateClient(apiName);
            var response = await httpClient.PostAsJsonAsync(relativeUrl, data);
            await HandlePotentialError(response);

            return await response.Content.ReadFromJsonAsync<T>();
        }

        public async Task InvokePut<T>(string relativeUrl, T data)
        {
            var httpClient = HttpClientFactory.CreateClient(apiName);
            var response = await httpClient.PutAsJsonAsync(relativeUrl, data);

            await HandlePotentialError(response);
        }

        public async Task InvokeDelete(string relativeUrl)
        {
            var httpClient = HttpClientFactory.CreateClient(apiName);
            var response = await httpClient.DeleteAsync(relativeUrl);

            await HandlePotentialError(response);
        }

        private async Task HandlePotentialError(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                var errorJson = await response.Content.ReadAsStringAsync();
                throw new WebAPIException(errorJson);
            }
        }
    }
}
