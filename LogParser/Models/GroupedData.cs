namespace LogParser.Models;

public record GroupedData
{ 
    public GroupedData(string key, int count)
    {
        Key = key; 
        Count = count;
    }
    public string Key { get; private set; }
    public int Count { get; private set; }
}
