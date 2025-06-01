using CashBook.Domain.Entities;
using CashBook.Infra.Configuration;
using CashBook.Infra.Contexts;
using CashBook.Infra.Extensions;
using CashBook.Infra.Interfaces;
using CashBook.Infra.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//DI
var connectionString = builder.Configuration.GetSection("ConnectionString").Value;

builder.Services.AddControllers();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddManagerContext(connectionString ?? throw new InvalidOperationException("Connection string 'DefaultConnection' is not defined."));
builder.Services.AddEntityConfiguration();


// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();
app.UseHttpsRedirection();
app.Run();

