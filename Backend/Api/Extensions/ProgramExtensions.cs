using Api.Attributes;
using Api.Data;
using Api.Middleware;
using Api.Services;
using Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Api.Extensions
{
    public static class ProgramExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IRestaurantService, RestaurantService>();
            services.AddScoped<IRestaurantUserService, RestaurantUserService>();
            services.AddScoped<IStartupService, StartupService>();
            services.AddScoped<ISubdomainService, SubdomainService>();
            services.AddHttpContextAccessor();

            return services;
        }

        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Rifuud API",
                    Version = "v1",
                    Description = "API for Rifuud restaurant management system"
                });

                // Include XML comments
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }

                // Add JWT Bearer authentication to Swagger for both AdminUser and RestaurantUser
                c.AddSecurityDefinition("AdminUserBearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header for AdminUser using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityDefinition("RestaurantUserBearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header for RestaurantUser using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
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
                                Type = ReferenceType.SecurityScheme,
                                Id = "AdminUserBearer"
                            }
                        },
                        new string[] {}
                    },
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "RestaurantUserBearer"
                            }
                        },
                        new string[] {}
                    }
                });
            });

            return services;
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer("AdminUser", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            configuration["Authentication:Jwt:AdminUser:SecretKey"] ?? "tu_clave_secreta_muy_larga_y_segura_para_jwt_admin_2024")),
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Authentication:Jwt:AdminUser:Issuer"] ?? "RifuudApi",
                        ValidateAudience = true,
                        ValidAudience = configuration["Authentication:Jwt:AdminUser:Audience"] ?? "RifuudApiAdminUsers",
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                })
                .AddJwtBearer("RestaurantUser", options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                            configuration["Authentication:Jwt:RestaurantUser:SecretKey"] ?? "tu_clave_secreta_muy_larga_y_segura_para_jwt_restaurant_2024")),
                        ValidateIssuer = true,
                        ValidIssuer = configuration["Authentication:Jwt:RestaurantUser:Issuer"] ?? "RifuudApi",
                        ValidateAudience = true,
                        ValidAudience = configuration["Authentication:Jwt:RestaurantUser:Audience"] ?? "RifuudApiRestaurantUsers",
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    };
                });

            services.AddAuthorization();

            return services;
        }

        public static IServiceCollection AddControllersConfiguration(this IServiceCollection services)
        {
            services.AddControllers(options =>
            {
                options.Filters.Add<ModelStateValidationFilter>();
                // Use custom route prefix
                options.UseGeneralRoutePrefix("Api");
            })
            .ConfigureApiBehaviorOptions(options =>
            {
                // Configure global API behavior
            });

            // Configure global route prefix
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = false; // Keep URLs as defined (with capital letters)
            });

            services.AddOpenApi();

            return services;
        }

        public static IServiceCollection AddDatabaseConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<RifuudDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            return services;
        }

        public static IServiceCollection AddCorsConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? 
                new[] { "http://localhost:5173" };

            services.AddCors(options =>
            {
                options.AddPolicy("DefaultPolicy", policy =>
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                });
            });

            return services;
        }

        public static WebApplication ConfigureDevelopmentEnvironment(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Rifuud API v1");
                    c.DocumentTitle = "Rifuud API Documentation";
                    c.DefaultModelsExpandDepth(-1); // Hide models section by default
                    c.DisplayRequestDuration();
                });
                app.MapOpenApi();
            }

            return app;
        }

        public static WebApplication ConfigureProductionEnvironment(this WebApplication app)
        {
            if (app.Environment.IsProduction())
            {
                app.UseHttpsRedirection();
            }

            return app;
        }

        public static WebApplication ConfigureMiddlewarePipeline(this WebApplication app)
        {
            // Add CORS middleware
            app.UseCors("DefaultPolicy");

            // Add global exception handling middleware
            app.UseMiddleware<GlobalExceptionMiddleware>();

            // Add authentication and authorization middleware
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            return app;
        }

        public static async Task ExecuteStartupServicesAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            var startupService = scope.ServiceProvider.GetRequiredService<IStartupService>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
            
            try
            {
                await startupService.StartupServerConfiguration();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error durante el inicio del servidor");
                throw;
            }
        }
    }
}