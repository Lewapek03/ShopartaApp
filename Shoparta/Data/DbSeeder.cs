using Microsoft.AspNetCore.Identity;
using Shoparta.Constans;

namespace Shoparta.Data
{
    public class DbSeeder
    {
        public static async Task SeedDefault(IServiceProvider service) 
        {
            var userMgr = service.GetService < UserManager<IdentityUser>>();
            var roleMgr = service.GetService < RoleManager<IdentityRole>>();

            
            await roleMgr.CreateAsync(new IdentityRole(Roles.User.ToString()));
            await roleMgr.CreateAsync(new IdentityRole(Roles.Admin.ToString()));


            //create admin
            var admin = new IdentityUser
            {
                UserName = "admin@shoparta.com",
                Email = "admin@shoparta.com",
                EmailConfirmed = true
            };

            var isUserExist =await userMgr.FindByEmailAsync(admin.Email);
            if (isUserExist == null)
            {
                await userMgr.CreateAsync(admin, "Admin123!");
                await userMgr.AddToRoleAsync(admin, Roles.Admin.ToString());
            }
        }
    }
}
