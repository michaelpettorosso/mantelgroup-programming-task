using LogParser.Models;
using static LogParser.Extensions.ReportingExtensions;

namespace LogParser.Test;
public class ReportingExtensionsTests
{
    public ReportingExtensionsTests()
    {

    }
    [Theory]
    [InlineData(new string[] { "192.168.1.1" }, 1)]
    [InlineData(new string[] { "192.168.1.1", "192.168.1.1" }, 1)]
    [InlineData(new string[] { "192.168.1.1", "192.168.1.2" }, 2)]
    [InlineData(new string[] { "192.168.1.1", "192.168.1.2", "192.168.1.1" }, 2)]
    [InlineData(new string[] { "192.168.1.1", "192.168.1.2", "192.168.1.1", "192.168.1.2" }, 2)]
    [InlineData(new string[] { "192.168.1.1", "192.168.1.2", "192.168.1.3" }, 3)]
    [InlineData(new string[] { "192.168.1.1", "192.168.1.2", "192.168.1.3", "192.168.1.1" }, 3)]
    [InlineData(new string[] { "192.168.1.1", "192.168.1.2", "192.168.1.3", "192.168.1.1", "192.168.1.2" }, 3)]
    [InlineData(new string[] { "192.168.1.1", "192.168.1.2", "192.168.1.3", "192.168.1.1", "192.168.1.2", "192.168.1.3" }, 3)]
    [InlineData(new string[] { "192.168.1.1", "192.168.1.2", "192.168.1.3", "192.168.1.4", "192.168.1.5" }, 5)]
    public void NumberOfUniqueIpAddresses_Should_Return_Correct_Number(string[] ipAddresses, int uniqueIpAddresses)
    {

        //Assign
        List<LogData> logData = [];

        foreach (var ipAddress in ipAddresses) {
            logData.Add(new LogData { IpAddress = ipAddress, Url = "/" });
        }

        //Act
        var numberOfUniqueIpAddresses = logData.NumberOfUniqueIpAddresses();

        //Assert
        Assert.Equal(uniqueIpAddresses, numberOfUniqueIpAddresses);
    }
    public class TopTheoryData<T1, T2> : TheoryData<T1, T2>
    {
        public TopTheoryData((T1 values, T2 groupData)[] theoryData)
        {
            foreach (var data in theoryData)
            {
                Add(data.values, data.groupData);
            }
        }
    }

    public static TopTheoryData<string[], GroupedData[]> TopIpAddressesData = new([
        new() { values = ["192.168.1.1"], groupData = [new GroupedData("192.168.1.1", 1)] },
        new() { values = ["192.168.1.1", "192.168.1.1"], groupData = [new GroupedData("192.168.1.1", 2)] },
        new() { values = ["192.168.1.1", "192.168.1.1", "192.168.1.2"], groupData = [new GroupedData("192.168.1.1", 2), new GroupedData("192.168.1.2", 1)] },
        new() { values = ["192.168.1.1", "192.168.1.2", "192.168.1.1", "192.168.1.2"], groupData = [new GroupedData("192.168.1.1", 2), new GroupedData("192.168.1.2", 2)] },
        new() { values = ["192.168.1.1", "192.168.1.2", "192.168.1.3"], groupData = [new GroupedData("192.168.1.1", 1), new GroupedData("192.168.1.2", 1), new GroupedData("192.168.1.3", 1)] },
        new() { values = ["192.168.1.1", "192.168.1.2", "192.168.1.3", "192.168.1.1"], groupData = [new GroupedData("192.168.1.1", 2), new GroupedData("192.168.1.2", 1), new GroupedData("192.168.1.3", 1)] },
        new() { values = ["192.168.1.1", "192.168.1.2", "192.168.1.3", "192.168.1.1", "192.168.1.2"], groupData = [new GroupedData("192.168.1.1", 2), new GroupedData("192.168.1.2", 2), new GroupedData("192.168.1.3", 1)] },
        new() { values = ["192.168.1.1", "192.168.1.2", "192.168.1.3", "192.168.1.1", "192.168.1.2", "192.168.1.3"], groupData = [new GroupedData("192.168.1.1", 2), new GroupedData("192.168.1.2", 2), new GroupedData("192.168.1.3", 2)] },
        new() { values = ["192.168.1.1", "192.168.1.2", "192.168.1.3", "192.168.1.4"], groupData = [new GroupedData("192.168.1.1", 1), new GroupedData("192.168.1.2", 1), new GroupedData("192.168.1.3", 1)] }
        ]);

    [Theory]
    [MemberData(nameof(TopIpAddressesData))]
    public void TopIpAddresses_Should_Return_Correct_Group_Data(string[] ipAddresses, GroupedData[] groupData)
    { 

        //Assign
        List<LogData> logData = [];

        foreach (var ipAddress in ipAddresses)
        {
            logData.Add(new LogData { IpAddress = ipAddress, Url = "/" });
        }

        //Act
        var topIpAdresses = logData.TopIpAddresses(3);

        //Assert
        Assert.Equal(groupData.Count(), topIpAdresses.Count());
        Assert.Equal(groupData, topIpAdresses);
    }

    public static TopTheoryData<string[], GroupedData[]> TopUrlsData = new([
        new() { values = ["/url1/"], groupData = [new GroupedData("/url1/", 1)] },
        new() { values = ["/url1/", "/url1/"], groupData = [new GroupedData("/url1/", 2)] },
        new() { values = ["/url1/", "/url1/", "/url2/"], groupData = [new GroupedData("/url1/", 2), new GroupedData("/url2/", 1)] },
        new() { values = ["/url1/", "/url2/", "/url1/", "/url2/"], groupData = [new GroupedData("/url1/", 2), new GroupedData("/url2/", 2)] },
        new() { values = ["/url1/", "/url2/", "/url3/"], groupData = [new GroupedData("/url1/", 1), new GroupedData("/url2/", 1), new GroupedData("/url3/", 1)] },
        new() { values = ["/url1/", "/url2/", "/url3/","/url1/"], groupData = [new GroupedData("/url1/", 2), new GroupedData("/url2/", 1), new GroupedData("/url3/", 1)] },
        new() { values = ["/url1/", "/url2/", "/url3/", "/url1/", "/url2/"], groupData = [new GroupedData("/url1/", 2), new GroupedData("/url2/", 2), new GroupedData("/url3/", 1)] },
        new() { values = ["/url1/", "/url2/", "/url3/", "/url1/", "/url2/", "/url3/"], groupData = [new GroupedData("/url1/", 2), new GroupedData("/url2/", 2), new GroupedData("/url3/", 2)] },
        new() { values = ["/url1/", "/url2/", "/url3/", "/url4/"], groupData = [new GroupedData("/url1/", 1), new GroupedData("/url2/", 1), new GroupedData("/url3/", 1)] }
        ]);

    [Theory]
    [MemberData(nameof(TopUrlsData))]
    public void TopVisitedUrls_Should_Return_Correct_Group_Data(string[] urls, GroupedData[] groupData)
    {

        //Assign
        List<LogData> logData = [];

        foreach (var url in urls)
        {
            logData.Add(new LogData { IpAddress = "192.168.1.1", Url = url });
        }

        //Act
        var topVisitedUrls = logData.TopVisitedUrls(3);

        //Assert
        Assert.Equal(groupData.Count(), topVisitedUrls.Count());
        Assert.Equal(groupData, topVisitedUrls);
    }
}