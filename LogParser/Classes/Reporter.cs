using LogParser.Interfaces;

namespace LogParser.Classes
{
    public class Reporter(IReporterOutput reporterOutput) : IReporter
    {
        private readonly IReporterOutput _reporterOutput = reporterOutput;

        public void Header(string value)
        {
            _reporterOutput.TextWriter.WriteLine(value);
        }

        public void Summary(string value)
        {
            _reporterOutput.TextWriter.WriteLine($"    {value}");
        }
        public void Break()
        {
            _reporterOutput.TextWriter.WriteLine();
        }
    }
}
