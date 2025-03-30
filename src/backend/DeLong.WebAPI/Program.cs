using Serilog;
using System.Text;
using DeLong.Domain.Enums;
using DeLong.Domain.Entities;
using DeLong.WebAPI.Extentions;
using DeLong.WebAPI.Middlewares;
using DeLong.Service.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using DeLong.Infrastructure.Contexts;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using DeLong.Service.DTOs.Branchs;

var builder = WebApplication.CreateBuilder(args);

// ✅ JWT sozlamalarini yuklash va validatsiya
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

if (string.IsNullOrWhiteSpace(secretKey) || secretKey.Length < 32)
{
    throw new Exception("JWT SecretKey must be at least 32 characters long!");
}

var key = Encoding.UTF8.GetBytes(secretKey);

// ✅ JWT autentifikatsiyasini sozlash
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });


builder.Services.AddAuthorization();

// JWT dagi id, usernamelarni olish uchun foydalaniladi.
builder.Services.AddHttpContextAccessor();

// ✅ Swagger uchun JWT qo'llab-quvvatlash
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Bearer tokenini kiriting: Bearer YOUR_TOKEN",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// ✅ Xizmatlarni ro'yxatdan o'tkazish
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddServices();

// ✅ CORS qo'shish
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

// ✅ Log konfiguratsiyasi
var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

var app = builder.Build();

// Seed Data qo‘shish
using (var scope = app.Services.CreateScope())
{
    var employeeService = scope.ServiceProvider.GetRequiredService<IEmployeeService>();
    var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    var branchService = scope.ServiceProvider.GetRequiredService<IBranchService>();

    // 1. Avval filial qo‘shamiz
    var branches = await branchService.RetrieveAllAsync();
    long branchId;
    if (!branches.Any())
    {
        var defaultBranch = new BranchCreationDto
        {
            BranchName = "Default Branch",
            Location = "Default Location"
        };
        var createdBranch = await branchService.AddAsync(defaultBranch);
        branchId = createdBranch.Id;
        Console.WriteLine($"Standart filial qo‘shildi: {createdBranch.BranchName}, ID: {createdBranch.Id}");
    }
    else
    {
        branchId = (branches.FirstOrDefault())?.Id ?? throw new Exception("No branch found after retrieval");
    }

    // 2. Keyin user qo‘shamiz
    long userId;
    if (!await userService.AnyUsersAsync()) // AnyUsersAsync qo‘shish kerak
    {
        var adminSettings = builder.Configuration.GetSection("Admin");

        var adminUser = new User
        {
            FirstName = adminSettings["FirstName"],
            LastName = adminSettings["LastName"],
            Patronomyc = string.Empty,
            SeriaPasport = adminSettings["SeriaPasport"],
            DateOfBirth = DateTimeOffset.Parse(adminSettings["DateOfBirth"]).ToUniversalTime(),
            DateOfIssue = DateTimeOffset.UtcNow,
            DateOfExpiry = DateTimeOffset.UtcNow.AddYears(10),
            Gender = Gender.Erkak,
            Phone = adminSettings["Phone"],
            TelegramPhone = string.Empty,
            Address = string.Empty,
            JSHSHIR = "12345678912345",
            Role = Role.Admin,
            CreatedBy = 0, // Tizim tomonidan yaratilgan
            CreatedAt = DateTimeOffset.UtcNow,
            IsDeleted = false,
            BranchId = branchId
        };
        userService.CreateSeedUserAsync(adminUser);
        var user = await userService.GetLastUser();
        userId = user.Id;
        Console.WriteLine($"Standart user qo‘shildi: {adminUser.FirstName} {adminUser.LastName}, ID: {userId}");
    }
    else
    {
        userId = (await userService.GetLastUser())?.Id ?? throw new Exception("No user found after retrieval");
    }

    // 3. Oxirida employee qo‘shamiz
    if (!await employeeService.AnyEmployeesAsync())
    {
        var adminSettings = builder.Configuration.GetSection("Admin");

        var adminEmployee = new Employee
        {
            UserId = userId,
            BranchId = branchId,
            Username = adminSettings["Username"],
            Password = BCrypt.Net.BCrypt.HashPassword(adminSettings["Password"]),
            CreatedBy = 0, // Tizim tomonidan yaratilgan
            CreatedAt = DateTimeOffset.UtcNow,
            IsDeleted = false
        };
        await employeeService.CreateSeedEmployeeAsync(adminEmployee);

        Console.WriteLine("Standart admin qo‘shildi: " + adminSettings["Username"]);
    }
}// ✅ Middleware va API konfiguratsiyasi
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();
app.UseCors("AllowAll");

// ✅ JWT autentifikatsiya va avtorizatsiya
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
