using GymMangementDAL.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymMangementDAL.Data.DataSeed
{
    public class IdentityDbContextSeeding
    {
        public static bool SeedData(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            try
            {
                var HasUsers = userManager.Users.Any();
                var HasRoles = roleManager.Roles.Any();
                if (HasUsers && HasRoles) return false;

                if (!HasRoles)
                {
                    var Roles = new List<IdentityRole>() 
                    { 
                        new() {Name="SuperAdmin"},
                        new() {Name="Admin"},
                    };

                    foreach (var role in Roles)
                    {
                        if (!roleManager.RoleExistsAsync(role.Name!).Result)
                        {
                            roleManager.CreateAsync(role).Wait();
                        }
                    }
                }

                if (!HasUsers)
                {
                    var mainAdmin = new ApplicationUser()
                    {
                        FirstName = "Bassant",
                        LastName = "Wael",
                        Email = "bassant@gmail.com",
                        UserName = "Bassant_Wael",
                        PhoneNumber = "01293508378"
                    };
                    userManager.CreateAsync(mainAdmin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(mainAdmin, "SuperAdmin").Wait();

                    var Admin = new ApplicationUser()
                    {
                        FirstName = "Ahmed",
                        LastName = "Hassan",
                        Email = "ahmed@gmail.com",
                        UserName = "ahmed_hassan",
                        PhoneNumber = "01293508375"
                    };
                    userManager.CreateAsync(Admin, "P@ssw0rd").Wait();
                    userManager.AddToRoleAsync(Admin, "Admin").Wait();
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Faild To Seed Data :{ex}");
                return false;
            }
        }
    }
}
