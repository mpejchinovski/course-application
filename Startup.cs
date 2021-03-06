using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CourseApp.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace CourseApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
            .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.WriteIndented = true;
                });

            services.AddControllersWithViews();

            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var connectionUrl = Environment.GetEnvironmentVariable("CLEARDB_DATABASE_URL").Replace("mysql://", string.Empty);
            var username = connectionUrl.Split(':')[0];
            var password = connectionUrl.Split(':')[1].Split('@')[0];
            var host = connectionUrl.Split('@')[1].Split('/')[0];
            var database = connectionUrl.Split('@')[1].Split('/')[1].Split('?')[0];

            string connectionString = "server=" + host + ";database=" + database + ";user=" + username + ";password=" + password;
            services.AddDbContextPool<CourseAppDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
            services.AddSwaggerGen();
            services.AddCors(o => o.AddPolicy("policy", builder =>
            {
                builder.AllowAnyOrigin()
                       .AllowAnyMethod()
                       .AllowAnyHeader();
            }));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Applications}/{action=Create}");
            });

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
        }
    }
}
