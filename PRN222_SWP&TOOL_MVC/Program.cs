using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IRoleRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IUserRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.RoleRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.UserRepository;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.UnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.IServices.IAuthentication;
using PRN222_SWP_TOOL_MVC.Service.Services.GoogleAuthService;

namespace PRN222_SWP_TOOL_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //DI DB 
            builder.Services.AddDbContext<AppDbContext>(options =>
             options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                // Chỉnh cái này thành Cookie để nó ưu tiên nhảy về LoginPath của bạn trước
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
     .AddCookie(options =>
     {
         options.LoginPath = "/Account/Login";
         options.AccessDeniedPath = "/Account/AccessDenied";
     })
     .AddGoogle(options =>
     {
         options.ClientId = builder.Configuration["Authentication:Google:ClientId"];
         options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
     });

            // DI Repo
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //DI Service
            builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
