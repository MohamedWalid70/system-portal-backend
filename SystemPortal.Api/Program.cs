using Microsoft.EntityFrameworkCore;
using SystemPortal.Data.Context;
using SystemPortal.Repository.Repositories;
using SystemPortal.Repository.UnitOfWork;
using SystemPortal.Services.services.CompanyServices;
using SystemPortal.Services.services.OtpServices;
using SystemPortal.Services.Services.AuthServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ICompanyRepository, CompanyRepository>();

builder.Services.AddScoped<ICompanyServices, CompanyServices>();
builder.Services.AddScoped<IOtpServices, OtpServices>();
builder.Services.AddScoped<IAuthServices, AuthServices>();

builder.Services.AddCors(corsOptions => corsOptions.AddPolicy("AllowAll", policyConfig => policyConfig.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));

var app = builder.Build();

// Configure the HTTP request pipeline.
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
