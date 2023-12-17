using LogReporter.Interfaces;
using LogReporter.Models;
using Microsoft.Extensions.Options;
using System.Text;
using System.Text.RegularExpressions;
using static LogReporter.Helpers.RegExHelper;
using static LogReporter.Extensions.ReportingExtensions;

namespace LogReporter.Classes;

public class LogParser(ILogFileManager logFileManager, IReporter reporter, IOptions<LogReportOptions> reportOptions) : ILogParser
{
    private readonly ILogFileManager _logFileManager = logFileManager;
    private readonly IReporter _reporter = reporter;
    private readonly LogReportOptions _reportOptions = reportOptions.Value;
    private string _fileName = string.Empty;
    private readonly TextWriter _writer = Console.Out;

    /// <summary>
    /// Returns Regex Pattern used to parse logfile
    /// Use space as delimiter between fields
    /// - represents no value
    /// 
    /// https://regex101.com/r/zJUEdQ/2
    /// (?<IpAddress>[^ ]+) ([^ ]+) (?<User>[^ ]+) \[(?<DateTime>[^\]]+)\] ""(?<Method>[^ ]+) (?<Url>[^ ]+) (?<Protocol>[^""]+)"" (?<StatusCode>[^ ]+) ([^ ]+) ""([^""]+)"" ""(?<UserAgent>[^""]+)"" ?(?<Extra>.*)
    /// 50.112.00.11 - admin [11/Jul/2018:17:31:56 +0200] "GET /asset.js HTTP/1.1" 200 3574 "-" "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1092.0 Safari/536.6"
    /// </summary>
    private string Pattern()
    {
        var sb = new StringBuilder();
        //Ip Address (IPv4) eg. 192.168.1.0 
        //(?<IpAddress>[^ ]+) 
        sb.Append(NamedGroupFieldWithoutCharacter(Constants.Group.IpAddress, Space));

        //? eg. -
        //([^ ] +)
        sb.Append(GroupFieldWithoutCharacter(Space)); 

        //User (Can be -) eg. admin
        //(?<User>[^ ]+) 
        sb.Append(NamedGroupFieldWithoutCharacter(Constants.Group.User, Space));

        //Date and Time with offset (exclude [..]) eg. [11/Jul/2018:17:31:56 +0200]
        //\[ 
        sb.Append(SquareBracket);
        //(?<DateTime>[^\]]+)\]
        sb.Append(NamedGroupFieldWithoutCharacter(Constants.Group.DateTime, ClosingSquareBracket));

        //Request (exclude "..") eg. "GET /intranet-analytics/ HTTP/1.1" => Method=GET, Path=/intranet-analytics/, Protocol=HTTP/1.1
        //"" 
        sb.Append(Quote);
        //(?<Method>[^ ]+)
        sb.Append(NamedGroupFieldWithoutCharacter(Constants.Group.Method, Space));
        //(?<Url>[^ ]+) 
        sb.Append(NamedGroupFieldWithoutCharacter(Constants.Group.Url, Space));
        //(?<Protocol>[^""]+)"" 
        sb.Append(NamedGroupFieldWithoutCharacter(Constants.Group.Protocol, Quote));

        //Status Code eg. 200
        //(?<StatusCode>[^ ]+) 
        sb.Append(NamedGroupFieldWithoutCharacter(Constants.Group.StatusCode, Space));

        //? eg. -
        //([^ ]+) 
        sb.Append(GroupFieldWithoutCharacter(Space));

        //? eg. "-"
        //""
        sb.Append(Quote);
        //([^""]+)""
        sb.Append(GroupFieldWithoutCharacter(Quote));

        //User Agent (exclude "..") eg. "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1092.0 Safari/536.6"
        //""
        sb.Append(Quote);
        //(?<UserAgent>[^""]+)""
        sb.Append(NamedGroupFieldWithoutCharacter(Constants.Group.UserAgent, Quote)); 

        //Optional Extra data at end
        sb.Append("?(?<Extra>.*)"); 
        return sb.ToString();
    }

    public string FileName => _fileName;
    public List<LogData> Data { get; private set; } = [];
    public int Lines { get; private set; } = 0;
    public List<int> EmptyLines { get; private set; } = [];
    public List<(int line, string value)> LogErrors { get; private set; } = [];

    /// <summary>
    /// Read file and parse set FileName, Data. Lines, EmptyLines and LogErrors
    /// </summary>
    /// <param name="fileName"></param>
    public void ParseFile(string fileName)
    {
        _fileName = fileName;
        string? line;
        string pattern = Pattern();
        string appPath = AppDomain.CurrentDomain.BaseDirectory;

        using var reader = _logFileManager.StreamReader($"{appPath}\\data\\{fileName}");

        while ((line = reader.ReadLine()) != null)  // Check for end of file
        {
            Lines++;
            if (!string.IsNullOrEmpty(line.Trim()))
            {
                Match result = Regex.Match(line, pattern, RegexOptions.ExplicitCapture);
                if (result.Success)
                {
                    var logDataRow = new LogData()
                    {
                        IpAddress = result.Groups[Constants.Group.IpAddress].Value,
                        User = result.Groups[Constants.Group.User].Value,
                        DateTime = result.Groups[Constants.Group.DateTime].Value,
                        Method = result.Groups[Constants.Group.Method].Value,
                        Url = result.Groups[Constants.Group.Url].Value,
                        Protocol = result.Groups[Constants.Group.Protocol].Value,
                        StatusCode = result.Groups[Constants.Group.StatusCode].Value,
                        UserAgent = result.Groups[Constants.Group.UserAgent].Value,
                    };
                    Data.Add(logDataRow);
                }
                else
                    LogErrors.Add((Lines, line));
            }
            else
                EmptyLines.Add(Lines);
        }
    }

    /// <summary>
    /// Present a Report Summary
    /// Only show if ShowReportSummary is true
    /// </summary>
    public void ReportSummary()
    {
        if (!_reportOptions.ShowReportSummary) return;

        _reporter.Header($"Read log file '{FileName}'");
        _reporter.Summary($"{Lines} line(s)");
        _reporter.Summary($"{Data.Count} record(s)");

        //Empty Lines ie. only whitespace 
        _reporter.Summary($"{EmptyLines.Count} empty line(s)");
        if (EmptyLines.Count > 0)
            _reporter.Summary($"  on row(s) {string.Join(",", EmptyLines)}");

        //Log Errors
        _reporter.Summary($"{LogErrors.Count} row(s) with errors");
        if (LogErrors.Count > 0 && _reportOptions.ShowLogErrorDetail)
        {
            LogErrors.ToList().ForEach(x => _reporter.Summary($"    ({x.line}) {x.value}"));
        }

        _reporter.Break();
    }

    /// <summary>
    /// Present required Report metrics
    /// </summary>
    public void Report()
    {
        _reporter.Header($"Number of Unique IP Addresses");
        _reporter.Summary($"{Data.NumberOfUniqueIpAddresses()}");
        _reporter.Break();

        _reporter.Header($"Top {_reportOptions.TopNumberOfUrls} most visited Urls");
        Data.TopVisitedUrls(_reportOptions.TopNumberOfUrls).ToList().ForEach(x => _reporter.Summary($"{x.Key} ({x.Count})"));
        _reporter.Break();

        _reporter.Header($"Top {_reportOptions.TopNumberOfIpAddresses} most active IP Addresses");
        Data.TopIpAddresses(_reportOptions.TopNumberOfIpAddresses).ToList().ForEach(x => _reporter.Summary($"{x.Key} ({x.Count})"));
        _reporter.Break();
    }
}