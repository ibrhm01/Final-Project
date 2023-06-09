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
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Backend.Areas.AdminArea.Controllers

{
    [Area("AdminArea")]
    [Authorize(Roles = "SuperAdmin, Admin")]

    public class RoleController : Controller
    {
        private RoleManager<IdentityRole> _roleManager;

        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        // GET: /<controller>/
        public IActionResult Index(int page = 1, int take = 3)
        {
            List<IdentityRole> roles = _roleManager.Roles
                .Skip((page - 1) * take)
                .Take(take)
                .ToList();
            int pageCount = CalculatePageCount(_roleManager.Roles.ToList(), take);
            PaginationVM<IdentityRole> pagination = new(roles, pageCount, page);
            return View(pagination);
        }

        private int CalculatePageCount(List<IdentityRole> roles, int take)
        {
            return (int)Math.Ceiling((double)roles.Count / (double)take);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole role)
        {
            if (!string.IsNullOrEmpty(role.Name))
            {
                if (!await _roleManager.RoleExistsAsync(role.Name))
                {
                    var result = await _roleManager.CreateAsync(role);
                    if (result.Succeeded)
                    {

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    ModelState.AddModelError("Name", "There is already such role!");
                    return View();
                }

            }
            ModelState.AddModelError("Name", "You have to enter a Name!");
            return View();

        }

        public async Task<IActionResult> Edit(string id)
        {
            if (id == null) return NotFound();

            IdentityRole updatedRole = await _roleManager.FindByIdAsync(id);

            if (updatedRole == null) return NotFound();

            return View(new IdentityRole
            {
               Name= updatedRole.Name
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(string id, IdentityRole role)
        {
            if (id == null) return NotFound();

            IdentityRole updatedRole = await _roleManager.FindByIdAsync(id);

            if (updatedRole == null) return NotFound();


            if (String.IsNullOrEmpty(role.Name))
            {
                ModelState.AddModelError("Name", "You have to enter a Name!");
                return View();
            }
            updatedRole.Name = role.Name;
            var result = await _roleManager.UpdateAsync(updatedRole);
            if (result.Succeeded)
            {
                return RedirectToAction("Index");

            }
            return View();
        }

        public async Task<IActionResult> Delete(string id)
        {
            if (id == null) return NotFound();
            IdentityRole role = await _roleManager.FindByIdAsync(id);
            if (role != null)
            {
                var result=await _roleManager.DeleteAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                return View();
            }
            return NotFound();
           
        }
    }
}

