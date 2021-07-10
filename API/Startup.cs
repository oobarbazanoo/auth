using System.IO;
using Application.Mappings;
using Application.Service.Implementation;
using Application.Service.Interface;
using Domain.Service.Implementation;
using Domain.Service.Interface;
using Infrastructure.Data.Configuration;
using Infrastructure.Data.Repositories;
using Infrastructure.Service.Implementation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace API
{
    public class Startup
    {
        readonly IConfiguration Configuration;
        readonly string CorsPolicyName = "corsPolicyName";

        public Startup(IWebHostEnvironment env)
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", false, true)
                .AddEnvironmentVariables()
                .Build();
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //api
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
            });
            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, builder =>
                {
                    builder
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .SetIsOriginAllowed(host => true);
                });
            });

            //infrastructure
            services.AddScoped<ITokenManager, TokenManager>();
            services.AddScoped<IUserRepository, UserRepository>();
            AddDbContext(services);

            //application
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            Mappings.Register();
            services.AddScoped<IAuthService, AuthService>();

            //domain
            services.AddScoped<IUserService, UserService>();
        }

        protected virtual void AddDbContext(IServiceCollection services)
        {
            services.AddDbContext<AuthDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("Default"))
            );
        }

        public virtual void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1"));
            }

            app.UseCors(CorsPolicyName);

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}