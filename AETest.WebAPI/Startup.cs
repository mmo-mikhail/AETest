using AETest.DataAccess;
using AETest.DataAccess.Repositories;
using AETest.WebAPI.HealthChecks;
using AETest.WebAPI.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Logging;

namespace AETest.WebAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            // Configure Health Checks
            services.AddHealthChecks()
                .AddCheck<MainHealthCheck>(
                    nameof(MainHealthCheck),
                    failureStatus: HealthStatus.Degraded,
                    tags: new[] { "initial" });

            // Configure DAL
            services.AddDbContext<IDbContext, DatabaseContext>(options =>
            {
                options.UseInMemoryDatabase("inmemorydb");

                // Uncomment following code to setup connection to real db:

                //options.UseSqlServer(Configuration["ConnectionString"],
                //    sqlServerOptionsAction: sqlOptions =>
                //    {
                //        sqlOptions.EnableRetryOnFailure(
                //            maxRetryCount: 1,
                //            maxRetryDelay: TimeSpan.FromSeconds(10),
                //            errorNumbersToAdd: null);
                //    });
            }, ServiceLifetime.Scoped);

            services.AddTransient(typeof(IEntityRepository<>), typeof(EntityRepository<>));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILogger<Startup> logger)
        {
            app.UseMiddleware<ErrorHttpMiddleware>();

            if (env.IsDevelopment())
            {
                logger.LogInformation("Starting Up In Development Environment");
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseCors(builder =>
                builder.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

            app.UseMvc();

            app.UseHealthChecks("/health", new HealthCheckOptions()
            {
                Predicate = (check) => check.Tags.Contains("initial"),
                AllowCachingResponses = false,
            });
        }
    }
}
