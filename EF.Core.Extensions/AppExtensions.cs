using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;

namespace EF.Core.Extensions;

#if NET6_0_OR_GREATER
public static class AppExtensions
{
    public static IApplicationBuilder AddIf(this IApplicationBuilder app,
        bool condition,
        Func<RequestDelegate, RequestDelegate> middleware)
        => condition
            ? app.Use(middleware)
            : app;

    public static IApplicationBuilder AddIf(this IApplicationBuilder app,
        Func<bool> condition,
        Func<RequestDelegate, RequestDelegate> middleware)
        => condition()
            ? app.Use(middleware)
            : app;
}
#endif