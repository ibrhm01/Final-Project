using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModels
{
    public class BlogCreateVM
    {
        [Required]
        public IFormFile Photo { get; set; }
        [Required]
        public string ImageUrl { get; set; }
        [Required]
        public DateTime Date { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string DescTop { get; set; }
        [Required]
        public string DescBottom { get; set; }
        [Required]
        public int Like { get; set; }
        [Required]
        public string Quote { get; set; }
        [Required]
        public string QuoteAuthor { get; set; }
        [Required]
        public string QuoteAuthorProfession { get; set; }
        [Required]
        public List<int> TagIds { get; set; }
    }
}

