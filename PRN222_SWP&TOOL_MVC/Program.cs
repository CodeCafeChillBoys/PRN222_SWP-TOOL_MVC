using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IClassRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IQuestionRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IRoleRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.ISemesterRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IStudentRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.ITeacherRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.ITopicRegistrationsRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.ITopRepositroy;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IUserRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.ClassRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.QuestionRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.RoleRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.SemesterRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.StudentRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.TeacherRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.TopicRegistrationsRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.TopicRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.UserRepository;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.UnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.IServices.IAuthentication;
using PRN222_SWP_TOOL_MVC.Service.IServices.ISemester;
using PRN222_SWP_TOOL_MVC.Service.IServices.ITeacher;
using PRN222_SWP_TOOL_MVC.Service.IServices.ITopic;
using PRN222_SWP_TOOL_MVC.Service.Services.GoogleAuthService;
using PRN222_SWP_TOOL_MVC.Service.Services.SemesterService;
using PRN222_SWP_TOOL_MVC.Service.Services.TeacherService;
using PRN222_SWP_TOOL_MVC.Service.Services.TopicService;

namespace PRN222_SWP_TOOL_MVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            // DI session
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession();

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
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
            builder.Services.AddScoped<IClassRepository, ClassRepository>();
            builder.Services.AddScoped<ITopicRepository, TopicRepository>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
            builder.Services.AddScoped<ITopicRegistrationsRepository, TopicRegistrationsRepository>();
            //DI Service
            builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            builder.Services.AddScoped<ISemesterService, SemesterService>();

            builder.Services.AddScoped<ITeacherService, TeacherService>();
            builder.Services.AddScoped<ITopicService, TopicService>();

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

            app.UseSession();


            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
