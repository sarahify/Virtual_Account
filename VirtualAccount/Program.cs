using Microsoft.Extensions.Logging.Configuration;
using Serilog;
using VirtualAccount.Context;
using VirtualAccount.Contract;
using VirtualAccount.Dto;
using VirtualAccount.Repositories;
using VirtualAccount.Services.BackgroundJob;
using VirtualAccount.Services.Implementation;
using VirtualAccount.Services.Interface;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Host.UseSerilog();

builder.Services.AddScoped<IVirtualAccountRepo, VirtualAccountRepo>();
builder.Services.AddScoped<IVirtualAccountService, VirtualAccountService>();
builder.Services.AddScoped<DapperContext>();
builder.Services.AddServices();
builder.Services.AddTransient<PayStackCreateCustomerRequest>();

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration).CreateLogger();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
