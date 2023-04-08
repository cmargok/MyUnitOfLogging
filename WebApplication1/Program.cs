using Microsoft.AspNetCore.Mvc;
using NLog.Web;
using UnitOfLogging;
using UnitOfLogging.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseNLog();


builder.Services.UseUnitOfLogging().UseMyUnitOfLogging(builder.Configuration, "LogSettings"/*,
    options =>  
    {
        options.AddTargets(tg =>
        {
            tg.AddConsoleLogging(col =>
            {
                col.DefaultConsoleLogSettings = true;
                col.IsActive = true;
            });
           // tg.AddDefaultFileLogging();
        
        });

        

    }*/
).UseDefaultPresets(c => c.SeqLog = false).InitLoggers();









builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
