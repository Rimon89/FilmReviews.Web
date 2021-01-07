using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace FilmReviews.Web.Services
{
    public interface IHttpService
    {
        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync(string url, object content);
        Task<HttpResponseMessage> PutAsync(string url, object content);
        Task<HttpResponseMessage> DeleteAsync(string url);
        Task<T> DeserializeAsync<T>(HttpResponseMessage response);
    }
}
