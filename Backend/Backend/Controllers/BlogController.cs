using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Models;
using Backend.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    public class BlogController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public readonly UserManager<AppUser> _userManager;
        public readonly SignInManager<AppUser> _signInManager;
        public readonly RoleManager<IdentityRole> _roleManager;

        public BlogController(AppDbContext appDbContext, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
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

            blogVM.RecentBlogs = _appDbContext.Blogs.Where(t => (t.Date.Hour - DateTime.Now.Hour) < 5).ToList();


            return View(blogVM);
        }

        private int CalculatePageCount(List<Blog> blogs, int take)
        {
            return (int)Math.Ceiling((double)blogs.Count / (double)take);
        }

        public async Task<IActionResult> Detail(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
                ViewBag.UserId = user.Id;
            }
            BlogDetailVM blogDetailVM = new();

            blogDetailVM.Categories = _appDbContext.Categories.ToList();
            blogDetailVM.Tags = _appDbContext.Tags.ToList();
            blogDetailVM.TrialTest = _appDbContext.TrialTests.FirstOrDefault();
            blogDetailVM.Blog = _appDbContext.Blogs.Where(b => b.Id == id).Include(b => b.BlogContentImages).Include(b=>b.Comments).FirstOrDefault();
            blogDetailVM.RecentBlogs = _appDbContext.Blogs.Where(t => (t.Date.Hour - DateTime.Now.Hour) < 5).ToList();
            blogDetailVM.Blog.Comments = _appDbContext.Comments.Include(c => c.Blog).Include(c=>c.AppUser).ToList();
            return View(blogDetailVM);
        }

        [HttpPost]
        public async Task<IActionResult> AddComment(string commentMessage, int blogId)
        {
            AppUser user = new();

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }
            else
            {
                return RedirectToAction("Login", "Account");
            }


            //if(commentMessage == null) ModelState.AddModelError("c");

            Comment comment = new();
            comment.CreatedDate = DateTime.Now;
            comment.AppUserId = user.Id;
            comment.AppUser = user;
            comment.BlogId = blogId;
            comment.Blog = _appDbContext.Blogs.Where(b => b.Id == blogId).FirstOrDefault();
            comment.Message = commentMessage;
            _appDbContext.Comments.Add(comment);
            _appDbContext.SaveChanges();
            return RedirectToAction("detail", new { id = blogId });
        }

        public async Task<IActionResult> DeleteComment(int commentId)
        {
            if (commentId == null) return NotFound();

            Comment deletedComment = _appDbContext.Comments.FirstOrDefault(c => c.Id == commentId);
            if (deletedComment != null)
            {
                _appDbContext.Remove(deletedComment);
                _appDbContext.SaveChanges();

            }
            else return NotFound();

            return RedirectToAction("Detail", "Blog", new { id = deletedComment.BlogId });
        }
    }
}

