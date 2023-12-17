using LogParser.Classes;
using LogParser.Interfaces;
using Moq;

namespace LogParser.Test;
public class ReporterTests
{
    private readonly Mock<IReporterOutput> mockReporterOutput = new();
    private readonly StringWriter stringWriter = new();
    private readonly Reporter reporter;
    public ReporterTests()
    {

        mockReporterOutput.Setup(x => x.TextWriter).Returns(stringWriter);
        reporter = new Reporter(mockReporterOutput.Object);
    }

    [Fact]
    public void Header_Outputs_Header_Text()
    {

        //Assign
        var text = "Header";

        //Act
        reporter.Header(text);

        //Assert
        var output = stringWriter.ToString();
        Assert.NotNull(output);
        Assert.Equal($"{text}\r\n", output);
    }

    [Fact]
    public void Summary_Outputs_Summary_Text()
    {
        //Assign
        var text = "Summary";

        //Act
        reporter.Summary(text);

        //Assert
        var output = stringWriter.ToString();
        Assert.NotNull(output);
        Assert.Equal($"    {text}\r\n", output);
    }

    [Fact]
    public void Break_Outputs_Line()
    {
        //Assign

        //Act
        reporter.Break();

        //Assert
        var output = stringWriter.ToString();
        Assert.NotNull(output);
        Assert.Equal("\r\n", output);
    }
}