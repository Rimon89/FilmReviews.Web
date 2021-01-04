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
        private readonly IHttpClientFactory _clientFactory;
        private readonly IFlashMessage FlashMessage;

        public ReviewController(IHttpClientFactory clientFactory, IFlashMessage flashMessage)
        {
            _clientFactory = clientFactory;
            FlashMessage = flashMessage;
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

        [HttpGet]
        public async Task<IActionResult> GetAllReviews()
        {
            var client = _clientFactory.CreateClient("filmReviewsAPI");

            var request = new HttpRequestMessage(HttpMethod.Get,
                $"api/review/getall");

            var response = await client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return BadRequest();

            var content = await response.Content.ReadAsStringAsync();
            var reviews = JsonConvert.DeserializeObject<List<Review>>(content);

            return View(reviews);
        }

        [HttpPost]
        public async Task<IActionResult> AddReview([FromForm] Review review)
        {
            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient("filmReviewsAPI");

                var response = await client.PostAsJsonAsync("api/review/create", review);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index", "Home");
            }
            return View(nameof(Index), review);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var client = _clientFactory.CreateClient("filmReviewsAPI");

            var response = await client.DeleteAsync($"api/review/delete/{id}");

            if(response.IsSuccessStatusCode)
                return RedirectToAction(nameof(GetAllReviews));

            var content = await response.Content.ReadAsStringAsync();
            FlashMessage.Danger(content);
            return RedirectToAction(nameof(GetAllReviews));
        }
    }
}
