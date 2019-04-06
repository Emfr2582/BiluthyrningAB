using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB2.Models.Cars
{
    public class SmallCar : Car
    {
        public SmallCar(int id, decimal pricePerDay,
            int rentedDays, double kmDriven, string model,
            string registrationNumber, string name, DateTime startToRent) 
            : base(id, model, name, pricePerDay,
         kmDriven,registrationNumber, startToRent)
        {
            
        }

        public override decimal CalculateTotalPrice(int rentedDays)
        {
            return PricePerDay * rentedDays;
        }

        public override int TotalDaysRented(DateTime startToRent)
        {

            return 0;
        }
    }
}
