using FilmReviews.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace FilmReviews.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;
        Movie movie;

        public MovieController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("{imdbId}")]
        public async Task<IActionResult> Details(string imdbId)
        {
            var client = _clientFactory.CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"https://localhost:44318/api/movie/{imdbId}");

            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var x = await response.Content.ReadAsStringAsync();
                movie = JsonConvert.DeserializeObject<Movie>(x);
            }
            else
            {
                movie = null;
            }

            return View(movie);
        }
    }
}
