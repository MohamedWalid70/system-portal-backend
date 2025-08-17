using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SystemPortal.Api.ExceptionHandling;
using SystemPortal.Data.Context;
using SystemPortal.Data.Entities;
using SystemPortal.Repository.Repositories.CompanyRepository;
using SystemPortal.Repository.Repositories.OtpRepository;
using SystemPortal.Repository.UnitOfWork;
using SystemPortal.Services.Services.AuthServices;
using SystemPortal.Services.Services.CompanyServices;
using SystemPortal.Services.Services.OtpServices;
using SystemPortal.Services.Services.UserServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();
builder.Services.AddScoped<IOtpRepository, OtpRepository>();

builder.Services.AddScoped<ICompanyServices, CompanyServices>();
builder.Services.AddScoped<IOtpServices, OtpServices>();
builder.Services.AddScoped<IAuthServices, AuthServices>();
builder.Services.AddScoped<IUserServices, UserServices>();

builder.Services.AddIdentity<AppUser, IdentityRole<int>>(options => {
        options.SignIn.RequireConfirmedAccount = false;
        options.Password.RequireLowercase = false;
        options.User.RequireUniqueEmail = true;
    }).
    AddRoles<IdentityRole<int>>().
    AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddCors(corsOptions => corsOptions.AddPolicy("AllowAll", policyConfig => policyConfig.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

//using(var scope = app.Services.CreateScope())
//{
//    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
//    string[] roleNames = { "Company", "Admin", "Employee" };

//    foreach(var role in roleNames)
//    {
//        if (!await roleManager.RoleExistsAsync(role))
//        {
//            await roleManager.CreateAsync(new IdentityRole<int>(role));
//        }
//    }
//}

// Configure the HTTP request pipeline.

app.UseMiddleware<CustomExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
