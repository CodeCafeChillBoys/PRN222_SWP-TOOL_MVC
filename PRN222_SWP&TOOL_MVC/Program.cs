using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IRoleRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IUserRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.RoleRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.UserRepository;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.UnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.IServices.IAuthentication;
using PRN222_SWP_TOOL_MVC.Service.Services.GoogleAuthService;
<<<<<<< HEAD
=======
using PRN222_SWP_TOOL_MVC.Service.Services.SemesterService;
using PRN222_SWP_TOOL_MVC.Service.Services.StudentGroupService;
using PRN222_SWP_TOOL_MVC.Service.Services.StudentService;
using PRN222_SWP_TOOL_MVC.Service.Services.TeacherService;
using PRN222_SWP_TOOL_MVC.Service.Services.TopicService;
using PRN222_SWP_TOOL_MVC.Service.IServices.IQnA;
using PRN222_SWP_TOOL_MVC.Service.Services.QnAService;
>>>>>>> 45af78c (fix class AppDBcontext)

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


            // DI Google
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


            // DI Authen before page student teacher
            builder.Services.AddControllersWithViews(options =>
            {
                // Tạo một chính sách yêu cầu người dùng phải đăng nhập
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();

                // Thêm vào Filter toàn cục
                options.Filters.Add(new AuthorizeFilter(policy));
            });


            // DI Repo
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            //DI Service
            builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
<<<<<<< HEAD
=======
            builder.Services.AddScoped<ISemesterService, SemesterService>();
            builder.Services.AddScoped<IStudentGroupService, StudentGroupService>();
            builder.Services.AddScoped<ITeacherService, TeacherService>();
            builder.Services.AddScoped<ITopicService, TopicService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IQnaService, QnaService>();
>>>>>>> 45af78c (fix class AppDBcontext)

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
<<<<<<< HEAD

=======
           
            // MVC Routes
>>>>>>> 45af78c (fix class AppDBcontext)
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // API Routes (Q&A: StudentQnaController, TeacherQnaController)
            app.MapControllers();

            app.Run();
        }
    }
}
