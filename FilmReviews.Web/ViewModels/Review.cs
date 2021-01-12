using System;
using System.ComponentModel.DataAnnotations;

namespace FilmReviews.Web.ViewModels
{
    public class Review
    {
        public Guid Id { get; set; }

        [Required]
        [Display(Name = "Movie title")]
        public string MovieTitle { get; set; }

        [Required]
        [Display(Name = "Imdb-id")]
        public string ImdbId { get; set; }

        [Required]
        public string Name { get; set; }

        [Display(Name = "Created/Updated")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy H:mm}")]
        public DateTime ReviewDate { get; set; }

        [Required]
        [Display(Name = "Review text")]
        public string MovieReview { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "A rating between 1 and 5 is required!")]
        public int Rating { get; set; }

        [Required]
        public string Author { get; set; }
    }
}
