using be.Data.Seeder;
using hotel_booking_api.Extencions;
using hotel_booking_api.Extensions;
using hotel_booking_api.Polices;
using hotel_booking_data.Contexts;
using hotel_booking_models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Unicode;
using thda;
using thda.Utilities.AutoMapper;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<HotelContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("db"), sqlOptions => sqlOptions.EnableRetryOnFailure()
    ).LogTo(Console.WriteLine, LogLevel.Information));

builder.Services.AddControllersWithViews()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
    });

// Cấu hình dịch vụ gửi Mail
builder.Services.ConfigureMailService(builder.Configuration);

// Cấu hình Identity
builder.Services.ConfigureIdentity();

// Add Jwt Authentication and Authorization
builder.Services.ConfigureAuthentication(builder.Configuration);


// Adds our Authorization Policies to the Dependecy Injection Container
builder.Services.AddPolicyAuthorization();

builder.Services.AddAutoMapper(typeof(AutoMapperProfile).Assembly);

//Cấu hình Cloudinary
builder.Services.AddCloudinary(CloudinaryServiceExtension.GetAccount(builder.Configuration));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3001", "https://adminhotel24.azurewebsites.net")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddControllers().AddNewtonsoftJson(op => op.SerializerSettings.ReferenceLoopHandling
            = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Đăng ký dịch vụ mở rộng Dependency Injection
builder.Services.AddDependencyInjection();



builder.Logging.ClearProviders();
builder.Logging.AddConsole();

var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseCors("AllowSpecificOrigin");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Hotel Management Api v1"));
}

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<HotelContext>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<AppUser>>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    await HotelSeeder.SeedData(context, userManager, roleManager);
}

app.UseHttpsRedirection();

var logger = app.Services.GetRequiredService<ILogger<Program>>();



app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
