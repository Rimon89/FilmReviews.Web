using FilmReviews.Web.Services;
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
using Vereyon.Web;

namespace FilmReviews.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MovieController : Controller
    {
        private readonly IFlashMessage _flashMessage;
        private readonly IHttpService _httpService;
        Movie movie;

        public MovieController(IHttpService httpService, IFlashMessage flashMessage)
        {
            _httpService = httpService;
            _flashMessage = flashMessage;
        }

        [HttpGet("{imdbId}")]
        public async Task<IActionResult> Details(string imdbId)
        {
            using (var response = await _httpService.GetAsync($"api/movie/{imdbId}"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    movie = null;
                    _flashMessage.Warning("An error occurred on the server.");

                    return View(movie);
                }
                movie = await _httpService.DeserializeAsync<Movie>(response);
            }
            return View(movie);
        }
    }
}
