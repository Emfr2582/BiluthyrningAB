using System;
using BiluthyrningAB2.Models.Cars;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BiluthyrningAB2.Models.ViewModels
{
    public class SelectedCarVM
    {
        public List<Car> _cars { get; set; }
        public int Id { get; set; }
    }
}
