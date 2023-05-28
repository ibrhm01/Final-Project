using System;
using Backend.Models;

namespace Backend.ViewModels
{
	public class BlogDetailVM
	{
        public List<Category> Categories { get; set; }

        public List<Tag> Tags { get; set; }

        public TrialTest TrialTest { get; set; }

        public Blog Blog { get; set; }

        public List<Blog> RecentBlogs { get; set; }
    }
}

