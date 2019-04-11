using BiluthyrningAB2.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB2.Models.ViewModels
{
    public class BookingsVM
    {
        public List<Bookings> ListOfUserBookings { get; set; }

        public DateTime ReturnedTime { get; set; }

        public decimal Price { get; set; }

    }
}
