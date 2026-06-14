using FluentValidation;
using FluentValidation.AspNetCore;
using HospitalManagementSystem.Data.Data;
using HospitalManagementSystem.Models.Middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace HospitalManagementSystems.API
{
    /// <summary>
    /// Startup class used by the AWS Lambda entry point.
    /// Mirrors the configuration in Program.cs so that the app behaves
    /// identically whether it runs as a standard web host or inside Lambda.
    /// </summary>
    public class LambdaStartup
    {
        public IConfiguration Configuration { get; }

        public LambdaStartup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // ----------------------------------------------------------------
        // ConfigureServices — register DI services
        // ----------------------------------------------------------------
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // Entity Framework / SQL Server
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("HMSDbContext")));

            // JWT Authentication
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(option =>
                {
                    option.RequireHttpsMetadata = false;
                    option.SaveToken = true;
                    option.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer   = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidIssuer      = Configuration["Jwt:Issuer"],
                        ValidAudience    = Configuration["Jwt:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(Configuration["Jwt:Key"] ?? Configuration["Jwt:key"]!))
                    };
                });

            // Swagger — useful for testing even on Lambda via API Gateway
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title   = "HospitalManagementSystems.API",
                    Version = "v1"
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name        = "Authorization",
                    Type        = SecuritySchemeType.Http,
                    Scheme      = "bearer",
                    BearerFormat= "JWT",
                    In          = ParameterLocation.Header,
                    Description = "Enter 'Bearer {token}'"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id   = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddHospitalManagementServices(Configuration);

            // FluentValidation
            services.AddFluentValidationAutoValidation();
            services.AddFluentValidationClientsideAdapters();
            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly())
                .AddFluentValidation(v =>
                {
                    v.ImplicitlyValidateChildProperties        = true;
                    v.ImplicitlyValidateRootCollectionElements = true;
                    v.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly());
                });

            // CORS
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                    policy.WithOrigins("https://localhost:7082", "http://localhost:4200")
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });
        }

        // ----------------------------------------------------------------
        // Configure — build the HTTP pipeline
        // ----------------------------------------------------------------
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, AppDbContext db)
        {
            // Auto-migrate on startup
            db.Database.Migrate();

            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<GlobalErrorHandlingMiddleware>();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
