using Microsoft.AspNetCore.Identity;
using Shopping.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shopping.Data;
using Shopping.Areas.Admin.Models;

namespace Shopping
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 添加 Session 服務
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // 配置 Cookie 認證
            builder.Services.AddAuthentication("ShoppingCookieAuth")
	            .AddCookie("ShoppingCookieAuth", options =>
	            {
		            options.LoginPath = "/admin/Login";
	            });
            builder.Services.AddAuthorization();

			builder.Services.AddDbContext<DBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DBContext") ?? throw new InvalidOperationException("Connection string 'DBContext' not found.")));
            // 加載自訂配置檔案 myconfig.json
            builder.Configuration.AddJsonFile("AdminMenu.json", optional: false, reloadOnChange: true);

            // 註冊配置類
            builder.Services.Configure<List<MemuModel>>(builder.Configuration.GetSection("Menus"));

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            //  PasswordHasher 
            builder.Services.AddScoped<IPasswordHasher<Member>, PasswordHasher<Member>>();

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
                name: "area",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

           

            app.Run();
        }
    }
}
