using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
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

    public class CategoryController : Controller
    {
        public readonly AppDbContext _appDbContext;

        public CategoryController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }
        // GET: /<controller>/
        public IActionResult Index(int page = 1, int take = 3)
        {
            List<Category> categories = _appDbContext.Categories
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();
            int pageCount = CalculatePageCount(_appDbContext.Categories.ToList(), take);
            PaginationVM<Category> pagination = new(categories, pageCount, page);
            return View(pagination);
        }

        private int CalculatePageCount(List<Category> categories, int take)
        {
            return (int)Math.Ceiling((double)categories.Count / (double)take);
        }

        public IActionResult Create()
        {
            ViewBag.Movies = new SelectList(_appDbContext.Movies.ToList(), "Id", "Name");
            ViewBag.TopRateds = new SelectList(_appDbContext.TopRateds.ToList(), "Id", "Name");
            ViewBag.TvSeries = new SelectList(_appDbContext.TvSeries.ToList(), "Id", "Name");
            ViewBag.Banners = new SelectList(_appDbContext.Banners.ToList(), "Id", "Title");


            return View();
        }

        [HttpPost]
        public IActionResult Create(CategoryCreateVM CategoryCreateVM)
        {

            if (CategoryCreateVM.Name == null)
            {
                ModelState.AddModelError("Name", "You have to enter a Name!");
                return View();
            }
            bool isExist = _appDbContext.Categories.Any(c => c.Name.ToLower() == CategoryCreateVM.Name.ToLower());

            if (isExist)
            {
                ModelState.AddModelError("Name", "There is already such category");
                return Create();
            }

            Category newCategory = new();
            if (CategoryCreateVM.MovieIds != null)
            {
                List<MovieCategory> movieCategories = new();
                foreach (var item in CategoryCreateVM.MovieIds)
                {
                    MovieCategory movieCategory = new();
                    movieCategory.CategoryId = newCategory.Id;
                    movieCategory.MovieId = item;
                    movieCategories.Add(movieCategory);
                }
                newCategory.MovieCategories = movieCategories;

            }

            if (CategoryCreateVM.TopRatedIds != null)
            {
                List<TopRatedCategory> topRatedCategories = new();
                foreach (var item in CategoryCreateVM.TopRatedIds)
                {
                    TopRatedCategory topRatedCategory = new();
                    topRatedCategory.CategoryId = newCategory.Id;
                    topRatedCategory.TopRatedId = item;
                    topRatedCategories.Add(topRatedCategory);
                }
                newCategory.TopRatedCategories = topRatedCategories;

            }

            if (CategoryCreateVM.TvSeriesIds != null)
            {
                List<TvSeriesCategories> tvSeriesCategories = new();
                foreach (var item in CategoryCreateVM.TvSeriesIds)
                {
                    TvSeriesCategories tvSeriesCategory = new();
                    tvSeriesCategory.CategoryId = newCategory.Id;
                    tvSeriesCategory.TvSeriesId = item;
                    tvSeriesCategories.Add(tvSeriesCategory);
                }
                newCategory.TvSeriesCategories = tvSeriesCategories;
            }

            newCategory.Name = CategoryCreateVM.Name;

            if (CategoryCreateVM.BannerId != null)
            {
                newCategory.BannerId = CategoryCreateVM.BannerId;
            }



            _appDbContext.Categories.Add(newCategory);
            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            Category existingCategory = _appDbContext.Categories.Include(m => m.MovieCategories).Include(m => m.TvSeriesCategories).Include(m => m.TopRatedCategories).FirstOrDefault(m => m.Id == id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            CategoryUpdateVM CategoryUpdateVM = new CategoryUpdateVM
            {
                Name= existingCategory.Name,
                BannerId = existingCategory.BannerId,
                MovieIds = existingCategory.MovieCategories.Select(mc => mc.MovieId).ToList(),
                TopRatedIds = existingCategory.TopRatedCategories.Select(mc => mc.TopRatedId).ToList(),
                TvSeriesIds = existingCategory.TvSeriesCategories.Select(mc => mc.TvSeriesId).ToList()

            };

            ViewBag.Movies = new SelectList(_appDbContext.Movies.ToList(), "Id", "Name");
            ViewBag.TopRateds = new SelectList(_appDbContext.TopRateds.ToList(), "Id", "Name");
            ViewBag.TvSeries = new SelectList(_appDbContext.TvSeries.ToList(), "Id", "Name");
            ViewBag.Banners = new SelectList(_appDbContext.Banners.ToList(), "Id", "Title");

            return View(CategoryUpdateVM);
        }




        [HttpPost]
        public IActionResult Edit(int id, CategoryUpdateVM CategoryUpdateVM)
        {

            if (CategoryUpdateVM.Name == null)
            {
                ModelState.AddModelError("Name", "You have to enter a Name!");
                return View();
            }

            bool isExist = _appDbContext.Categories.Any(c => c.Name.ToLower() == CategoryUpdateVM.Name.ToLower() && c.Id != id);

            if (isExist)
            {
                ModelState.AddModelError("Name", "There is already such category");
                return Edit(id);
            }



            Category existingCategory = _appDbContext.Categories.Include(m => m.MovieCategories).Include(m => m.TvSeriesCategories).Include(m => m.TopRatedCategories).FirstOrDefault(m => m.Id == id);
            if (existingCategory == null)
            {
                return NotFound();
            }



            existingCategory.Name = CategoryUpdateVM.Name;


            _appDbContext.MovieCategories.RemoveRange(existingCategory.MovieCategories);
            _appDbContext.TvSeriesCategories.RemoveRange(existingCategory.TvSeriesCategories);
            _appDbContext.TopRatedCategories.RemoveRange(existingCategory.TopRatedCategories);

            if (CategoryUpdateVM.TvSeriesIds != null)
            {
                List<MovieCategory> movieCategories = new();
                foreach (var movieId in CategoryUpdateVM.MovieIds)
                {
                    MovieCategory movieCategory = new MovieCategory
                    {
                        CategoryId = existingCategory.Id,
                        MovieId = movieId
                    };

                    movieCategories.Add(movieCategory);
                }
                existingCategory.MovieCategories = movieCategories;
            }

            if (CategoryUpdateVM.TvSeriesIds != null)
            {

                List<TvSeriesCategories> tvSeriesCategories = new();
                foreach (var tvSeriesId in CategoryUpdateVM.TvSeriesIds)
                {
                    TvSeriesCategories tvSeriesCategory = new TvSeriesCategories
                    {
                        CategoryId = existingCategory.Id,
                        TvSeriesId = tvSeriesId
                    };

                    tvSeriesCategories.Add(tvSeriesCategory);
                }
                existingCategory.TvSeriesCategories = tvSeriesCategories;
            }

            if (CategoryUpdateVM.TvSeriesIds != null)
            {

                List<TopRatedCategory> topRatedCategories = new();
                foreach (var topRatedId in CategoryUpdateVM.TopRatedIds)
                {
                    TopRatedCategory topRatedCategory = new TopRatedCategory
                    {
                        CategoryId = existingCategory.Id,
                        TopRatedId = topRatedId
                    };

                    topRatedCategories.Add(topRatedCategory);
                }
                existingCategory.TopRatedCategories = topRatedCategories;
            }



            _appDbContext.SaveChanges();

            return RedirectToAction("Index");
        }


        public IActionResult Delete(int id)
        {
            if (id == null) return NotFound();

            Category deletedCategory = _appDbContext.Categories.Find(id);

            if (deletedCategory == null) return NotFound();

            _appDbContext.Remove(deletedCategory);
            _appDbContext.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}

