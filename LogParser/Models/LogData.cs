namespace LogParser.Models;

public class LogData
{ 
    public required string IpAddress { get; set; }
    public string User { get; set; } = string.Empty;
    public string DateTime { get; set; } = string.Empty;
    public string Method { get; set; } = string.Empty;
    public required string Url { get; set; }
    public string Protocol { get; set; } = string.Empty;
    public string StatusCode { get; set; } = string.Empty;
    public string UserAgent { get; set; } = string.Empty;
}
