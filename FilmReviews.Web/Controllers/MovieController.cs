using FilmReviews.Web.Common;
using FilmReviews.Web.Services;
using FilmReviews.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
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
        private readonly CacheService _cacheService;

        public MovieController(IHttpService httpService, IFlashMessage flashMessage, CacheService cacheService)
        {
            _httpService = httpService;
            _flashMessage = flashMessage;
            _cacheService = cacheService;
        }

        [HttpGet(Constants.ApiRoutes.Movie.GetId)]
        public async Task<IActionResult> Details(string imdbId)
        {
            if (!_cacheService.TryGetValue<Movie>(Constants.CacheKeys.Movie + imdbId, out Movie movie))
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
                    _cacheService.Set(Constants.CacheKeys.Movie + imdbId, movie, new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddHours(1),
                        SlidingExpiration = TimeSpan.FromMinutes(5)
                    });
                }
            }
            return View(movie);
        }
    }
}
