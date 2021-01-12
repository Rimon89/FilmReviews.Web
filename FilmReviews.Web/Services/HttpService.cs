using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace FilmReviews.Web.Services
{
    public class HttpService : IHttpService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly HttpClient _client;

        public HttpService(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
            _client = _clientFactory.CreateClient("filmReviewsAPI");
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _client.GetAsync(url);
        }
        public async Task<HttpResponseMessage> PostAsync<T>(string url, T content)
        {
            return await _client.PostAsJsonAsync(url, content);
        }
        public async Task<HttpResponseMessage> PutAsync<T>(string url, T content)
        {
            return await _client.PutAsJsonAsync(url, content);
        }

        public async Task<HttpResponseMessage> DeleteAsync(string url)
        {
            return await _client.DeleteAsync(url);
        }

        public async Task<T> DeserializeAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            T model = JsonConvert.DeserializeObject<T>(content);

            return model;
        }
    }
}
