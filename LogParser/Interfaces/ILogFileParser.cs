﻿using LogParser.Models;

namespace LogParser.Interfaces;

internal interface ILogFileParser
{
    public string FileName { get; }
    public int Lines { get; }
    public List<int> EmptyLines { get; }
    public List<LogData> Data { get; }
    public List<(int line, string value)> LogErrors { get; }

    public void ParseFile(string fileName);
    public void ReportSummary();
    public void Report();
}
