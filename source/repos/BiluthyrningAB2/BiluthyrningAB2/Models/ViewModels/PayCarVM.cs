using BiluthyrningAB2.Models.Cars;
using BiluthyrningAB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB2.Models.ViewModels
{
    public class PayCarVM
    {
        public decimal Price { get; set; }
        public int KmDriven { get; set; }
        public Bookings Bookings { get; set; }

        public string BookingId { get; set; }

        public string RegNr { get; set; }
        public Car Car { get; set; }
        public double Days { get; set; }
        public DateTime ReturnedDate { get; set; }



    }
}
