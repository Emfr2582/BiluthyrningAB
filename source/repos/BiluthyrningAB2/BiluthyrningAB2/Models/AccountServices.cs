using BiluthyrningAB2.Models.Entities;
using BiluthyrningAB2.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BiluthyrningAB2.Models
{
    public class AccountServices
    {
        BiluthyrningABContext context;
        UserManager<MyIdentityUser> userManager;
        SignInManager<MyIdentityUser> signInManager;

        public AccountServices(BiluthyrningABContext context, UserManager<MyIdentityUser> userManager,
            SignInManager<MyIdentityUser> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
        }

        public async Task<bool> RegisterUser(RegisterVM vm)
        {
            var result = await userManager.CreateAsync(
                new MyIdentityUser
                {
                    UserName = vm.UserName,
                    Firstname = vm.FirstName,
                    Lastname = vm.LastName,
                    Ssn = vm.SSN,
                    Email = vm.Email,
                },
                vm.Password);
            return result.Succeeded;
        }

        public async Task<bool> SignInUser(LogInVM vM)
        {

            var result = await signInManager.PasswordSignInAsync(
                vM.UserName, vM.Password, false, false);
            return result.Succeeded;
        }

        public async Task LogOutUser()
        {
            await signInManager.SignOutAsync();
        }


    }
}
