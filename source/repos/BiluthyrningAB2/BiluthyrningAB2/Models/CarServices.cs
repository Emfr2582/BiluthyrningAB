using BiluthyrningAB2.Models.Cars;
using BiluthyrningAB2.Models.Entities;
using BiluthyrningAB2.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public Car GetCarByRegNr(string regNr)
        {
            return CarList().SingleOrDefault(x => x.RegistartionNumber == regNr);
        }

        public void AddBooking(RentCarVM vM, string userId)
        {
            Bookings booking = new Bookings()
            {
                Model = vM.Car.Model,
                RegNr = vM.Car.RegistartionNumber,
                Km = (int)vM.Car.KmDriven,
                UserId = userId,
                BookingTime = DateTime.UtcNow,
                BookingNr = Guid.NewGuid().ToString(),
            };
            context.Bookings.Add(booking);
            context.SaveChanges();
        }

        public void ReturnCar(PayCarVM vM,string userId)
        {
            ReturnedCars returnCar = new ReturnedCars()
            {
                UserId = userId,
                RegNr = vM.Bookings.RegNr,
                BookingNr = vM.Bookings.BookingNr,
                KmDriven = vM.KmDriven,
                ReturnedTime = vM.ReturnedDate,
                Price = vM.Price
            };

            context.ReturnedCars.Add(returnCar);
            context.SaveChanges();
           
        }

        public List<Bookings> getBookings(string userId)
        {
            return context.Bookings.Where(q => q.UserId == userId).ToList();
        }




    }
}
