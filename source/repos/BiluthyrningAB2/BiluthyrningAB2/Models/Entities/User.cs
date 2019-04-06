using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace BiluthyrningAB2.Models.Entities
{
    public partial class User
    {
        public User()
        {
            Bookings = new HashSet<Bookings>();
        }

        public int Id { get; set; }
        public string Username { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public string Ssn { get; set; }
        public string Email { get; set; }

        public virtual ICollection<Bookings> Bookings { get; set; }
    }
}
