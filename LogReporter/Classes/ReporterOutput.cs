using LogReporter.Interfaces;

namespace LogReporter.Classes
{
    internal class ReporterOutput : IReporterOutput
    {
        public TextWriter TextWriter => Console.Out;
    }
}
