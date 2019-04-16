using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB2.Models.Cars
{
   public class MiniBus : Car
    {
        public decimal KmPrice { get; set; }
        public decimal NumberOfKm { get; set; }

        public MiniBus(int id, decimal pricePerDay,
        double kmDriven, string model, decimal kmPrice, decimal numberOfKm,
            string regisrationNumber, string name, DateTime startToRent) 
            : base(id,model,name,pricePerDay,
        kmDriven, regisrationNumber, startToRent)
        {
            KmPrice = kmPrice;
            NumberOfKm = numberOfKm;
        }

        public override decimal CalculateTotalPrice(int rentedDays)
        {
            decimal newPricePerDay = decimal.Multiply(PricePerDay, 1.7M);
            return newPricePerDay * rentedDays + (KmPrice * NumberOfKm * 1.5M);
        }

    }
}

