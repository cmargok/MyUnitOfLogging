# My Unit Of Logging

## Descripción

El paquete NuGet proporciona un sistema de logging preconfigurado con NLog que permite la salida a la consola, un archivo y Seq y posteriormente a otros blancos de logueo

## Instalación

Puede instalar este paquete NuGet mediante la consola del Administrador de paquetes de Visual Studio o mediante la línea de comandos de NuGet.

### Consola del Administrador de paquetes

1. Abra la solución en Visual Studio.
2. Seleccione el proyecto donde desea instalar el paquete.
3. Haga clic derecho sobre el proyecto y seleccione "Administrador de paquetes NuGet".
4. En la pestaña "Examinar", busque "MyUnitOfLogging".
5. Seleccione el paquete en la lista y haga clic en "Instalar".

### Línea de comandos de NuGet

1. Abra la consola del sistema.
2. Navegue hasta la carpeta del proyecto donde desea instalar el paquete.
3. Ejecute el siguiente comando:

```powershell
Install-Package MyUnitOfLogging
```

### Configuración

Para utilizar este paquete, debe agregar la configuración correspondiente en su archivo `appsettings.json` o `web.config`, como se muestra a continuación:
```json
 "LogSettings": {
    "LoggingActive": true,
    "Loggers": [
      {
        "Active": true,
        "Name": "ConsoleLogger"
      },
      {
        "Active": true,
        "Name": "FileLogger"
      },
      {
        "Active": true,
        "Name": "SeqLogger"
      }
    ]
  }
  ```
  
### Contribuir
Si encuentra algún error o desea mejorar este paquete, no dude en enviar una solicitud de extracción o crear un problema.

### Licencia
Este paquete está bajo la Licencia MIT.

