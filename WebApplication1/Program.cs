using Microsoft.AspNetCore.Mvc;
using NLog.Web;
using UnitOfLogging;
using UnitOfLogging.Logging;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Host.UseNLog();


builder.Services
    .UseUnitOfLogging()
    .Configure(opt => opt.UseDefaultPresets(c => c.SeqLog = false))
    .InitLoggers();


builder.Services
    .UseUnitOfLogging()
    .UseJsonSettings(builder.Configuration,"LogSettings")
    .Configure(opt => opt.UseDefaultPresets())
    .InitLoggers(); 



//builder.Services.UseUnitOfLogging().UseMyUnitOfLogging(builder.Configuration, "LogSettings"/*,
//    options =>  
//    {
//        options.AddTargets(tg =>
//        {
//            tg.AddConsoleLogging(col =>
//            {
//                col.DefaultConsoleLogSettings = true;
//                col.IsActive = true;
//            });
//           // tg.AddDefaultFileLogging();
//        });
//    }*/
//).UseDefaultPresets(c => c.SeqLog = false).InitLoggers();


//todo
/*
re estructuralo para poder analizar el settings correctamente
crear mapa o flujo para cada logger
implementar creacion de log manual
verificar si creamos un singleton o scope cosa que permitamos el registro de errores sin inyectar nada directamente
crear directamente un loggeo en frio, para logear despues de que se devuelva la peticion

 
 */





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







