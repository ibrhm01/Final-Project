﻿using System;
using Backend.Models;

namespace Backend.ViewModels
{
	public class BlogVM
	{
        public List<Category> Categories { get; set; }

        public List<Tag> Tags { get; set; }

        public TrialTest TrialTest { get; set; }

        public List<Blog> Blogs { get; set; }

        public List<Blog> RecentBlogs { get; set; }



    }
}

