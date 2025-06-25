using Medi_Connect.Application.Interfaces.IRepositories;
using Medi_Connect.Application.Interfaces.ISerives;
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
        var builder = WebApplication.CreateBuilder(args);

        DotNetEnv.Env.Load();
        builder.Configuration.AddEnvironmentVariables();

        // Add services to the container.
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


        builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        builder.Services.Configure<RazorPayOptions>(builder.Configuration.GetSection("RazorPayOptions"));



        builder.Services.AddDbContext<AppDbContext>((serviceProvider, optionsBuilder) =>
        {
            optionsBuilder.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly("Medi-Connect.Infrastructure"));
        });

        builder.Services.AddControllers().AddJsonOptions(x =>
    x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);
        builder.Services.AddControllers().AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });



        builder.Services.AddHttpContextAccessor();

        var jwtKey = builder.Configuration["Jwt:Key"];
        if (string.IsNullOrEmpty(jwtKey))
            throw new Exception("JWT key not found in environment variables");

        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = builder.Configuration["Jwt:Issuer"],
                    ValidAudience = builder.Configuration["Jwt:Audience"],
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                };
            });


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
            new string[] { }
        }
    });
        });


        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowSpecificOrigin", policy =>
            {
                policy
                    .WithOrigins("http://localhost:3000", "https://sanoof-medi-connect.vercel.app")
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        //builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseMiddleware<GetUserIdMiddleWare>();
        app.UseCors("AllowSpecificOrigin");
        app.UseAuthorization();
        app.MapControllers();

        app.Run();
    }
}