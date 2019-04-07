using BiluthyrningAB2.Models.Cars;
using BiluthyrningAB2.Models.Entities;
using BiluthyrningAB2.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB2.Models
{
    public class CarServices
    {
        BiluthyrningABContext context;
        UserManager<MyIdentityUser> userManager;
        SignInManager<MyIdentityUser> signInManager;

        public CarServices(BiluthyrningABContext context,
        UserManager<MyIdentityUser> userManager,
        SignInManager<MyIdentityUser> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

       

        public List<Car> CarList()
        {
            List<Car> cars = new List<Car>();
            cars.Add(new SmallCar(1,200, 0, 2000, "Liten bil", "ABC 123", "Kia", DateTime.Today));
            cars.Add(new SmallCar(2, 200, 0, 2500, "Liten bil", "DEF 456", "Skoda", DateTime.Today));
            cars.Add(new SmallCar(3,200, 0, 6000, "Liten bil", "GHI 789", "Honda", DateTime.Today));
            cars.Add(new Van(4, 500, 0, 1000, "Van", 10, 0, "KIL 234", "Wolksvagen", DateTime.Today));
            cars.Add(new Van(5, 500, 0, 2300, "Van", 10, 0, "OOP 877", "Wolksvagen", DateTime.Today));
            cars.Add(new Van(6, 500, 0, 3000, "Van", 10, 0, "QLP 665", "Wolksvagen",DateTime.Today));
            cars.Add(new MiniBus(7, 800, 5000, "Minibuss", 12, 0, "PPL 332", "Puegot", DateTime.Today));
            cars.Add(new MiniBus(8, 800, 4211, "Minibuss", 12, 0, "YLO 887", "Puegot", DateTime.Today));
            cars.Add(new MiniBus(9, 800, 3000, "Minibuss", 12, 0, "TTO 998", "Puegot", DateTime.Today));

            return cars; 
        }


        public List<Days> DayList()
        {
            List<Days> days = new List<Days>()
            {
                new Days {Day = "1 dag", Value = 1, IsChecked = false},
                new Days {Day = "2 dagar", Value = 2, IsChecked = false},
                new Days {Day = "3 dagar", Value = 3, IsChecked = false},
                new Days {Day = "4 dagar", Value = 4, IsChecked = false},
                new Days {Day = "5 dagar", Value = 5, IsChecked = false},
                new Days {Day = "6 dagar", Value = 6, IsChecked = false},
                new Days {Day = "7 dagar", Value = 7, IsChecked = false},
                new Days {Day = "8 dagar", Value = 8, IsChecked = false},
                new Days {Day = "9 dagar", Value = 9, IsChecked = false}
            };
            ListOfDays obj = new ListOfDays();
            obj.Booked = days;

            return days; 
        }

        public void AddBooking(RentCarVM vM, string userId)
        {
            int bookingsnumber = 1;
            Bookings booking = new Bookings()
            {
                Model = vM.Car.Model,
                RegNr = vM.Car.RegistartionNumber,
                Km = (int)vM.Car.KmDriven,
                UserId = userId,
                BookingTime = DateTime.UtcNow,
                BookingNr = bookingsnumber++.ToString()
            };
            context.Bookings.Add(booking);
            context.SaveChanges();
        }

    }
}
