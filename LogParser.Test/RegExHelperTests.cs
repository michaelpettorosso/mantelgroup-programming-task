using LogParser.Helpers;
using System.Text.RegularExpressions;

namespace LogParser.Test;
public class RegExHelperTests
{
    const string TestStringValue = nameof(TestStringValue);

    [Theory]
    [InlineData(TestStringValue, " ", RegExHelper.Space)]
    [InlineData(TestStringValue, @"""", RegExHelper.Quote)]
    [InlineData(TestStringValue, "]", @"\]")]
    public void FieldWithoutCharacter_Should_Return_String_Up_To_Character(string value, string character, string regExCharacter)
    {
       
        //Assign
        var stringToTest = value + character;

        //Act
        Match result = Regex.Match(stringToTest, RegExHelper.FieldWithoutCharacter(regExCharacter));

        //Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(value, result.Value);
    }

    [Theory]
    [InlineData("192.168.1.200", "", RegExHelper.Space)]
    [InlineData(" 192.168.1.200", RegExHelper.Space, RegExHelper.Space)]
    [InlineData(@"""Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/5.0)""", RegExHelper.Quote, RegExHelper.Quote)]
    [InlineData("[10/Jul/2018:22:01:17 +0200]", RegExHelper.SquareBracket, RegExHelper.ClosingSquareBracket)]
    public void GroupFieldWithoutCharacter_Should_Return_String_Group_Up_To_Character(string value, string startRegExCharacter, string regExCharacter)
    {

        //Assign
        var stringToTest = value + RegExHelper.Space;
        var startIndex = 0;
        if (startRegExCharacter != "")
            startIndex = 1;
        var endIndex = 1;
        if (regExCharacter != RegExHelper.Space)
            endIndex = 2;
        //Act
        Match result = Regex.Match(stringToTest, startRegExCharacter + RegExHelper.GroupFieldWithoutCharacter(regExCharacter));

        //Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(stringToTest, result.Value);
        Assert.Equal(stringToTest[startIndex..^endIndex], result.Groups[1].Value);
    }

    [Theory]
    [InlineData("IpAddress","192.168.1.200", "", RegExHelper.Space)]
    [InlineData("IpAddress", " 192.168.1.200", RegExHelper.Space, RegExHelper.Space)]
    [InlineData("UserAgent", @"""Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.1; Trident/5.0)""", RegExHelper.Quote, RegExHelper.Quote)]
    [InlineData("DateTime", "[10/Jul/2018:22:01:17 +0200]", RegExHelper.SquareBracket, RegExHelper.ClosingSquareBracket)]
    public void NamedGroupFieldWithoutCharacter_Should_Return_String_Group_Up_To_Character(string groupName, string value, string startRegExCharacter, string regExCharacter)
    {

        //Assign
        var stringToTest = value + RegExHelper.Space;
        var startIndex = 0;
        if (startRegExCharacter != "")
            startIndex = 1;
        var endIndex = 1;
        if (regExCharacter != RegExHelper.Space)
            endIndex = 2;

        //Act
        Match result = Regex.Match(stringToTest, startRegExCharacter + RegExHelper.NamedGroupFieldWithoutCharacter(groupName, regExCharacter));

        //Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        Assert.Equal(stringToTest, result.Value);
        Assert.Equal(stringToTest[startIndex..^endIndex], result.Groups[groupName].Value);
    }

    [Theory]
    [InlineData(RegExHelper.Space, RegExHelper.Space)]
    [InlineData(RegExHelper.Quote, RegExHelper.Quote + RegExHelper.Space)]
    [InlineData(RegExHelper.ClosingSquareBracket, RegExHelper.ClosingSquareBracket + RegExHelper.Space)]
    public void AddSpaceToGroup_Should_Return_Character_And_Space(string regExCharacter, string result)
    {

        //Assign
        
        //Act
        var value = RegExHelper.AddSpaceToGroup(regExCharacter);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(result, value);
    }
}