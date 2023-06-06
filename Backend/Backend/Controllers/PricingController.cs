using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Models;
using Backend.ViewModels;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    public class PricingController : Controller
    {
        private readonly AppDbContext _appDbContext;

        public PricingController(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IActionResult Index()
        {
            PricingVM pricingVM = new();

            pricingVM.Pricings = _appDbContext.Pricings.Where(s => s.IsDeleted != true).ToList();

            pricingVM.TrialTest = _appDbContext.TrialTests.Where(s => s.IsDeleted != true).FirstOrDefault();
            

            return View(pricingVM);
        }



        public IActionResult Subscribe(int id)
        {
            Pricing pricing = _appDbContext.Pricings.Where(p => p.Id == id).FirstOrDefault();


            return View(pricing);
        }
    }
}

