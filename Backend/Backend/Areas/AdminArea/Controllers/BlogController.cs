using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Extensions;
using Backend.Helpers;
using Backend.Models;
using Backend.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Areas.AdminArea.Controllers
{
    [Area("AdminArea")]
    [Authorize(Roles = "SuperAdmin, Admin")]

    public class BlogController : Controller
    {
        public readonly AppDbContext _appDbContext;
        private readonly IWebHostEnvironment _env;

        public BlogController(IWebHostEnvironment env, AppDbContext appDbContext)
        {
            _env = env;
            _appDbContext = appDbContext;
        }
        // GET: /<controller>/
        public IActionResult Index(int page = 1, int take = 3)
        {
            List<Blog> blogs = _appDbContext.Blogs
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();
            int pageCount = CalculatePageCount(_appDbContext.Blogs.ToList(), take);
            PaginationVM<Blog> pagination = new(blogs, pageCount, page);
            return View(pagination);
        }

        private int CalculatePageCount(List<Blog> blogs, int take)
        {
            return (int)Math.Ceiling((double)blogs.Count / (double)take);
        }

        public IActionResult Detail(int id)
        {
            Blog blog = _appDbContext.Blogs.Where(m => m.Id == id).FirstOrDefault();
            return View(blog);
        }

        public IActionResult Create()
        {
            ViewBag.Tags = new SelectList(_appDbContext.Tags.ToList(), "Id", "Name");

            return View();
        }

        [HttpPost]
        public IActionResult Create(BlogCreateVM BlogCreateVM)
        {
            if (BlogCreateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "You have to submit a file!");
                return View();
            }
            if (BlogCreateVM.Title == null)
            {
                ModelState.AddModelError("Title", "You have to enter a Title!");
                return View();
            }
            if (BlogCreateVM.Quote == null)
            {
                ModelState.AddModelError("Quote", "You have to enter a Quote!");
                return View();
            }
            if (BlogCreateVM.QuoteAuthor == null)
            {
                ModelState.AddModelError("QuoteAuthor", "You have to enter a QuoteAuthor!");
                return View();
            }
            if (BlogCreateVM.QuoteAuthorProfession == null)
            {
                ModelState.AddModelError("QuoteAuthorProfession", "You have to enter a QuoteAuthorProfession!");
                return View();
            }
            if (BlogCreateVM.DescTop == null)
            {
                ModelState.AddModelError("DescTop", "You have to enter a DescTop!");
                return View();
            }
            if (BlogCreateVM.DescBottom == null)
            {
                ModelState.AddModelError("DescBottom", "You have to enter a DescBottom!");
                return View();
            }
            if (BlogCreateVM.Like == null)
            {
                ModelState.AddModelError("Like", "You have to enter a Like!");
                return View();
            }
            if (BlogCreateVM.Date == null)
            {
                ModelState.AddModelError("Date", "You have to enter a Date!");
                return View();
            }

            if (!BlogCreateVM.Photo.IsImage())
            {

                ModelState.AddModelError("Photo", "You have to upload an image!");
                return View();

            }
            Blog newBlog = new();
            List<BlogTag> blogTags = new();
            foreach (var item in BlogCreateVM.TagIds)
            {
                BlogTag blogTag = new();
                blogTag.BlogId = newBlog.Id;
                blogTag.TagId = item;
                blogTags.Add(blogTag);
            }

            newBlog.ImageUrl = BlogCreateVM.Photo.SaveImage(_env, "assets/img/blog", BlogCreateVM.Photo.FileName);
            newBlog.Title = BlogCreateVM.Title;
            newBlog.Date = DateTime.Now;
            newBlog.DescBottom = BlogCreateVM.DescBottom;
            newBlog.DescTop = BlogCreateVM.DescTop;
            newBlog.Like = BlogCreateVM.Like;
            newBlog.Quote = BlogCreateVM.Quote;
            newBlog.QuoteAuthor = BlogCreateVM.QuoteAuthor;
            newBlog.QuoteAuthorProfession = BlogCreateVM.QuoteAuthorProfession;
            newBlog.BlogTags = blogTags;


            _appDbContext.Blogs.Add(newBlog);
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Blog existingBlog = _appDbContext.Blogs.Include(m => m.BlogTags).FirstOrDefault(m => m.Id == id);
            if (existingBlog == null)
            {
                return NotFound();
            }

            // Create a BlogEditVM object to pass the existing Blog data to the view
            BlogUpdateVM BlogUpdateVM = new BlogUpdateVM
            {
                Title = existingBlog.Title,
                Quote = existingBlog.Quote,
                QuoteAuthor = existingBlog.QuoteAuthor,
                Like = existingBlog.Like,
                DescBottom = existingBlog.DescBottom,
                DescTop = existingBlog.DescTop,
                QuoteAuthorProfession = existingBlog.QuoteAuthorProfession,
                Date = existingBlog.Date,
                TagIds = existingBlog.BlogTags.Select(mc => mc.TagId).ToList()
            };

            // Populate the CategoryList property of the view model with available categories
            ViewBag.Tags = new SelectList(_appDbContext.Tags.ToList(), "Id", "Name");


            return View(BlogUpdateVM);
        }




        [HttpPost]
        public IActionResult Edit(int id, BlogUpdateVM blogUpdateVM)
        {
            if (blogUpdateVM.Photo == null)
            {
                ModelState.AddModelError("Photo", "You have to submit a file!");
                return View();
            }
            if (blogUpdateVM.Title == null)
            {
                ModelState.AddModelError("Title", "You have to enter a Title!");
                return View();
            }
            if (blogUpdateVM.Quote == null)
            {
                ModelState.AddModelError("Quote", "You have to enter a Quote!");
                return View();
            }
            if (blogUpdateVM.QuoteAuthor == null)
            {
                ModelState.AddModelError("QuoteAuthor", "You have to enter a QuoteAuthor!");
                return View();
            }
            if (blogUpdateVM.QuoteAuthorProfession == null)
            {
                ModelState.AddModelError("QuoteAuthorProfession", "You have to enter a QuoteAuthorProfession!");
                return View();
            }
            if (blogUpdateVM.DescTop == null)
            {
                ModelState.AddModelError("DescTop", "You have to enter a DescTop!");
                return View();
            }
            if (blogUpdateVM.DescBottom == null)
            {
                ModelState.AddModelError("DescBottom", "You have to enter a DescBottom!");
                return View();
            }
            if (blogUpdateVM.Like == null)
            {
                ModelState.AddModelError("Like", "You have to enter a Like!");
                return View();
            }
            if (blogUpdateVM.Date == null)
            {
                ModelState.AddModelError("Date", "You have to enter a Date!");
                return View();
            }
            if (!blogUpdateVM.Photo.IsImage())
            {
                ModelState.AddModelError("Photo", "You have to upload an image!");
                return View();
            }

            Blog existingBlog = _appDbContext.Blogs.Include(m => m.BlogTags).FirstOrDefault(m => m.Id == id);
            if (existingBlog == null)
            {
                return NotFound();
            }

            string fullPath = Path.Combine(_env.WebRootPath, "assets/img/blog", existingBlog.ImageUrl);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            existingBlog.ImageUrl = blogUpdateVM.Photo.SaveImage(_env, "assets/img/blog", blogUpdateVM.Photo.FileName);
            existingBlog.Title = blogUpdateVM.Title;
            existingBlog.Quote = blogUpdateVM.Quote;
            existingBlog.QuoteAuthor = blogUpdateVM.QuoteAuthor;
            existingBlog.QuoteAuthorProfession = blogUpdateVM.QuoteAuthorProfession;
            existingBlog.Like = blogUpdateVM.Like;
            existingBlog.Date = blogUpdateVM.Date;

            existingBlog.DescTop = blogUpdateVM.DescTop;
            existingBlog.DescBottom = blogUpdateVM.DescBottom;

            _appDbContext.BlogTags.RemoveRange(existingBlog.BlogTags);

            List<BlogTag> blogTags = new List<BlogTag>();
            foreach (var tagId in blogUpdateVM.TagIds)
            {
                BlogTag blogTag = new BlogTag
                {
                    BlogId = existingBlog.Id,
                    TagId = tagId
                };

                blogTags.Add(blogTag);
            }

            existingBlog.BlogTags = blogTags;

            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();

            Blog deletedBlog = _appDbContext.Blogs.Find(id);

            if (deletedBlog == null) return NotFound();

            _appDbContext.Remove(deletedBlog);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

