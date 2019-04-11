﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
        BiluthyrningABContext context;

        public HomeController(AccountServices service, UserManager<MyIdentityUser> userManager,
            SignInManager<MyIdentityUser> signInManager, CarServices carServices, IConfiguration config,
            BiluthyrningABContext context)
        {
            this.service = service;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.carServices = carServices;
            Configuration = config;
            this.context = context;
            
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
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            carServices.AddBooking(temp, userId);
            return View("SelectedCar",temp);
             
        }

        [HttpGet]
        [Authorize]
        public IActionResult SelectedCar(RentCarVM vm)
        {
            return View(vm);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ReturnCar()
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var bookings = carServices.getBookings(userId);
            var bookingVMs = new BookingsVM
            {
                ListOfUserBookings = bookings
            };

            return View(bookingVMs);
        
        }
        
        [HttpPost]
        [Authorize]
        public IActionResult ReturnCar(BookingsVM vM)
        {
            return View("PayCar",vM);
        }

        [HttpGet]
        [Authorize]
        public IActionResult PayCar(string booking)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var x = carServices.getBookings(userId);
            var booked = x.Single(b => b.BookingNr == booking);
            var car = carServices.GetCarByRegNr(booked.RegNr);
            var temp = new PayCarVM
            {
                Bookings = booked,
                 Car = car,
                 Days = Math.Round((DateTime.Now -booked.BookingTime).TotalDays)
                
            };
            return  View(temp);
        }

        [HttpPost]
        [Authorize]
        public IActionResult PayCar()
        {
            //Bygg ett formulär där du med infon du har räknar ut hur mycket som ska
            //betalas när användaren lämnar tillbaka bilen.
            //Se till att skicka till databas när bilen är tillbaka.
            return null; 
        }

    }
}