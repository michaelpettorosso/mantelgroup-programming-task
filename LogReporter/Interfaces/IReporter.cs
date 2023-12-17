namespace LogReporter.Interfaces;

public interface IReporter
{
    public void Header(string value);
    public void Summary(string value);
    public void Break();
}
