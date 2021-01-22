namespace FilmReviews.Web.Common
{
    public static class Constants
    {
        public static class CacheKeys
        {
            public const string Review = "Review.Id:";
            public const string AllReviews = "Reviews";

            public const string Movie = "Movie.Id:";
            public const string AllMovies = "Movies";
        }

        public static class ApiRoutes
        {
            public const string All = "All";
            public static class Review
            {
                public const string RouteValues = Movie.GetId + "/" + Movie.GetTitle;
                public const string Details = "Details/{id}";
                public const string GetId = "{id}";
                public const string Edit = "Edit/{id}";
            }

            public static class Movie
            {
                public const string Details = "Details/{imdbId}";
                public const string GetTitle = "{movieTitle}";
                public const string GetId = "{imdbId}";
                public const string Delete = "Delete/{imdbId}";
            }
        }
    }
}
