namespace EF.Core.Extensions;

public static class BoolExtensions
{
    public static bool FalseIfNull(this bool? value) => value ?? false;

    public static bool TrueIfNull(this bool? value) => value ?? true;

    public static bool DefaultIfNull(this bool? value, bool @default) => value ?? @default;
}
