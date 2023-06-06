using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Backend.DAL;
using Backend.Models;
using Backend.ViewModels;
using Backend.ViewModels.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Controllers
{
    public class PricingController : Controller
    {
        private readonly AppDbContext _appDbContext;
        public readonly UserManager<AppUser> _userManager;


        public PricingController(AppDbContext appDbContext, UserManager<AppUser> userManager)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
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
            UserSubscriptionVM userSubscriptionVM = new();
            userSubscriptionVM.Pricing = _appDbContext.Pricings.Where(p => p.Id == id).FirstOrDefault();
            if (User.Identity.IsAuthenticated)
            {
                return View(userSubscriptionVM.Pricing);

            }

            return RedirectToAction("Login", "Account");

        }



        public async Task<IActionResult> AddSubscription(int id)
        {
            if (id == null) return NotFound();
            UserSubscriptionVM userSubscriptionVM = new();

            userSubscriptionVM.Pricing = _appDbContext.Pricings.Where(p => p.Id == id).FirstOrDefault();

            if (userSubscriptionVM.Pricing == null) return NotFound();

            if (User.Identity.IsAuthenticated)
            {
                userSubscriptionVM.User = await _userManager.FindByNameAsync(User.Identity.Name);

                if (userSubscriptionVM.User != null)
                {
                    userSubscriptionVM.User.PricingId = userSubscriptionVM.Pricing.Id;
                    IdentityResult result = await _userManager.UpdateAsync(userSubscriptionVM.User);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index", "Pricing");

                    }
                    else
                    {
                        return Subscribe(id);
                    }

                }
                else NotFound();

            }

            return RedirectToAction("Login", "Account");

        }

        
    }
}

