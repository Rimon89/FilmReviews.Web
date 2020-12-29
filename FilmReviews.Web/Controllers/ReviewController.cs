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

namespace FilmReviews.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewController : Controller
    {
        private readonly IHttpClientFactory _clientFactory;

        public ReviewController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        [HttpGet("{imdbId}/{movieTitle}")]
        public IActionResult Index(string imdbId, string movieTitle)
        {
            var review = new Review();
            review.MovieTitle = movieTitle;
            review.ImdbId = imdbId;
            return View(review);
        }

        [HttpPost]
        public IActionResult AddReview([FromForm] Review review)
        {
            if (ModelState.IsValid)
            {
                var client = _clientFactory.CreateClient("local");

                var response = client.PostAsJsonAsync("api/review/create", review).Result;

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Index", "Home");
            }
            return View(nameof(Index), review);
        }
    }
}
