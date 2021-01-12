using System.Net.Http;
using System.Threading.Tasks;

namespace FilmReviews.Web.Services
{
    public interface IHttpService
    {
        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync<T>(string url, T content);
        Task<HttpResponseMessage> PutAsync<T>(string url, T content);
        Task<HttpResponseMessage> DeleteAsync(string url);
        Task<T> DeserializeAsync<T>(HttpResponseMessage response);
    }
}
