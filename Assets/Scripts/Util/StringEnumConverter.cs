using System;

public class StringEnumConverter
{
    public static T ParserStringToEnum<T>(string context) where T : struct, Enum
    {
        T enumValue;
        return Enum.TryParse(context, out enumValue) ? enumValue : default(T);
    }
}