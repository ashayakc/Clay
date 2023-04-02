using API.Authorization;
using API.Configuration;
using API.Middlewares;
using Application.Commands.Authenticate;
using Application.Common.Behaviours;
using Application.Common.Interfaces;
using Domain;
using Domain.Mappings;
using FluentValidation;
using Infrastructure;
using Infrastructure.Persistance;
using Infrastructure.Services;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using Nest;

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
            services.AddScoped<IGenericRepository<Door>, Repository<Door>>();
            services.AddScoped<IGenericRepository<User>, Repository<User>>();
            services.AddScoped<IGenericRepository<RoleDoorMapping>, Repository<RoleDoorMapping>>();
            services.AddScoped<IAuditService, AuditService>();

            var settings = new ConnectionSettings(new Uri(appConfig.ElasticUrl))
                .BasicAuthentication(appConfig.ElasticUserName, appConfig.ElasticPassword)
                            .DefaultIndex(appConfig.IndexName);
            var client = new ElasticClient(settings);
            services.AddSingleton<IElasticClient>(provider => client);

            services.AddApiVersioning(config =>
            {
                config.ApiVersionReader = new QueryStringApiVersionReader("version");
                config.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddValidatorsFromAssembly(AppDomain.CurrentDomain.GetAssemblies().SingleOrDefault(assembly => assembly.GetName().Name == "Application"));
            services.AddAutoMapper(x => x.AddProfile(new MappingProfile()));

            services.AddDbContext<LockDbContext>(options =>
                options.UseSqlServer(appConfig.DbConnectionString));

            services.AddMediatR((config) =>
            {
                config.RegisterServicesFromAssemblyContaining(typeof(Startup));
                config.RegisterServicesFromAssemblyContaining(typeof(AuthenticateCommandValidator));
                config.AddBehavior(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            });
            services.AddJwtAuthentication();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.EnableAnnotations();
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Authorization header using the Bearer scheme. \r\n\r\n 
                      Enter 'Bearer' [space] and then your token in the text input below.
                      \r\n\r\nExample: 'Bearer 12345abcdef'",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type=ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
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
            app.UseSwagger();
            app.UseSwaggerUI();

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
