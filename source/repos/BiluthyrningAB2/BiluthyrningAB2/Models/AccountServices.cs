using BiluthyrningAB2.Models.Entities;
using BiluthyrningAB2.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace BiluthyrningAB2.Models
{
    public class AccountServices
    {
        BiluthyrningABContext context;
        UserManager<MyIdentityUser> userManager;
        SignInManager<MyIdentityUser> signInManager;
        RoleManager<IdentityRole> roleManager;

        public AccountServices(BiluthyrningABContext context, UserManager<MyIdentityUser> userManager,
            SignInManager<MyIdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public async Task<bool> RegisterUser(RegisterVM vm)
        {
            var newUser = new MyIdentityUser
            {
                UserName = vm.UserName,
                Firstname = vm.FirstName,
                Lastname = vm.LastName,
                Ssn = vm.SSN,
                Email = vm.Email,
            };
            var result = await userManager.CreateAsync(
              newUser,vm.Password);
            var user = await userManager.FindByNameAsync(vm.UserName);
            var role = await roleManager.CreateAsync(new IdentityRole {Name = "Customer" });
            var roleResult = await userManager.AddToRoleAsync(user, "Customer");
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
