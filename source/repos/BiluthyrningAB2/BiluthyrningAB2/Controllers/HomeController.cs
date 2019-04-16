using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BiluthyrningAB2.Models;
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
            var listOfCars = carServices.CarList();

            var temp = new RentCarVM
            {
                _cars = listOfCars.Select(y => new SelectListItem { Value = $"{y.Id}", Text = $"{y.Name}, {y.Model }, Pris per dag: {y.PricePerDay}" }).ToList(),
            };
            return View(temp);
        }

        [HttpPost]
        [Authorize]
        public IActionResult RentCar([FromForm] RentCarVM vM)
        {
            
            var listOfCars = carServices.CarList();
            var temp = new RentCarVM
            {
                _cars = listOfCars.Select(t => new SelectListItem { Selected = t.Id == vM.Id, Value = $"{t.Id}", Text = $"{t.Name}, {t.Model }, Pris per dag: {t.PricePerDay}" }).ToList(),
                Car = listOfCars.SingleOrDefault(m => m.Id == vM.Id),
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
            var userBooking = carServices.getBookings(userId);
            var bookingVMs = new BookingsVM
            {
                ListOfUserBookings = userBooking
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
            var listOfCars = carServices.getBookings(userId);
            var booked = listOfCars.Single(b => b.BookingNr == booking);
            var calculatedDays = Math.Floor((DateTime.Now - booked.BookingTime).TotalDays);
            var car = carServices.GetCarByRegNr(booked.RegNr);
            var temp = new PayCarVM
            {
                Bookings = booked,
                BookingId = booked.BookingNr,
                RegNr = car.RegistartionNumber,
                Car = car,
                Days = calculatedDays < 1 ? 1 : calculatedDays, 
                ReturnedDate = DateTime.Now
            };
           
            return  View("PayCar",temp); 
        }

        [HttpPost]
        [Authorize]
        public IActionResult PayCar(PayCarVM vM)
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userBookedCar = carServices.getBookings(userId);
            var booked = userBookedCar.Single(b => b.BookingNr == vM.BookingId);
            var calculatedDays = Math.Floor((DateTime.Now - booked.BookingTime).TotalDays);
            var car = carServices.GetCarByRegNr(booked.RegNr);
            var temp = new PayCarVM()
            {
                Bookings = booked,
                BookingId = booked.BookingNr,
                RegNr = car.RegistartionNumber,
                Car = car,
                Days = calculatedDays < 1 ? 1 : calculatedDays,
                ReturnedDate = DateTime.Now,
                KmDriven = vM.KmDriven,
                Price = car.CalculateTotalPrice((int)calculatedDays)
            };
            
            return View("PayBill",temp);
        }

        [HttpPost]
        [Authorize]
        public IActionResult PayBill(PayCarVM vM) 
        {
            var userId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var userBookedCar = carServices.getBookings(userId);
            var booked = userBookedCar.Single(b => b.BookingNr == vM.BookingId);
            var calculatedDays = Math.Floor((DateTime.Now - booked.BookingTime).TotalDays);
            var car = carServices.GetCarByRegNr(booked.RegNr);
            var temp = new PayCarVM()
            {
                Bookings = booked,
                BookingId = booked.BookingNr,
                RegNr = car.RegistartionNumber,
                Car = car,
                Days = calculatedDays < 1 ? 1 : calculatedDays,
                ReturnedDate = DateTime.Now,
                KmDriven = vM.KmDriven,
                Price = car.CalculateTotalPrice((int)(calculatedDays < 1 ? 1 : calculatedDays))
            };
            carServices.ReturnCar(temp, userId);
            return View("ThankYou",temp);
        }

        [HttpGet]
        [Authorize]
        public IActionResult ThankYou()
        {
            return View();
        }
       
    }
}