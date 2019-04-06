using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB2.Models
{
    public class Days
    {
        public int Value { get; set; }
        public string Day{ get; set; }
        public bool IsChecked { get; set; }
    }

    public class ListOfDays
    {
        public List<Days> Booked { get; set;}
    }
}
