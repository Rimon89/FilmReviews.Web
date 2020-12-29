using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FilmReviews.Web.ViewModels
{
    public class Review
    {
        public string MovieTitle { get; set; }
        public string ImdbId { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string MovieReview { get; set; }

        [Required]
        [Range(1, 5, ErrorMessage = "A rating between 1 and 5 is required!")]
        public int Rating { get; set; }

        [Required]
        public string Author { get; set; }
    }
}
