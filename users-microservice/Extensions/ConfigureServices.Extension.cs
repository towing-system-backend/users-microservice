using Application.Core;
using MassTransit;
using Microsoft.IdentityModel.Tokens;
using User.Domain;
using User.Infrastructure;
using System.Text;
using System.Security.Claims;

namespace User.Extensions
{
    public static class ConfigureServicesExtension
    {
        public static void ConfigureServices(this IServiceCollection services)
        {
            services.AddScoped<IdService<string>, GuidGenerator>();
            services.AddScoped<Logger, DotNetLogger>();
            services.AddScoped<IMessageBrokerService, RabbitMQService>();
            services.AddScoped<UserController>();
            services.AddSingleton<IEventStore, MongoEventStore>();
            services.AddSingleton<IUserRepository, MongoUserRepository>();
            services.AddSingleton<IPerformanceLogsRepository, MongoPerformanceLogsRespository>();
        }

        public static void ConfigureAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Jwt:Issuer"],
                        ValidateAudience = true,
                        ValidAudience = configuration["Jwt:Audience"],
                        ValidateLifetime = true,
                        RoleClaimType = ClaimTypes.Role,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("JWT_SECRET")!))
                    };
                });
        }

        public static void ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMassTransit(busConfigurator =>
            {
                busConfigurator.AddConsumer<CreateUserConsumer>();
                busConfigurator.SetKebabCaseEndpointNameFormatter();
                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(new Uri(Environment.GetEnvironmentVariable("RABBITMQ_URI")!), h =>
                    {
                        h.Username(Environment.GetEnvironmentVariable("RABBITMQ_USERNAME")!);
                        h.Password(Environment.GetEnvironmentVariable("RABBITMQ_PASSWORD")!);
                    });

                    configurator.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));
                    configurator.ConfigureEndpoints(context);
                });
            });
        }

        public static void AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new() { Title = "User API", Version = "v1" });
            });
        }

        public static void UseSwagger(this IApplicationBuilder app)
        {
            app.UseSwagger(c =>
            {
                c.SerializeAsV2 = true;
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "User v1");
                c.RoutePrefix = string.Empty;
            });
        }
    }
}
