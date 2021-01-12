namespace FilmReviews.Web.Common
{
    public static class Constants
    {
        public static class CacheKeys
        {
            public const string Review = "Review.Id:";
            public const string AllReviews = "Reviews";

            public const string Movie = "Movie.Id:";
        }

        public static class ApiRoutes
        {
            public static class Review
            {
                public const string RouteValues = Movie.GetId + "/" + Movie.GetTitle;
                public const string Details = "Details/{id}";
                public const string All = "AllReviews";
                public const string GetId = "{id}";
                public const string Edit = "Edit/{id}";
            }

            public static class Movie
            {
                public const string GetId = "{imdbId}";
                public const string GetTitle = "{movieTitle}";
            }
        }
    }
}
