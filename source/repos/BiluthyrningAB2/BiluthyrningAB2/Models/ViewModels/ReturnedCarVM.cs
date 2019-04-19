using BiluthyrningAB2.Models.Cars;
using BiluthyrningAB2.Models.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB2.Models.ViewModels
{
    public class ReturnedCarVM
    {
        public int KmDriven { get; set; }

        public decimal Price { get; set; }

        public int Id { get; set; }

        public List<SelectListItem> _cars { get; set; }

        public Car Car { get; set; }

    }
}
