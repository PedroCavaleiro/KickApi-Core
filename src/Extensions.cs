using System;
using System.ComponentModel;

namespace Extensions;

/// <summary>
/// Exception when the Enum can't be parsed
/// </summary>
public class InvalidEnumException : Exception
{
    /// <summary>
    /// Default initializer when there's no data
    /// </summary>
    public InvalidEnumException() { }

    /// <summary>
    /// Default initializer, for message only
    /// </summary>
    /// <param name="message">Error message</param>
    public InvalidEnumException(string message) : base(message) { }

    /// <summary>
    /// Default initializer, for message and inner exception
    /// </summary>
    /// <param name="message">Error message</param>
    /// <param name="inner">Inner exception</param>
    public InvalidEnumException(string message, Exception inner) : base(message, inner) { }
}

/// <summary>
/// Adds extensions to classes
/// </summary>
public static class EnumExtensions {

    /// <summary>
    /// Parses a string to a enum
    /// If a num is not found throws InvalidEnumException
    /// </summary>
    /// <typeparam name="T">The type of the enum</typeparam>
    /// <param name="str">The string to be parsed</param>
    /// <returns>The enum value</returns>
    public static T ParseEnum<T>(this string str) where T : Enum {
        foreach (T e in Enum.GetValues(typeof(T))) {
            if (e.GetDescription() == str)
                return e;
        }
        throw new InvalidEnumException("The given enum type doesn't contain " + str);
    }

    /// <summary>
    /// Gets the description of the enumerator
    /// </summary>
    /// <param name="e">The enumerator</param>
    /// <returns>The description as string</returns>
    public static string GetDescription(this Enum e) {
        var eType = e.GetType();
        var eName = Enum.GetName(eType, e);
        if (eName == null)
            return null;

        var fieldInfo = eType.GetField(eName);
        if (fieldInfo == null)
            return null;

        var descriptionAttribute = Attribute.GetCustomAttribute(fieldInfo, typeof(DescriptionAttribute)) as DescriptionAttribute;
        return descriptionAttribute?.Description;
    }
}
