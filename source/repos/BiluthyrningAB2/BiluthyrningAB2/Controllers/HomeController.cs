using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BiluthyrningAB2.Models;
using BiluthyrningAB2.Models.Cars;
using BiluthyrningAB2.Models.Entities;
using BiluthyrningAB2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;

namespace BiluthyrningAB2.Controllers
{

    public class HomeController: Controller
    {
        public IConfiguration Configuration { get; set; }
        AccountServices service;
        UserManager<MyIdentityUser> userManager;
        SignInManager<MyIdentityUser> signInManager;
        CarServices carServices;

        public HomeController(AccountServices service, UserManager<MyIdentityUser> userManager,
            SignInManager<MyIdentityUser> signInManager, CarServices carServices, IConfiguration config)
        {
            this.service = service;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.carServices = carServices;
            Configuration = config;
        }

        [HttpGet]
        [Route("")]
        public IActionResult Home()
        {
            return View();
        }

        [HttpPost]
        [Route("")]
        public IActionResult Home(HomeVM vM)
        {
            return View(vM);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM vM)
        {
            if (!ModelState.IsValid)
                return View(vM);
            var succeded = await service.RegisterUser(vM);
            if (!succeded)
                return View(vM);
            else
                return RedirectToAction(nameof(Home));
        }

        [HttpGet]
        public IActionResult Login(LogInVM vM)
        {
            return View(vM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LogInVM vM, string returnUrl = null)
        {
            returnUrl = returnUrl ?? Url.Content("~");
            if (ModelState.IsValid)
            {
                var result = await signInManager.PasswordSignInAsync(vM.UserName, vM.Password, false, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tyvärr kunde du inte loggas in, försök igen");
                    return RedirectToAction("Login");

                }
            }
            return Redirect("/");
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> LogOut()
        {
             await service.LogOutUser();
            return Redirect("/");
        }

        [HttpGet]
        [Authorize]
        public IActionResult RentCar()
        {
            var x = carServices.CarList();
            var days = carServices.DayList();

            var temp = new RentCarVM
            {
                _cars = x.Select(y => new SelectListItem { Value = $"{y.Id}", Text = $"{y.Name}, {y.Model }, Pris per dag: {y.PricePerDay}" }).ToList(),
                _days = days.Select(p => new SelectListItem { Value = $"{p.Value}", Text = $"{p.Day}" }).ToList()
            };
            return View(temp);
        }

        [HttpPost]
        [Authorize]
        public IActionResult RentCar([FromForm] RentCarVM vM)
        {
            
            var x = carServices.CarList();
            var days = carServices.DayList();
            var temp = new RentCarVM
            {
                _cars = x.Select(t => new SelectListItem { Selected = t.Id == vM.Id, Value = $"{t.Id}", Text = $"{t.Name}, {t.Model }, Pris per dag: {t.PricePerDay}" }).ToList(),
                Car = x.SingleOrDefault(m => m.Id == vM.Id),
                _days = days.Select(p => new SelectListItem { Selected = p.Value == vM.DaysId, Value = $"{p.Value}", Text = $"{p.Day}"}).ToList(),
                Days = days.SingleOrDefault(n => n.Value == vM.DaysId)
            };

            return View("SelectedCar",temp);
             
        }

        [HttpGet]
        [Authorize]
        public IActionResult SelectedCar(RentCarVM vm)
        {
            return View(vm);
        }

    }
}