using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB2.Models.Cars
{
    class Van: Car
    {
        public decimal KmPrice { get; set; }
        public decimal NumberOfKm { get; set; }

        public Van(int id, decimal pricePerDay,
            int rentedDays, double kmDriven, string model, decimal kmPrice, decimal numberOfKm,
            string regisrationNumber, string name, DateTime startToRent)
            : base(id, model, name, pricePerDay, kmDriven, regisrationNumber, startToRent)
        {
            KmPrice = kmPrice;
            NumberOfKm = numberOfKm;
        }

        public override decimal CalculateTotalPrice(int rentedDays)
        {
            return (PricePerDay* rentedDays)*1.2M + (KmPrice * NumberOfKm);
        }
    }
}
