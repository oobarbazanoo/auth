using API;
using Infrastructure.Data.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Tests.Integration.Base
{
    public class TestStartup : Startup
    {
        public TestStartup(IWebHostEnvironment env) : base(env) { }

        public override void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UsePathBase("/Auth");
            base.Configure(app, env);
        }

        protected override void AddDbContext(IServiceCollection services)
        {
            services.AddDbContext<AuthDbContext, TestAuthDbContext>();
        }
    }
}