using LogReporter.Interfaces;

namespace LogReporter.Classes
{
    internal class LogFileManager : ILogFileManager
    {
        public StreamReader StreamReader(string path)
        {
            return new StreamReader(path);
        }
    }
}
