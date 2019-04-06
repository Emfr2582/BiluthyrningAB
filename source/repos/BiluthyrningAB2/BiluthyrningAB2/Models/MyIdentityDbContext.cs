using BiluthyrningAB2.Models.Entities;
using BiluthyrningAB2.Models.ViewModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB2.Models
{
    public class MyIdentityDbContext: IdentityDbContext<MyIdentityUser>
    {
        public MyIdentityDbContext(DbContextOptions<MyIdentityDbContext> options)
            :base(options)
        {
            var result = Database.EnsureCreated(); 
        }
    }
}
