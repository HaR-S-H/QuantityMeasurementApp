using Microsoft.EntityFrameworkCore;
using QuantityMeasurementApp.Business;
using QuantityMeasurementApp.Middleware;
using QuantityMeasurementApp.Repository;
using QuantityMeasurementApp.Repository.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<QuantityMeasurementDbContext>(options =>
    options.UseInMemoryDatabase("QuantityMeasurementDb")
);

builder.Services.AddScoped<IQuantityMeasurementRepository, EfQuantityMeasurementRepository>();
builder.Services.AddScoped<IQuantityMeasurementService, QuantityMeasurementServiceImpl>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();
