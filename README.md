# My Unit Of Logging

## Descripción

Api que se implementada para recibir logs por medios de masstransint y rabitmq, al momento se encuentra en fase de pruebas, solo admite configuraciones predeterminadas, pronto se agregara mas lectura de campos y definicion de targets escogidos.

## Instalación

descargue el repositorio, compile y ejecute

#



### Configuración

Configuracion en program. cs

```csharp
builder.AddUnitOfLogging()
    .UseJsonConfiguration("LogSettings")
    .Configure( opt =>
    {
        opt.UseDefaultPresets();       
        
    })
    .Build();
```

Para utilizar este paquete, debe agregar la configuración correspondiente en su archivo `appsettings.json` o `web.config`, como se muestra a continuación:


```json
 "LogSettings": {
    "LoggingActive": true,
    "Loggers": [
      {
        "Console": {
          "Active": true,
          "Name": "ConsoleLogger"
        },
        "File": {
          "Active": false,
          "Name": "FileLogger"
        },
        "ErrorFile": {
          "Active": true,
          "Name": "ErrorFileLogger"
        },
        "Seq": {
          "Active": false,
          "Name": "SeqLogger"
        }
      }
    ]
  }
  ```
  
### Contribuir
Si encuentra algún error o desea mejorar este repositio, no dude en enviar una solicitud de extracción o crear un problema.

### Licencia
Este paquete está bajo la Licencia MIT.

