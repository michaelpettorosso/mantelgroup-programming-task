namespace LogReporter.Classes;
public sealed class LogReportOptions
{
    public string FileName { get; set; } = string.Empty;
    public int TopNumberOfUrls { get; set; } = 3;
    public int TopNumberOfIpAddresses { get; set; } = 3;
    public bool ShowReportSummary { get; set; } = false;
    public bool ShowLogErrorDetail { get; set; } = false;
}