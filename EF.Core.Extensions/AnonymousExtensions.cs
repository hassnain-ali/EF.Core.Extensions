namespace EF.Core.Extensions;

public static class AnonymousExtensions
{
    public static T DefaultIfEmpty<T>(this T? value, T @default) => value ?? @default;
}