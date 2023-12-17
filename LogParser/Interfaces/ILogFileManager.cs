namespace LogParser.Interfaces;
public interface ILogFileManager
{
    StreamReader StreamReader(string path);
}
