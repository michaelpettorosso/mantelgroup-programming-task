using LogParser.Models;

namespace LogParser.Extensions;

public static class ReportingExtensions {
    public static int NumberOfUniqueIpAddresses(this List<LogData> data) {
       return data.Select(s => s.IpAddress).Distinct().Count();
    }

    public static IEnumerable<GroupedData> TopIpAddresses(this List<LogData> data, int topCount)
    {
        var groupedData = data.GroupBy(x => x.IpAddress)
            .Select(group => new GroupedData(group.Key, group.Count()))
            .OrderByDescending(x => x.Count);

        return groupedData.Take(topCount);
    }

    public static IEnumerable<GroupedData> TopVisitedUrls(this List<LogData> data, int topCount)
    {
        var groupedData = data.GroupBy(x => x.Url)
            .Select(group => new GroupedData(group.Key, group.Count()))
            .OrderByDescending(x => x.Count);

        return groupedData.Take(topCount);
    }

}

