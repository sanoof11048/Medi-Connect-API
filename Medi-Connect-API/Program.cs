using DotNetEnv;
using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Application.Interfaces.ISerives;
using Medi_Connect.Application.Mapper;
using Medi_Connect.Application.Services;
using Medi_Connect.Domain.Common;
using Medi_Connect.Infrastructure.Context;
using Medi_Connect.Infrastructure.Repositories;
using Medi_Connect.Infrastructure.Services;
using Medi_Connect_API.middleware;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.Text.Json.Serialization;

public class Program
{
    public static void Main(string[] args)
    {
        if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
        {
            Env.Load();
        }
        var builder = WebApplication.CreateBuilder(args);

        builder.Configuration.AddEnvironmentVariables();

        // Listen on port 8080 for Render
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(8080);
        });

        builder.Services.AddAutoMapper(typeof(MappingProfile));


        // ---- Service Registrations ----
        builder.Services.AddScoped<IUserService, UserServices>();
        builder.Services.AddScoped<IAuthService, AuthService>();
        builder.Services.AddScoped<IAdminService, AdminService>();
        builder.Services.AddScoped<IEmailService, EmailService>();
        builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
        builder.Services.AddScoped<INurseService, NurseService>();
        builder.Services.AddScoped<IPatientService, PatientService>();
        builder.Services.AddScoped<IRelativeService, RelativeService>();
        builder.Services.AddScoped<IVitalService, VitalService>();
        builder.Services.AddScoped<IFoodLogService, FoodLogService>();
        builder.Services.AddScoped<IMedicationLogService, MedicationLogService>();
        builder.Services.AddScoped<IReportService, ReportService>();
        builder.Services.AddScoped<INurseAssignmentService, NurseAssignmentService>();
        builder.Services.AddScoped<ICareTypeRateService, CareTypeRateService>();
        builder.Services.AddScoped<IRazorpayService, RazorpayService>();
        builder.Services.AddScoped<IPaymentService, PaymentService>();

        builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<IAdminRepository, AdminRepository>();
        builder.Services.AddScoped<INurseRepository, NurseRepository>();
        builder.Services.AddScoped<IPatientRepository, PatientRepository>();
        builder.Services.AddScoped<INurseRequestRepository, NurseRequestRepository>();
        builder.Services.AddScoped<ICareTypeRateRepository, CareTypeRateRepository>();
        builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

        builder.Services.Configure<RazorPayOptions>(options =>
        {
            options.Key = Environment.GetEnvironmentVariable("RazorPayOptions__Key");
            options.Secret = Environment.GetEnvironmentVariable("RazorPayOptions__Secret");
        });

        // ---- Database Configuration ----
        var connectionString = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("Database connection string is missing.");

        builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(connectionString, sqlOptions =>
        {
            sqlOptions.MigrationsAssembly("Medi-Connect.Infrastructure");
            sqlOptions.EnableRetryOnFailure();
        })
        );




        // ---- JSON Options ----
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        builder.Services.AddHttpContextAccessor();

        // ---- JWT Authentication ----
        var jwtKey = Environment.GetEnvironmentVariable("Jwt__Key");
        var jwtIssuer = Environment.GetEnvironmentVariable("Jwt__Issuer");
        var jwtAudience = Environment.GetEnvironmentVariable("Jwt__Audience");

        if (string.IsNullOrWhiteSpace(jwtKey))
            throw new Exception("JWT Key is not configured");

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = jwtIssuer,
                    ValidAudience = jwtAudience,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });

        // ---- Swagger Setup ----
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo { Title = "MediConnect API", Version = "v1" });

            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Enter JWT token in format: Bearer {token}"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        // ---- CORS ----

        var origins = Environment.GetEnvironmentVariable("FRONTEND_URL")?.Split(',') ?? Array.Empty<string>();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins(origins)
                      .AllowAnyHeader()
                      .AllowAnyMethod()
                      .AllowCredentials();
            });
        });
        builder.Services.AddEndpointsApiExplorer();


        // ---- Application Pipeline ----
        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            db.Database.Migrate();
        }

            app.UseSwagger();
            app.UseSwaggerUI();

        if (app.Environment.IsDevelopment())
        {
            app.UseHttpsRedirection(); // Local dev only
        }

        app.UseRouting();
        app.UseCors("AllowFrontend");
        app.UseMiddleware<GetUserIdMiddleWare>();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}
