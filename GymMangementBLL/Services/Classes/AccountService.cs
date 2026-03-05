using GymMangementBLL.Services.Interfaces;
using GymMangementBLL.ViewModels.AccountViewModels;
using GymMangementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementBLL.Services.Classes
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> userManager;

        public AccountService(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }
        public ApplicationUser? ValidateUser(LoginViewModel loginViewModel)
        {
            var user = userManager.FindByEmailAsync(loginViewModel.Email).Result;
            if (user is null) return null;

            var IsPasswordValid = userManager.CheckPasswordAsync(user, loginViewModel.Password).Result;
            return IsPasswordValid ? user : null;
        }
    }
}
