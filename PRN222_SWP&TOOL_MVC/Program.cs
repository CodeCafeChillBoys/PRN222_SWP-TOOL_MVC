using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IClassRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGroupMemberRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IGroupRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IQuestionRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IRoleRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.ISemesterRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IStudentRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.ITeacherRepository;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.ITopRepositroy;
using PRN222_SWP_TOOL_MVC.Repository.IRepositories.IUserRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.ClassRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.GroupMemberRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.GroupRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.QuestionRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.RoleRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.SemesterRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.StudentRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.TeacherRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.TopicRepository;
using PRN222_SWP_TOOL_MVC.Repository.Repositories.UserRepository;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.IUnitOfWork;
using PRN222_SWP_TOOL_MVC.Repository.UnitOfWorkRepo.UnitOfWork;
using PRN222_SWP_TOOL_MVC.Service.IServices.IAuthentication;
using PRN222_SWP_TOOL_MVC.Service.IServices.ISemester;
using PRN222_SWP_TOOL_MVC.Service.IServices.IStudent;
using PRN222_SWP_TOOL_MVC.Service.IServices.IStudentGroup;
using PRN222_SWP_TOOL_MVC.Service.IServices.ITeacher;
using PRN222_SWP_TOOL_MVC.Service.IServices.ITopic;
using PRN222_SWP_TOOL_MVC.Service.IServices.IQnA;
using PRN222_SWP_TOOL_MVC.Service.Services.GoogleAuthService;
using PRN222_SWP_TOOL_MVC.Service.Services.SemesterService;
using PRN222_SWP_TOOL_MVC.Service.Services.StudentGroupService;
using PRN222_SWP_TOOL_MVC.Service.Services.StudentService;
using PRN222_SWP_TOOL_MVC.Service.Services.TeacherService;
using PRN222_SWP_TOOL_MVC.Service.Services.TopicService;
using PRN222_SWP_TOOL_MVC.Service.Services.QnAService;

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

            // DI DB
            builder.Services.AddDbContext<AppDbContext>(options =>
             options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

            // DI Google Auth
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
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

            // DI Authorization — yêu cầu đăng nhập toàn cục
            builder.Services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                                 .RequireAuthenticatedUser()
                                 .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            });

            // DI Repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
            builder.Services.AddScoped<IStudentRepository, StudentRepository>();
            builder.Services.AddScoped<ITeacherRepository, TeacherRepository>();
            builder.Services.AddScoped<ISemesterRepository, SemesterRepository>();
            builder.Services.AddScoped<IClassRepository, ClassRepository>();
            builder.Services.AddScoped<IGroupMemberRepository, GroupMemberRepository>();
            builder.Services.AddScoped<IStudentGroupRepository, StudentGroupRepository>();
            builder.Services.AddScoped<ITopicRepository, TopicRepository>();
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            // DI Services
            builder.Services.AddScoped<IGoogleAuthService, GoogleAuthService>();
            builder.Services.AddScoped<ISemesterService, SemesterService>();
            builder.Services.AddScoped<IStudentGroupService, StudentGroupService>();
            builder.Services.AddScoped<ITeacherService, TeacherService>();
            builder.Services.AddScoped<ITopicService, TopicService>();
            builder.Services.AddScoped<IStudentService, StudentService>();
            builder.Services.AddScoped<IQnaService, QnaService>();

            var app = builder.Build();

            // ── Seed Roles nếu chưa có ─────────────────────────────────────
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                db.Database.EnsureCreated();
                if (!db.Roles.Any())
                {
                    db.Roles.AddRange(
                        new PRN222_SWP_TOOL_MVC.Repository.Entities.Role { RoleID = 1, RoleCode = "ADMIN",   RoleName = "Admin",   Description = "Administrator" },
                        new PRN222_SWP_TOOL_MVC.Repository.Entities.Role { RoleID = 2, RoleCode = "STUDENT", RoleName = "Student", Description = "Student role" },
                        new PRN222_SWP_TOOL_MVC.Repository.Entities.Role { RoleID = 3, RoleCode = "TEACHER", RoleName = "Teacher", Description = "Teacher role" }
                    );
                    db.SaveChanges();
                }
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            // MVC Routes
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // API Routes (Q&A: StudentQnaController, TeacherQnaController)
            app.MapControllers();

            app.Run();
        }
    }
}
