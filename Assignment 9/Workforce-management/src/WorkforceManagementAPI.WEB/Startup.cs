using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WorkforceManagementAPI.DAL;
using WebApi.Middleware;
using Microsoft.EntityFrameworkCore;
using WorkforceManagementAPI.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using WorkforceManagementAPI.DAL.Contracts.IdentityContracts;
using WorkforceManagementAPI.BLL.Services.IdentityServices;
using WorkforceManagementAPI.BLL.Contracts;
using WorkforceManagementAPI.BLL.Services;
using WorkforceManagementAPI.BLL.Service;

namespace WorkforceManagementAPI.WEB
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WorkforceManagementAPI.WEB", Version = "v1" });
            });

            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:Default"]));

            services.AddTransient<IdentityUserManager>();
            services.AddTransient<TeamService>();


            //EF Identity
            services.AddIdentityCore<User>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
            })

            //Injecting the services and DB in the DI containter
                   .AddRoles<IdentityRole>()
                   .AddEntityFrameworkStores<DatabaseContext>();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                policy.RequireRole("Admin"));

                options.AddPolicy("User", policy =>
                policy.RequireRole("User"));
            }
            );
                
            services.AddTransient<IIdentityUserManager, IdentityUserManager>();
            services.AddTransient<ITimeOffService, TimeOffService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            //app.UseIdentityServer();
          /*  DatabaseSeeder.Seed(app.ApplicationServices);
            app.UseIdentityServer();*/

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WorkforceManagementAPI.WEB v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseMiddleware<ErrorHandlerMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
