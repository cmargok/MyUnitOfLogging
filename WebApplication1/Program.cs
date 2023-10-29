using K.Logger.Client.Provider;
using K.Logger.Core.Tools;
using Serilog;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Host.UseNLog();
builder.Host.UseSerilog();

builder.Services.AddKLogClient()
    .AddSettings(builder.Configuration.GetKLogConfig())
    .RegisterKLog();






builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();







