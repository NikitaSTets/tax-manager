using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Services;
using Services.Interfaces;
using System.Text.Json.Serialization;
using TaxManager;
using TaxManager.Extensions;
using TaxManager.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.IgnoreNullValues = true;
}); ;

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();
builder.Services.AddTransient<ITaxRuleService, TaxRuleService>();
builder.Services.AddTransient<ICityService, CityService>();
builder.Services.AddTransient<IAuthenticationService, AuthenticationService>();

builder.Services.AddDBExtensions(builder.Configuration.GetConnectionString("TaxDb"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var taxContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<TaxContext>>();
    var taxContext = await taxContextFactory.CreateDbContextAsync();
    var isExist = taxContext.GetService<IDatabaseCreator>().CanConnect();
    if (!isExist)
    {
        await taxContext.Database.EnsureCreatedAsync();
    }

    taxContext.Seed();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

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
