# Movie Reviews (Client)

En applikation där man kan söka efter filmer i Imdb med hjälp av ett öppet API, för att sedan kunna recensera dem. Applikation är skriven i .net core MVC.

## Installation

Här räcker det med att ladda hem projektet.

## Struktur

Här har jag också använt mig av olika SOLID principer, felhantering, asynkrona anrop och caching.

Exempel:

```cs
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
```

## Project Link

[Movie Reviews Client](https://github.com/Rimon89/FilmReviews.Web)
