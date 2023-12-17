using LogParser.Classes;
using LogParser.Extensions;
using LogParser.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);


builder.AddConfiguration();
builder.Services.AddOptions(builder.Configuration);
builder.Services.AddServices();

using IHost host = builder.Build();

var logFileParser = host.Services.GetRequiredService<ILogFileParser>();

var logReportOptions = new LogReportOptions();
builder.Configuration.Bind(nameof(LogReportOptions), logReportOptions);

var fileName = logReportOptions.FileName;
logFileParser.ParseFile(fileName);
logFileParser.ReportSummary();
logFileParser.Report();

await host.RunAsync();

