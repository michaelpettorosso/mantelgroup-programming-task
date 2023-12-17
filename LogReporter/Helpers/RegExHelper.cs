namespace LogReporter.Helpers;

public static class RegExHelper
{
    public const string Space = @" ";
    public const string Quote = @"""";
    public const string SquareBracket = @"\[";
    public const string ClosingSquareBracket = @"\]";

    public static string FieldWithoutCharacter(string character)
    {
        return $"[^{character}]+";
    }
    /// <summary>
    /// Add an additional Space to Group if not a Space
    /// </summary>
    /// <param name="character"></param>
    /// <returns></returns>
    public static string AddSpaceToGroup(string character)
    {
        return (character != Space ? character + Space : character);
    }

    public static string NamedGroupFieldWithoutCharacter(string name, string character)
    {
        return $"(?<{name}>{FieldWithoutCharacter(character)}){AddSpaceToGroup(character)}";
    }

    public static string GroupFieldWithoutCharacter(string character)
    {
        return $"({FieldWithoutCharacter(character)}){AddSpaceToGroup(character)}";
    }

    
}
