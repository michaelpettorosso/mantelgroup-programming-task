using LogReporter.Classes;
using LogReporter.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LogReporter.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddSingleton<ILogFileManager, LogFileManager>();
        services.AddSingleton<ILogParser, LogParser>();
        services.AddSingleton<IReporterOutput, ReporterOutput>();
        services.AddSingleton<IReporter, Reporter>();
        return services;
    }
    public static HostApplicationBuilder AddConfiguration(this HostApplicationBuilder builder)
    {
        builder.Configuration.Sources.Clear();
        IHostEnvironment env = builder.Environment;
        builder.Configuration.AddJsonFile("appsettings.json", true, true)
            .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
            .AddEnvironmentVariables();
        return builder;
    }

    public static IServiceCollection AddOptions(this IServiceCollection services, ConfigurationManager configurationManager)
    {
        services.AddOptions<LogReportOptions>().Bind(configurationManager.GetSection(nameof(LogReportOptions)));
        return services;
    }
}