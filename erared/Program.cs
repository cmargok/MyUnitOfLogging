var builder = WebApplication.CreateBuilder(args);

builder.Services.UseUnitOfLogging().UseMyUnitOfLogging(builder.Configuration, options =>
{
    options.LogSectionName = "LogSettings";
    options.AddTargets(po =>
    {
        po.DefaultConsoleLogSettings = true;
        po.ConsoleConfiguration.ConsoleTargetConfig = new NLog.Targets.ColoredConsoleTarget
        {

        };
    });
});