namespace EF.Core.Extensions;

public static class StringExtensions
{
    public static string? EmptyToNullReplace(this string? value)
    {
        return string.IsNullOrEmpty(value?.Trim()) ? null : value;
    }

    public static string? ReplaceWithIf(this string? value, Func<string?, bool> @if, string? newValue)
    {
        if (@if(value)) return newValue;
        else return value;
    }
}
