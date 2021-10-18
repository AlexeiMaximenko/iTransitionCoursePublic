using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using iTransitionCourse.Entity;
using iTransitionCourse.Service;
using System.Security.Claims;

namespace iTransitionCourse.Data
{
    public static class DatabaseInit
    {
        public static void Init(IServiceProvider serviceScope)
        {
            var userManager = serviceScope.GetService<UserManager<User>>();
            var user = new User
            {
                UserName = "User"
            };

            userManager.CreateAsync(user, "123qwe").GetAwaiter().GetResult();

            var user2 = new User
            {
                UserName = "123"
            };

            userManager.CreateAsync(user2, "123qwe").GetAwaiter().GetResult();
            GetAdmin(serviceScope).GetAwaiter().GetResult();
        }

        public static async System.Threading.Tasks.Task GetAdmin(IServiceProvider serviceScope)
        {
            var userManager = serviceScope.GetService<UserManager<User>>();
            var config = serviceScope.GetService<SettingService>();
            var admin = new User
            {
                UserName = config.AdminUserName,
            };
            userManager.CreateAsync(admin, config.AdminPassword).GetAwaiter().GetResult();
            await userManager.AddClaimAsync(admin, new Claim(ClaimTypes.Role, "Admin"));
        }
    }
}