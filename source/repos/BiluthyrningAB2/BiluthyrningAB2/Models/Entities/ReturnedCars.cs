using System;
using System.Collections.Generic;

namespace BiluthyrningAB2.Models.Entities
{
    public partial class ReturnedCars
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string BookingNr { get; set; }
        public string RegNr { get; set; }
        public int KmDriven { get; set; }
        public DateTime ReturnedTime { get; set; }
        public decimal Price { get; set; }

        public virtual Bookings BookingNrNavigation { get; set; }
        public virtual Bookings RegNrNavigation { get; set; }
        public virtual AspNetUsers User { get; set; }
    }
}
