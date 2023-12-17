using LogParser.Classes;
using LogParser.Interfaces;
using Microsoft.Extensions.Options;
using Moq;
using System.Text;

namespace LogParser.Test;
public class LogParserTests
{
    private readonly Mock<ILogFileManager> mockLogFileManager = new();
    private readonly Mock<IReporter> mockReporter = new();
    private readonly IOptions<LogReportOptions> _reportOptions = Options.Create(new LogReportOptions());

    private const string validLogFileRow1 = "192.168.1.1 - admin [17/Dec/2023:17:31:56 +1100] \"GET /path1 HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1092.0 Safari/536.6\"\r\n";
    private const string validLogFileRow2 = "192.168.1.1 - admin [17/Dec/2023:17:31:58 +1100] \"GET /path2/ HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1092.0 Safari/536.6\"\r\n";
    private const string invalidLogFileRow1 = "192.168.1.1 - admin [17/Dec/2023:17:31:59 +1100] \"GET  /asset.js HTTP/1.1\" 200 3574 \"-\" \"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1092.0 Safari/536.6\"\r\n";
    private const string invalidLogFileRow2 = "192.168.1.1 - admin [17/Dec/2023:17:32:03 +1100] \"GET /asset.js HTTP/1.1\" 200 \"-\" \"Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/536.6 (KHTML, like Gecko) Chrome/20.0.1092.0 Safari/536.6\"\r\n";
    private const string emptyRow = "\r\n";

    private readonly LogFileParser logFileParser;

    private StreamReader CreateFileStream(string data)
    {
        byte[] fileBytes = Encoding.UTF8.GetBytes(data);
        MemoryStream fakeMemoryStream = new(fileBytes);
        return new StreamReader(fakeMemoryStream);
    }
    public LogParserTests()
    {
        logFileParser = new LogFileParser(mockLogFileManager.Object, mockReporter.Object, _reportOptions);
    }
    [Fact]
    public void ParseFile_Should_Set_FileName()
    {

        //Assign
        var fileName = "TestFile";
        mockLogFileManager.Setup(fileManager => fileManager.StreamReader(It.IsAny<string>()))
                       .Returns(() => CreateFileStream(""));

        //Act
        logFileParser.ParseFile(fileName);

        //Assert
        Assert.NotNull(logFileParser.FileName);
        Assert.Equal(fileName, logFileParser.FileName);
    }

    [Theory]
    [InlineData(validLogFileRow1, 1, 1, 0, 0)]
    [InlineData(validLogFileRow1 + validLogFileRow2, 2, 2, 0, 0)]
    [InlineData(validLogFileRow1 + validLogFileRow2 + invalidLogFileRow1, 2, 3, 0, 1)]
    [InlineData(validLogFileRow1 + validLogFileRow2 + invalidLogFileRow1 + invalidLogFileRow2, 2, 4, 0, 2)]
    [InlineData(validLogFileRow1 + emptyRow, 1, 2, 1, 0)]
    [InlineData(validLogFileRow1 + emptyRow + validLogFileRow2, 2, 3, 1, 0)]
    [InlineData(validLogFileRow1 + emptyRow + validLogFileRow2 + validLogFileRow1, 3, 4, 1, 0)]
    [InlineData(validLogFileRow1 + emptyRow + validLogFileRow2 + validLogFileRow1 + invalidLogFileRow1, 3, 5, 1, 1)]
    public void ParseFile_With_Log_Data_Rows_Should_Set_Data_Lines_EmptyLines_And_LogErrors(string logData, int dataRecords, int lines, int emptyLines, int logErrors)
    {

        //Assign
        var fileName = "TestFile";
        mockLogFileManager.Setup(fileManager => fileManager.StreamReader(It.IsAny<string>()))
                       .Returns(() => CreateFileStream(logData));

        //Act
        logFileParser.ParseFile(fileName);

        //Assert
        Assert.Equal(dataRecords, logFileParser.Data.Count);
        Assert.Equal(lines, logFileParser.Lines);
        Assert.Equal(emptyLines, logFileParser.EmptyLines.Count);
        Assert.Equal(logErrors, logFileParser.LogErrors.Count);
    }
}