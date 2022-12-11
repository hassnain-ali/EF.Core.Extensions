namespace EF.Core.Extensions;

public static class AppExtensions
{
    public static IApplicationBuilder UseIf(this IApplicationBuilder app,
        bool condition,
        Func<RequestDelegate, RequestDelegate> middleware)
        => condition
            ? app.Use(middleware)
            : app;

    public static IApplicationBuilder UseIf(this IApplicationBuilder app,
        Func<bool> condition,
        Func<RequestDelegate, RequestDelegate> middleware)
        => condition()
            ? app.Use(middleware)
            : app;

    public static IApplicationBuilder UseIf(this IApplicationBuilder app,
        bool condition,
        Func<HttpContext, Func<Task>, Task> middleware)
        => condition
            ? app.Use(middleware)
            : app;

    public static IApplicationBuilder UseIf(this IApplicationBuilder app,
        Func<bool> condition,
        Func<HttpContext, Func<Task>, Task> middleware)
        => condition()
            ? app.Use(middleware)
            : app;
}