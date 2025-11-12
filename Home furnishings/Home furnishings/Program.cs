using Home_furnishings.Models;
using Home_furnishings.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Home_furnishings
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //   MVC
            builder.Services.AddControllersWithViews();

            //Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole<int>>(options =>
            {
                options.Password.RequiredLength = 4;         // طول الباسورد
                options.Password.RequireDigit = false;        // لازم رقم
                options.Password.RequireLowercase = false;    // حرف صغير
                options.Password.RequireUppercase = false;    // حرف كبير
                options.Password.RequireNonAlphanumeric = false; // رموز خاصة مش مطلوبة
            })
                .AddEntityFrameworkStores<Context>()
                .AddDefaultTokenProviders();
            //DB
            builder.Services.AddDbContext<Context>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("CS"))
            );

            // Register Repositories
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<ICartRepository, CartRepository>();

            //cookie
          
            builder.Services.ConfigureApplicationCookie(options =>
            {
                // اسم الكوكي
                options.Cookie.Name = ".AspNetCore.Identity.Application";

                // مدة صلاحية الكوكي (مثلاً يوم واحد)
                options.ExpireTimeSpan = TimeSpan.FromDays(1);

                // لو المستخدم اختار "تذكرني"
                options.SlidingExpiration = true;

                // اجبار HTTPS (لو المشروع يعمل على HTTPS)
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;

                // الكوكي مش قابلة للقراءة من جافاسكريبت
                options.Cookie.HttpOnly = true;

                // مسار تسجيل الدخول
                options.LoginPath = "/Account/Login";
            });


            // Add Session
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });



            var app = builder.Build();

           
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}"
            );

            app.Run();
        }

    }

}
