using LogParser.Interfaces;

namespace LogParser.Classes
{
    internal class ReporterOutput : IReporterOutput
    {
        public TextWriter TextWriter => Console.Out;
    }
}
