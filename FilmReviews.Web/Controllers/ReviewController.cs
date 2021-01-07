using FilmReviews.Web.Services;
using FilmReviews.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
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

        public ReviewController(IFlashMessage flashMessage, IHttpService httpService)
        {
            _flashMessage = flashMessage;
            _httpService = httpService;
        }

        [HttpGet("{imdbId}/{movieTitle}")]
        public IActionResult Index(string imdbId, string movieTitle)
        {
            var review = new Review
            {
                MovieTitle = movieTitle,
                ImdbId = imdbId
            };
            return View(review);
        }

        [HttpGet("Details/{id}")]
        public async Task<IActionResult> Details(Guid id)
        {
            using (var response = await _httpService.GetAsync($"api/review/{id}"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    _flashMessage.Warning("An error occurred on the server.");
                    return RedirectToAction(nameof(GetAllReviews));
                }
                return View(await _httpService.DeserializeAsync<Review>(response));
            }
        }

        [HttpGet("AllReviews")]
        public async Task<IActionResult> GetAllReviews()
        {
            using (var response = await _httpService.GetAsync("api/review/getall"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    _flashMessage.Warning("An error occurred on the server.");
                    return RedirectToAction("Index", "Home");
                }
                return View(await _httpService.DeserializeAsync<List<Review>>(response));
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromForm] Review review)
        {
            if (ModelState.IsValid)
            {
                review.ReviewDate = DateTime.Now;
                using (var response = await _httpService.PostAsync("api/review/create", review))
                {
                    if (response.IsSuccessStatusCode)
                    {
                        _flashMessage.Confirmation($"Thank you {review.Author} for the review of {review.MovieTitle}");
                        return RedirectToAction("Index", "Home");
                    }
                    _flashMessage.Warning("An error occurred on the server.");
                }
            }
            return View(nameof(Index), review);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            using (var response = await _httpService.DeleteAsync($"api/review/delete/{id}"))
            {
                if (response.IsSuccessStatusCode)
                {
                    _flashMessage.Confirmation("Deleted successfully");
                    return RedirectToAction(nameof(GetAllReviews));
                }
                _flashMessage.Danger("An error occurred on the server.");
                return RedirectToAction(nameof(GetAllReviews));
            }
        }

        [HttpGet("Edit/{id}")]
        public async Task<IActionResult> Edit(Guid id)
        {
            using (var response = await _httpService.GetAsync($"api/review/{id}"))
            {
                if (!response.IsSuccessStatusCode)
                {
                    _flashMessage.Warning("An error occurred on the server.");
                    return RedirectToAction(nameof(GetAllReviews));
                }
                return View(await _httpService.DeserializeAsync<Review>(response));
            }
        }

        [HttpPost("Edit/{id}")]
        public async Task<IActionResult> Edit([FromForm] Review review)
        {
            if (ModelState.IsValid)
            {
                review.ReviewDate = DateTime.Now;
                using (var response = await _httpService.PutAsync($"api/review/update", review))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        _flashMessage.Warning("An error occurred on the server.");
                        return RedirectToAction(nameof(Edit));
                    }
                    _flashMessage.Confirmation("Updated successfully");
                }
            }
            return RedirectToAction(nameof(Edit));
        }
    }
}
