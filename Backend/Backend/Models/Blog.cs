using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Blog : BaseEntity
    {
        public string ImageUrl { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string DescTop { get; set; }
        public string DescBottom { get; set; }
        public int Like { get; set; }
        public int Comment { get; set; }
        public string Quote { get; set; }
        public string QuoteAuthor { get; set; }
        public string QuoteAuthorProfession { get; set; }
        public List<BlogContentImage> BlogContentImages { get; set; }
        
        //public string BlogAuthorImageUrl { get; set; }
        //public string BlogAuthorDesc{ get; set; }


    }
}

