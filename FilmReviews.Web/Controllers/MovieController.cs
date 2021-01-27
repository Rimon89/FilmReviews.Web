using FilmReviews.Web.Common;
using FilmReviews.Web.Services;
using FilmReviews.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Net;
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

        [HttpGet(Constants.ApiRoutes.Movie.Details)]
        public async Task<IActionResult> Details(string imdbId)
        {
            var response = await _httpService.GetAsync($"api/movie/{imdbId}");

            Movie movie;
            if (!response.IsSuccessStatusCode)
            {
                movie = null;

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    _flashMessage.Warning("Movie Not Found");
                    return View(movie);
                }

                _flashMessage.Danger("An error occurred on the server.");
                return View(movie);
            }
            _cacheService.Remove(Constants.CacheKeys.AllMovies);
            movie = await _httpService.DeserializeAsync<Movie>(response);
 
            return View(movie);
        }

        [HttpGet(Constants.ApiRoutes.All)]
        public async Task<IActionResult> GetAllMovies()
        {
            if (!_cacheService.TryGetValue(Constants.CacheKeys.AllMovies, out List<Movie> movies))
            {
                var response = await _httpService.GetAsync("api/movie/getall");

                if (!response.IsSuccessStatusCode)
                {
                    _flashMessage.Danger("An error occurred on the server.");
                    return RedirectToAction("Index", "Home");
                }

                movies = await _httpService.DeserializeAsync<List<Movie>>(response);
                _cacheService.Set(Constants.CacheKeys.AllMovies, movies, new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddDays(1),
                    SlidingExpiration = TimeSpan.FromMinutes(30)
                });
            }
            return View(movies);
        }

        [HttpGet(Constants.ApiRoutes.Movie.Delete)]
        public async Task<IActionResult> Delete(string imdbId)
        {
            var response = await _httpService.DeleteAsync($"api/movie/delete/{imdbId}");

            if (response.IsSuccessStatusCode)
            {
                _cacheService.Remove(Constants.CacheKeys.AllMovies);
                _cacheService.Remove(Constants.CacheKeys.AllReviews);
                return RedirectToAction(nameof(GetAllMovies));
            }

            return BadRequest();
        }
    }
}
