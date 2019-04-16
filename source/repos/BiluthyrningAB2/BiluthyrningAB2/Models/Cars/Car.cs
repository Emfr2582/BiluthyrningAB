using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB2.Models.Cars
{
   public abstract class Car
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public string Name { get; set; }
        public decimal PricePerDay { get; set; }
        public double KmDriven { get; set; }
        public string RegistartionNumber { get; set; }
        public DateTime SartToRent { get; private set; }
       


        public Car(int id, string model, string name, decimal pricePerDay, 
        double kmDriven, string registrationNumber, DateTime startToRent)
        {
            Id = id; 
            PricePerDay = pricePerDay;
            Name = name; 
            KmDriven = kmDriven;
            Model = model;
            RegistartionNumber = registrationNumber;
            SartToRent = startToRent;
        }

        public abstract decimal CalculateTotalPrice(int rentedDays);
        
    }
}
