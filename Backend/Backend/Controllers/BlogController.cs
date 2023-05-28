using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Models;
using Backend.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public BlogController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index(int page = 1, int take = 1)
        {
            BlogVM blogVM = new();

            blogVM.Categories = _appDbContext.Categories.ToList();

            blogVM.Tags = _appDbContext.Tags.ToList();

            blogVM.TrialTest = _appDbContext.TrialTests.FirstOrDefault();

            blogVM.Blogs = _appDbContext.Blogs
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();
            int pageCount = CalculatePageCount(_appDbContext.Blogs.ToList(), take);
            blogVM.Pagination = new(blogVM.Blogs, pageCount, page);

            blogVM.RecentBlogs = _appDbContext.Blogs.Where(t=>(t.Date.Hour-DateTime.Now.Hour)<5).ToList();


            return View(blogVM);
        }

        private int CalculatePageCount(List<Blog> blogs, int take)
        {
            return (int)Math.Ceiling((double)blogs.Count / (double)take);
        }

        public IActionResult Detail(int id)
        {
            BlogDetailVM blogDetailVM = new();

            blogDetailVM.Categories = _appDbContext.Categories.ToList();
            blogDetailVM.Tags = _appDbContext.Tags.ToList();
            blogDetailVM.TrialTest = _appDbContext.TrialTests.FirstOrDefault();
            blogDetailVM.Blog = _appDbContext.Blogs.Where(b=>b.Id==id).Include(b=>b.BlogContentImages).FirstOrDefault();
            blogDetailVM.RecentBlogs = _appDbContext.Blogs.Where(t => (t.Date.Hour - DateTime.Now.Hour) < 5).ToList();
            return View(blogDetailVM);
        }
    }
}

