using LogReporter.Classes;
using LogReporter.Extensions;
using LogReporter.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);


builder.AddConfiguration();
builder.Services.AddOptions(builder.Configuration);
builder.Services.AddServices();

using IHost host = builder.Build();

var logParser = host.Services.GetRequiredService<ILogParser>();

var logReportOptions = new LogReportOptions();
builder.Configuration.Bind(nameof(LogReportOptions), logReportOptions);

var fileName = logReportOptions.FileName;
logParser.ParseFile(fileName);
logParser.ReportSummary();
logParser.Report();

await host.RunAsync();

