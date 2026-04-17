using Application.Interfaces;
using Application.Queries;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Commands;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//custom
builder.Services.AddDbContext<AppDbContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IGetEventCatalogQuery, GetEventCatalogQuery>();
builder.Services.AddScoped<ICreateEventCommand, CreateEventCommand>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



