using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using WorkforceManagementAPI.DAL;
using WorkforceManagementAPI.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using WorkforceManagementAPI.WEB.IdentityAuth;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                        Reference = new OpenApiReference
                            {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
                });
            });

            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration["ConnectionStrings:Default"]));

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

            var builder = services.AddIdentityServer((options) =>
            {
                options.EmitStaticAudienceClaim = true;
            })

                //This is for dev only scenarios when you don�t have a certificate to use.
                .AddInMemoryApiScopes(IdentityConfig.ApiScopes)
                .AddInMemoryClients(IdentityConfig.Clients)
                .AddDeveloperSigningCredential()
                .AddResourceOwnerValidator<PasswordValidator>();

            // omitted for brevity
            // Authentication
            // Adds the asp.net auth services
            services
                .AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy =>
                policy.RequireRole("Admin"));

                options.AddPolicy("User", policy =>
                policy.RequireRole("User"));
            }
            )
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                })

                // Adds the JWT bearer token services that will authenticate each request based on the token in the Authorize header
                // and configures them to validate the token with the options

                .AddJwtBearer(options =>
                {
                    options.Authority = "http://localhost:5000";
                    options.Audience = "https://localhost:5001/resources";
                });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {

            /*DatabaseSeeder.Seed(app.ApplicationServices);

            app.UseIdentityServer();*/

            // The above code is applicable in future versions of the API where we have asp.net Identity user

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "WorkforceManagementAPI.WEB v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
