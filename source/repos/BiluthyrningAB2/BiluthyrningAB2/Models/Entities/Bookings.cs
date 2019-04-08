using System;
using System.Collections.Generic;

namespace BiluthyrningAB2.Models.Entities
{
    public partial class Bookings
    {
        public Bookings()
        {
            ReturnedCarsBookingNrNavigation = new HashSet<ReturnedCars>();
            ReturnedCarsRegNrNavigation = new HashSet<ReturnedCars>();
        }

        public int Id { get; set; }
        public string UserId { get; set; }
        public string BookingNr { get; set; }
        public string Model { get; set; }
        public string RegNr { get; set; }
        public DateTime BookingTime { get; set; }
        public int Km { get; set; }

        public virtual AspNetUsers User { get; set; }
        public virtual ICollection<ReturnedCars> ReturnedCarsBookingNrNavigation { get; set; }
        public virtual ICollection<ReturnedCars> ReturnedCarsRegNrNavigation { get; set; }
    }
}
