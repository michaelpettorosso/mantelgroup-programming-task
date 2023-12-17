namespace LogReporter.Interfaces;
public interface ILogFileManager
{
    StreamReader StreamReader(string path);
}
