using iTransitionCourse.Data;
using iTransitionCourse.Entity;
using iTransitionCourse.Service;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace iTransitionCourse
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            string dbConnect = $"User ID={Configuration["PosgressSQL:User ID"]};" +
            $"Password={Configuration["PosgressSQL:Password"]};" +
            $"Host={Configuration["PosgressSQL:Host"]};" +
            $"Port={Configuration["PosgressSQL:Port"]};" +
            $"Database={Configuration["PosgressSQL:Database"]};" +
            $"Pooling={Configuration["PosgressSQL:Pooling"]};" +
            $"sslmode={Configuration["PosgressSQL:sslmode"]};" +
            $"Trust Server Certificate={Configuration["PosgressSQL:Trust Server Certificate"]};";
            System.Net.ServicePointManager.ServerCertificateValidationCallback +=
            (s, cert, chain, sslPolicyErrors) => true;

            services.AddScoped<SettingService>();
            services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(dbConnect));


            services.AddIdentity<User, UserRole>(config =>
           {
               config.Password.RequireDigit = false;
               config.Password.RequireLowercase = false;
               config.Password.RequireNonAlphanumeric = false;
               config.Password.RequireUppercase = false;
               config.Password.RequiredLength = 6;
           })
           .AddEntityFrameworkStores<AppDbContext>()
           .AddRoleManager<RoleManager<UserRole>>()
           .AddSignInManager<SignInManager<User>>()
           .AddUserManager<UserManager<User>>()
           .AddDefaultTokenProviders();

            services.AddTransient<UserManager<User>>();
            services.AddTransient<SignInManager<User>>();
            services.AddTransient<RoleManager<UserRole>>();
            services.AddTransient<AppDbContext>();

            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Login/";
                config.AccessDeniedPath = "/Login/AccessDenied";
            });
            services.AddAuthentication()
            .AddGoogle(googleOptions =>
            {
                googleOptions.ClientId = $"{Configuration["Google:AppId"]}";
                googleOptions.ClientSecret = $"{Configuration["Google:AppSecret"]}";
            })
            .AddMicrosoftAccount(opt => 
            { 
                opt.ClientId = $"{Configuration["Microsoft:AppId"]}";
                opt.ClientSecret = $"{Configuration["Microsoft:AppSecret"]}";
            });

            services.AddAuthorization(o => 
            {
                o.AddPolicy("Admin", builder => 
                {
                    builder.RequireClaim(ClaimTypes.Role, "Admin");
                });
            });
            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();

            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
