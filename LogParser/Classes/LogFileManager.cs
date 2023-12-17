using LogParser.Interfaces;

namespace LogParser.Classes
{
    internal class LogFileManager : ILogFileManager
    {
        public StreamReader StreamReader(string path)
        {
            return new StreamReader(path);
        }
    }
}
