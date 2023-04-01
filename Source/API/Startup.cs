using API.Authorization;
using API.Configuration;
using API.Middlewares;
using Application;
using Application.Common.Interfaces;
using Domain;
using Domain.Mappings;
using Infrastructure;
using Infrastructure.Persistance;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var configuration = API.Configuration.ConfigurationProvider.GetConfig(Configuration);
            var appConfig = new AppConfig(configuration);
            services.AddSingleton(appConfig);

            services.AddScoped<IClaimsHandler, ClaimsHandler>();
            services.AddScoped<IRepository<Door>, Repository<Door>>();
            services.AddScoped<IRepository<User>, Repository<User>>();
            services.AddScoped<IRepository<RoleDoorMapping>, Repository<RoleDoorMapping>>();
            services.AddApplicationServices();

            services.AddApiVersioning(config =>
            {
                config.ApiVersionReader = new QueryStringApiVersionReader("version");
                config.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));

            services.AddDbContext<LockDbContext>(options =>
                options.UseSqlServer(appConfig.DbConnectionString));

            services.AddMediatR((config) =>
            {
                config.RegisterServicesFromAssemblyContaining(typeof(Startup));
            });
            services.AddJwtAuthentication();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.OperationFilter<ApiOperationFilter>();
            });
            services.AddHealthChecks()
               .AddCheck("ping",
                            () => HealthCheckResult.Healthy(),
                            tags: new[] { "ready" });

            services.AddControllers(options =>
            {
                options.Filters.Add(new AuthorizeFilter());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<PermissionMiddleware>();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHealthChecks("/ping", new HealthCheckOptions
                {
                    Predicate = (check) => check.Tags.Contains("ready"),
                    ResponseWriter = (context, result) => context.Response.WriteAsync("pong")
                });
                endpoints.MapControllers();
            });
            loggerFactory.AddFile($@"{Directory.GetCurrentDirectory()}\logs\logs.txt");
        }
    }
}
