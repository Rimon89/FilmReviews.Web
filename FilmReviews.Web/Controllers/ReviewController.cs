using FilmReviews.Web.Common;
using FilmReviews.Web.Services;
using FilmReviews.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vereyon.Web;

namespace FilmReviews.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : Controller
    {
        private readonly IFlashMessage _flashMessage;
        private readonly IHttpService _httpService;
        private readonly CacheService _cacheService;

        public ReviewController(IFlashMessage flashMessage, IHttpService httpService, CacheService cacheService)
        {
            _flashMessage = flashMessage;
            _httpService = httpService;
            _cacheService = cacheService;
        }

        [HttpGet(Constants.ApiRoutes.Review.RouteValues)]
        public IActionResult Index(string imdbId, string movieTitle)
        {
            var review = new Review
            {
                MovieTitle = movieTitle,
                ImdbId = imdbId
            };
            return View(review);
        }

        [HttpGet(Constants.ApiRoutes.Review.Details)]
        public async Task<IActionResult> Details(Guid id)
        {
            if (!_cacheService.TryGetValue(Constants.CacheKeys.Review + id, out Review review))
            {
                var response = await _httpService.GetAsync($"api/review/{id}");

                if (!response.IsSuccessStatusCode)
                {
                    _flashMessage.Danger("An error occurred on the server.");
                    return RedirectToAction(nameof(GetAllReviews));
                }

                review = await _httpService.DeserializeAsync<Review>(response);
                _cacheService.Set(Constants.CacheKeys.Review + id, review);
            }
            return View(review);
        }

        [HttpGet(Constants.ApiRoutes.All)]
        public async Task<IActionResult> GetAllReviews()
        {
            if (!_cacheService.TryGetValue(Constants.CacheKeys.AllReviews, out List<Review> reviews))
            {
                var response = await _httpService.GetAsync("api/review/getall");

                if (!response.IsSuccessStatusCode)
                {
                    _flashMessage.Danger("An error occurred on the server.");
                    return RedirectToAction("Index", "Home");
                }

                reviews = await _httpService.DeserializeAsync<List<Review>>(response);
                _cacheService.Set(Constants.CacheKeys.AllReviews, reviews);
            }
            return View(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromForm] Review review)
        {
            if (ModelState.IsValid)
            {
                review.ReviewDate = DateTime.Now;
                var response = await _httpService.PostAsync("api/review/create", review);

                if (response.IsSuccessStatusCode)
                {
                    _cacheService.Remove(Constants.CacheKeys.AllReviews);
                    _flashMessage.Confirmation($"Thank you {review.Author} for the review of {review.MovieTitle}");
                    return RedirectToAction("Index", "Home");
                }

                _flashMessage.Danger("An error occurred on the server.");
            }
            return View(nameof(Index), review);
        }

        [HttpGet(Constants.ApiRoutes.Review.GetId)]
        public async Task<IActionResult> Delete(Guid id)
        {
            var response = await _httpService.DeleteAsync($"api/review/delete/{id}");

            if (response.IsSuccessStatusCode)
            {
                _cacheService.Remove(Constants.CacheKeys.AllReviews);
                _flashMessage.Confirmation("Deleted successfully");
                return RedirectToAction(nameof(GetAllReviews));
            }

            _flashMessage.Danger("An error occurred on the server.");
            return RedirectToAction(nameof(GetAllReviews));
        }

        [HttpGet(Constants.ApiRoutes.Review.Edit)]
        public async Task<IActionResult> Edit(Guid id)
        {
            var response = await _httpService.GetAsync($"api/review/{id}");

            if (response.IsSuccessStatusCode)
                return View(await _httpService.DeserializeAsync<Review>(response));

            _flashMessage.Warning("An error occurred on the server.");
            return RedirectToAction(nameof(GetAllReviews));
        }

        [HttpPost(Constants.ApiRoutes.Review.Edit)]
        public async Task<IActionResult> Edit([FromForm] Review review)
        {
            if (ModelState.IsValid)
            {
                review.ReviewDate = DateTime.Now;
                var response = await _httpService.PutAsync($"api/review/update", review);

                if (response.IsSuccessStatusCode)
                {
                    _cacheService.Remove(Constants.CacheKeys.Review + review.Id);
                    _cacheService.Remove(Constants.CacheKeys.AllReviews);
                    _flashMessage.Confirmation("Updated successfully");
                    return RedirectToAction(nameof(Edit));
                }

                _flashMessage.Danger("An error occurred on the server.");
            }
            return RedirectToAction(nameof(Edit));
        }
    }
}
