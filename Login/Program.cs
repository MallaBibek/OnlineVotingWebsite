using Login.Data;
using Login.TokenServices;
using Microsoft.ApplicationInsights.DependencyCollector;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Login
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddMvc();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddTransient<ITokenService, TokenService>();



            // Add services to the container.
            builder.Services.AddControllersWithViews();

            if (builder.Configuration["recaptchaFlag"] == "y")
            { 
            
            
            }
            if (builder.Configuration["ApplicationInsights:EnableApplicationInsight"] == "y")
            {
                builder.Services.AddApplicationInsightsTelemetry();
                builder.Services.ConfigureTelemetryModule<DependencyTrackingTelemetryModule>((module, o) =>
                {
                    module.EnableSqlCommandTextInstrumentation = true;
                });
            }



            builder.Services.AddDbContext<LoginContext>(options => options.UseSqlServer(

                builder.Configuration.GetConnectionString("DefaultConnection")
                ));
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });





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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}